#include "NativeString.h"

#include "NativeStringBuilder.h"
#include "Memory/Memory.h"

SNativeString const SNativeString::Empty = SNativeString();

wchar* GetEmptyWideString()
{
    return new wchar[1]{TEXT('\0')};
}

SNativeString::SNativeString() : Data(GetEmptyWideString()), Length(0) {}

SNativeString::~SNativeString()
{
    delete[] Data;
}

SNativeString::SNativeString(const SNativeString& Other) : Length(Other.Length)
{
    wchar* Buffer = new wchar[Length + 1];
    StringCopyUnsafe(Buffer, Other.Data, Length);
    Data = Buffer;
}

SNativeString& SNativeString::operator=(const SNativeString& Other)
{
    if (this != &Other)
    {
        delete[] Data;

        Length = Other.Length;

        wchar* Buffer = new wchar[Length + 1];
        StringCopyUnsafe(Buffer, Other.Data, Length);
        Data = Buffer;
    }

    return *this;
}

SNativeString::SNativeString(SNativeString&& Other) noexcept : Data(Other.Data), Length(Other.Length)
{
    Other.Data = new wchar[1]{TEXT('\0')};
    Other.Length = 0;
}

SNativeString& SNativeString::operator=(SNativeString&& Other) noexcept
{
    if (this != &Other)
    {
        delete[] Data;

        Data = Other.Data;
        Length = Other.Length;

        Other.Data = new wchar[1]{TEXT('\0')};
        Other.Length = 0;
    }
    return *this;
}

SNativeString::SNativeString(const wchar* Source) : Length(StringLength(Source))
{
    wchar* Buffer = new wchar[Length + 1];
    StringCopyUnsafe(Buffer, Source, Length);
    Data = Buffer;
}

SNativeString& SNativeString::operator=(const wchar* Source)
{
    delete[] Data;

    Length = StringLength(Source);

    wchar* Buffer = new wchar[Length + 1];
    StringCopyUnsafe(Buffer, Source, Length);
    Data = Buffer;

    return *this;
}

SNativeString::SNativeString(const wchar* InData, const size InLength) : Data(InData), Length(InLength) {}

const wchar* SNativeString::GetRaw() const
{
    return Data;
}

size SNativeString::GetLength() const
{
    return Length;
}

size SNativeString::StringCopyUnsafe(wchar* Destination, const wchar* Source)
{
    const size Length = StringLength(Source);
    StringCopyUnsafe(Destination, Source, Length);
    return Length;
}

void SNativeString::StringCopyUnsafe(wchar* Destination, const wchar* Source, const size SourceLength)
{
    SMemory::Copy<wchar>(Destination, Source, SourceLength + 1);
}

bool8 SNativeString::StringCopy(wchar* Destination, const size BufferSize, const wchar* Source)
{
    return StringCopy(Destination, BufferSize, Source, StringLength(Source));
}

bool8 SNativeString::StringCopy(wchar* Destination, const size BufferSize, const wchar* Source, const size SourceLength)
{
    if (BufferSize < SourceLength + 1)
        return false;

    StringCopyUnsafe(Destination, Source, SourceLength);
    return true;
}

size SNativeString::StringLength(const wchar* InString)
{
    size It = 0;
    while (InString[It] != TEXT('\0')) ++It;
    return It;
}

SNativeString SNativeString::FromInteger(const int64 Input, const uint8 Base)
{
    return NativeStringBuilder::Create(20) << StringFormatters::Integer(Input, static_cast<StringFormatters::EIntegerBaseType>(Base)) << NativeStringBuilder::Disposer();
}

SNativeString SNativeString::FromUnsignedInteger(uint64 Input, uint8 Base)
{
    return NativeStringBuilder::Create(20) << StringFormatters::Integer(Input, static_cast<StringFormatters::EIntegerBaseType>(Base)) << NativeStringBuilder::Disposer();
}

SNativeString SNativeString::FromDecimal(const double Input, const uint8 MaxDigitsAfterDecimal)
{
    return NativeStringBuilder::Create(25) << StringFormatters::Decimal{Input, MaxDigitsAfterDecimal} << NativeStringBuilder::Disposer();
}

SNativeString SNativeString::operator+(const SNativeString& Other) const
{
    wchar* Buffer = new wchar[Length + Other.Length + 1];
    SMemory::Copy<wchar>(Buffer, Data, Length);
    SMemory::Copy<wchar>(Buffer + Length, Other.Data, Other.Length + 1);
    return SNativeString(Buffer, Length + Other.Length);
}

SNativeString& SNativeString::operator+=(const SNativeString& Other)
{
    *this = *this + Other;
    return *this;
}
