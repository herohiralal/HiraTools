#include "NativeStringBuilder.h"

#include "Memory/Memory.h"

uint8 StringFormatters::Integer::MaxPossibleDigitCountFromBase(const EIntegerBaseType Base)
{
    switch (Base)
    {
        case EIntegerBaseType::Binary:
            return 64;
        case EIntegerBaseType::Octal:
            return 22;
        case EIntegerBaseType::Decimal:
            return 20;
        case EIntegerBaseType::HexaDecimal:
            return 16;
        default:
            return 64;
    }
}

NativeStringBuilder::NativeStringBuilder(const size InBufferSize) : Buffer(new wchar[InBufferSize + 1]{TEXT('\0')}), BufferSize(InBufferSize), NullTerminationCharacterIndex(0)
{
}

NativeStringBuilder::~NativeStringBuilder()
{
    delete[] Buffer;
    Buffer = nullptr;
    BufferSize = 0;
    NullTerminationCharacterIndex = -1;
}

void NativeStringBuilder::AdjustBufferForTotalStringLength(const size NewSize)
{
    if (NewSize > BufferSize)
    {
        wchar* NewBuffer = new wchar[NewSize + 1];
        SMemory::Copy(NewBuffer, Buffer, NullTerminationCharacterIndex + 1);

        delete[] Buffer;
        Buffer = NewBuffer;
        BufferSize = NewSize;
    }
}

void NativeStringBuilder::AdjustBufferForAppendingStringOfLength(const size Delta)
{
    AdjustBufferForTotalStringLength(NullTerminationCharacterIndex + Delta);
}

void NativeStringBuilder::AppendWideString(const wchar* ToAppend)
{
    AppendWideString(ToAppend, SNativeString::StringLength(ToAppend));
}

void NativeStringBuilder::AppendWideString(const wchar* ToAppend, const size Length)
{
    AdjustBufferForAppendingStringOfLength(Length);

    SMemory::Copy(Buffer + NullTerminationCharacterIndex, ToAppend, Length + 1);
    NullTerminationCharacterIndex += Length;
}

wchar* NativeStringBuilder::WideCharArrayFromUnsignedInteger(wchar* ReverseBuffer, const StringFormatters::Integer& IntegerFormatter)
{
    uint64 UnsignedInput = IntegerFormatter.Input;
    const uint8 Base = static_cast<const uint8>(IntegerFormatter.Base);

    if (!UnsignedInput)
    {
        ReverseBuffer[0] = TEXT('0');
        return ReverseBuffer;
    }

    for (; UnsignedInput != 0; UnsignedInput /= Base, --ReverseBuffer)
    {
        *ReverseBuffer = TEXT("0123456789ABCDEF")[UnsignedInput % Base];
    }
    if (IntegerFormatter.IsNegative) *(ReverseBuffer--) = TEXT('-');
    return ReverseBuffer + 1;
}

wchar* NativeStringBuilder::WideCharArrayFromPureDecimal(wchar* Buffer, const StringFormatters::Decimal& DecimalFormatter)
{
    double PureDecimalInput = DecimalFormatter.Input >= 0 ? DecimalFormatter.Input : -DecimalFormatter.Input;
    PureDecimalInput -= static_cast<uint64>(PureDecimalInput);
    if (PureDecimalInput == 0)
    {
        *Buffer = TEXT('0');
        return Buffer;
    }

    wchar* ItMax = Buffer + DecimalFormatter.MaxDigitsAfterDecimal;

    for (; Buffer < ItMax && PureDecimalInput != 0; ++Buffer)
    {
        PureDecimalInput *= 10;
        const uint8 Digit = PureDecimalInput;
        PureDecimalInput -= Digit;
        *Buffer = TEXT("0123456789")[Digit];
    }

    return Buffer - 1;
}

SNativeString NativeStringBuilder::ToNativeString() const
{
    return SNativeString(Buffer, NullTerminationCharacterIndex);
}

NativeStringBuilder& NativeStringBuilder::Create(const size InBufferSize)
{
    return *new NativeStringBuilder(InBufferSize);
}

#define IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(x) NativeStringBuilder& operator<<(NativeStringBuilder& OutBuilder, x Other)

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const SNativeString&)
{
    OutBuilder.AppendWideString(Other.GetRaw(), Other.GetLength());
    return OutBuilder;
}

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const wchar*)
{
    OutBuilder.AppendWideString(Other, SNativeString::StringLength(Other));
    return OutBuilder;
}

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const SManagedString&)
{
    const wchar* OtherString = Other.GetRaw();
    OutBuilder.AppendWideString(OtherString, SNativeString::StringLength(OtherString));
    return OutBuilder;
}

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const StringFormatters::Integer&)
{
    const uint8 MaxInt64DigitCount = Other.MaxPossibleDigitCount;
    wchar* Buffer = new wchar[MaxInt64DigitCount + 2]; // +1 for null-termination, +1 for sign
    Buffer[MaxInt64DigitCount + 1] = TEXT('\0'); // last digit
    wchar* Result = NativeStringBuilder::WideCharArrayFromUnsignedInteger(Buffer + MaxInt64DigitCount, Other);
    OutBuilder.AppendWideString(Result, Buffer + MaxInt64DigitCount + 1 - Result);
    delete[] Buffer;
    return OutBuilder;
}

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const StringFormatters::Decimal&)
{
    constexpr uint8 MaxDoubleIntegerDigitCount = 20; // 20 digits max for an int64
    const uint8 DefaultDecimalDigitStringConversionCount = Other.MaxDigitsAfterDecimal;

    wchar* Buffer = new wchar[MaxDoubleIntegerDigitCount + DefaultDecimalDigitStringConversionCount + 3];
    // +1 for null-termination, +1 for sign, +1 for decimal point.

    Buffer[MaxDoubleIntegerDigitCount + 1] = TEXT('.'); // separate out the sections

    wchar* Result = NativeStringBuilder::WideCharArrayFromUnsignedInteger(
        Buffer + MaxDoubleIntegerDigitCount,
        StringFormatters::Integer(static_cast<int64>(Other.Input), StringFormatters::EIntegerBaseType::HexaDecimal));

    wchar* LastCharacter = NativeStringBuilder::WideCharArrayFromPureDecimal(
        Buffer + MaxDoubleIntegerDigitCount + 2,
        Other);

    *(LastCharacter) = TEXT('\0');

    OutBuilder.AppendWideString(Result, LastCharacter + 1 - Result);
    delete[] Buffer;
    return OutBuilder;
}

#define NATIVE_STRING_BUILDER_STREAM_INTEGER_OVERLOAD(x) IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const x) { return OutBuilder << StringFormatters::Integer(Other); }
FOR_EACH(NATIVE_STRING_BUILDER_STREAM_INTEGER_OVERLOAD, uint8, int8, uint16, int16, uint32, int32, uint64, int64)
#undef NATIVE_STRING_BUILDER_STREAM_INTEGER_OVERLOAD

#define NATIVE_STRING_BUILDER_STREAM_DECIMAL_OVERLOAD(x) IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const x) { return OutBuilder << StringFormatters::Decimal(Other); }
FOR_EACH(NATIVE_STRING_BUILDER_STREAM_DECIMAL_OVERLOAD, float, double)
#undef NATIVE_STRING_BUILDER_STREAM_DECIMAL_OVERLOAD

IMPLEMENT_NATIVE_STRING_BUILDER_STREAM(const wchar)
{
    wchar Buffer[2] = TEXT("0");
    Buffer[0] = Other;
    OutBuilder.AppendWideString(Buffer);
    return OutBuilder;
}

#undef IMPLEMENT_NATIVE_STRING_BUILDER_STREAM

SNativeString operator<<(NativeStringBuilder& OutBuilder, NativeStringBuilder::Disposer Disposer)
{
    SNativeString Output = OutBuilder.ToNativeString();
    OutBuilder.Buffer = nullptr;
    delete &OutBuilder;
    return Output;
}

void operator>>(NativeStringBuilder& OutBuilder, SNativeString& Target)
{
    Target = (OutBuilder << NativeStringBuilder::Disposer());
}
