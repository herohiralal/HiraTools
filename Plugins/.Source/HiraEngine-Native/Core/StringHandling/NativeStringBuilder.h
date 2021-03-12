#pragma once

#include "Platform/Platform.h"
#include "NativeString.h"
#include "ManagedString.h"

namespace StringFormatters
{
    enum class EIntegerBaseType : uint8 { Binary = 2, Octal = 8, Decimal = 10, HexaDecimal = 16 };

    /** Integer formatting settings. Immutable. */
    struct Integer
    {
#define INTEGER_STRING_FORMATTER_CONSTRUCTOR_UNSIGNED(x) Integer(const x InInput, const EIntegerBaseType Base = EIntegerBaseType::Decimal) : Input(InInput), Base(Base), MaxPossibleDigitCount(MaxPossibleDigitCountFromBase(Base)), IsNegative(false) { }
        FOR_EACH(INTEGER_STRING_FORMATTER_CONSTRUCTOR_UNSIGNED, uint8, uint16, uint32, uint64)
#undef INTEGER_STRING_FORMATTER_CONSTRUCTOR_UNSIGNED
#define INTEGER_STRING_FORMATTER_CONSTRUCTOR_SIGNED(x) Integer(const x InInput, const EIntegerBaseType Base = EIntegerBaseType::Decimal) : Input(static_cast<uint64>(InInput >= 0 ? InInput : -InInput)), Base(Base), MaxPossibleDigitCount(MaxPossibleDigitCountFromBase(Base)), IsNegative(false) { }
        FOR_EACH(INTEGER_STRING_FORMATTER_CONSTRUCTOR_SIGNED, int8, int16, int32, int64)
#undef INTEGER_STRING_FORMATTER_CONSTRUCTOR_SIGNED

        /** The integer to format. */
        const uint64 Input;
        /** The base, upto 16 is supported. */
        const EIntegerBaseType Base;
        /** Max possible digit count. Will cause buffer overflows if this value is less than required. */
        const uint8 MaxPossibleDigitCount = 64;
        /** Whether the input is negative. */
        const bool8 IsNegative;
        static uint8 MaxPossibleDigitCountFromBase(EIntegerBaseType Base);
    };

    /** Decimal formatting settings. Immutable. */
    struct Decimal
    {
#define DECIMAL_STRING_FORMATTER_CONSTRUCTOR(x) Decimal(const x Input, const uint8 MaxDigitsAfterDecimal = 5) : Input(Input), MaxDigitsAfterDecimal(MaxDigitsAfterDecimal) { }
        FOR_EACH(DECIMAL_STRING_FORMATTER_CONSTRUCTOR, double, float)
#undef DECIMAL_STRING_FORMATTER_CONSTRUCTOR
        /** The decimal to format. */
        const double Input;
        /** Max number of digits to use after decimal. Default is 5. */
        const uint8 MaxDigitsAfterDecimal = 5;
    };
}

#define DECLARE_NATIVE_STRING_BUILDER_STREAM(x) NativeStringBuilder& operator<<(NativeStringBuilder& OutBuilder, x Other);
#define DECLARE_NATIVE_STRING_BUILDER_FRIEND(x) friend DECLARE_NATIVE_STRING_BUILDER_STREAM(x)

class NativeStringBuilder
{
public:
    /** Token struct to denote the disposal of the NativeStringBuilder. */
    struct Disposer
    {
    };

    FOR_EACH(DECLARE_NATIVE_STRING_BUILDER_FRIEND,
             const wchar*,
             const SNativeString&,
             const SManagedString&,
             const StringFormatters::Integer&,
             const StringFormatters::Decimal&,
             wchar)

    friend SNativeString operator<<(NativeStringBuilder& OutBuilder, Disposer Disposer);

private:
    /** The actual WideString buffer. */
    wchar* Buffer;

    /** The size of the WideString buffer. */
    size BufferSize;

    /** The index of the null-termination character, i.e. - the length of the string that is currently occupied in the buffer. */
    size NullTerminationCharacterIndex;

    /**
     * Create a NativeStringBuilder with a starting buffer size.
     * @param InBufferSize The Starting buffer size.
     */
    explicit NativeStringBuilder(size InBufferSize);

    /** Destructor that disposes the buffer. */
    ~NativeStringBuilder();

    /** Deleted copy-constructor. */
    NativeStringBuilder(const NativeStringBuilder& Other) = delete;

    /** Deleted copy-assignment operator. */
    NativeStringBuilder& operator=(const NativeStringBuilder& Other) = delete;

    /**
     * Adjust the buffer for a new string length.
     * @param NewSize The new size required.
     */
    void AdjustBufferForTotalStringLength(size NewSize);

    /**
     * Adjust the buffer for a new string length.
     * @param Delta The length of the string to append.
     */
    void AdjustBufferForAppendingStringOfLength(size Delta);

    /**
     * Append a WideString. Uses SNativeString::StringLength to calculate length.
     * @param ToAppend The string to append.
     */
    void AppendWideString(const wchar* ToAppend);

    /**
     * Append a WideString of given length.
     * @param ToAppend The string to append.
     * @param Length The length of the string.
     */
    void AppendWideString(const wchar* ToAppend, size Length);

    /**
     * Get an SNativeString structure referencing the current buffer.
     * @returns SNativeString structure.
     */
    SNativeString ToNativeString() const;

    /**
     * Internal implementation of SNativeString::FromInteger, requires sanitized
     * positive-integer-only input and a manual confirmation regarding the negativity
     * of the input.
     * @param ReverseBuffer The address of where the last digit should be.
     * @param IntegerFormatter Formatting settings and input.
     * @returns The address of the first digit.
     */
    static wchar* WideCharArrayFromUnsignedInteger(wchar* ReverseBuffer, const StringFormatters::Integer& IntegerFormatter);

    /**
     * Internal implementation of SNativeString::FromDecimal, requires sanitized
     * pure-decimal-only input.
     * @param Buffer The address where the first digit after the decimal should be.
     * @param DecimalFormatter Formatting settings and input.
     * @returns The address of the last occupied-digit.
     */
    static wchar* WideCharArrayFromPureDecimal(wchar* Buffer, const StringFormatters::Decimal& DecimalFormatter);

public:
    /**
     * Create a NativeStringBuilder and use it like an OutputStream.
     * @param InBufferSize StartingBufferSize of the builder.
     * @returns A reference to a NativeStringBuilder, to fill more data into the stream.
     */
    static NativeStringBuilder& Create(size InBufferSize = 0);
};

FOR_EACH(DECLARE_NATIVE_STRING_BUILDER_STREAM,
         const wchar*,
         const SNativeString&,
         const SManagedString&,
         const StringFormatters::Integer&,
         const StringFormatters::Decimal&,
         uint8, uint16, uint32, uint64,
         int8, int16, int32, int64,
         float, double,
         wchar)

/**
 * Dispose the NativeStringBuilder and return SNativeString structure.
 * @param OutBuilder This NativeStringBuilder.
 * @param Disposer NativeStringBuilder::Disposer structure, create it on the stack with its default constructor.
 * @returns A NativeString.
 */
SNativeString operator<<(NativeStringBuilder& OutBuilder, NativeStringBuilder::Disposer Disposer);

/**
 * Dispose the NativeStringBuilder and fill SNativeStringStructure.
 * @param OutBuilder This NativeStringBuilder.
 * @param Target Output.
 */
void operator>>(NativeStringBuilder& OutBuilder, SNativeString& Target);

#undef DECLARE_NATIVE_STRING_BUILDER_FRIEND
#undef DECLARE_NATIVE_STRING_BUILDER_STREAM
