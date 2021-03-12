#pragma once

#include "Platform/Platform.h"

class NativeStringBuilder;

/**
 * ALWAYS null-terminated immutable string.
 */
struct SNativeString
{
    friend NativeStringBuilder;
private:
    /** The actual string data. */
    const wchar* Data;

    /** The length of the string. */
    size Length;
public:
    /**
     * Default constructor, allocates a single-character zero-length null-terminated string.
     */
    SNativeString();

    /**
     * Destructor, deallocates the stored string.
     */
    ~SNativeString();

    /**
     * Copy constructor, allocates a new copy of the other string.
     * @param Other The other SNativeString.
     */
    SNativeString(const SNativeString& Other);

    /**
     * Copy assignment operator, deallocates previous string, and allocates a new copy of the other string.
     * @param Other The other SNativeString.
     * @returns The modified SNativeString.
     */
    SNativeString& operator=(const SNativeString& Other);

    /**
     * Move constructor, references the other string, and then sets the other string to a single-character
     * zero-length null-terminated string.
     * @param Other The other SNativeString.
     */
    SNativeString(SNativeString&& Other) noexcept;

    /**
     * Move assignment operator, references the other string, and then sets the other string to a
     * single-character zero-length null-terminated string.
     * @param Other The other SNativeString.
     * @returns The modified SNativeString.
     */
    SNativeString& operator=(SNativeString&& Other) noexcept;

    /**
     * A static instance of an empty string.
     */
    static const SNativeString Empty;

    /**
     * Constructor for a WideString source.
     * @param Source Source data.
     */
    SNativeString(const wchar* Source);

    /**
     * Assignment operator for a WideString source.
     * @param Source Source data.
     * @returns The modified SNativeString.
     */
    SNativeString& operator=(const wchar* Source);

private:
    /**
     * Directly create a NativeString from pre-assigned buffer/length.
     * @param InData Data
     * @param InLength Length
     */
    SNativeString(const wchar* InData, size InLength);

public:
    /**
     * Get the raw const-ptr to the WideChar array.
     * @returns WideChar array containing actual string data.
     */
    const wchar* GetRaw() const;

    /**
     * Get the length of the string.
     * @returns The index for the null-termination character.
     */
    size GetLength() const;

public:
    /**
     * Copy a WideChar array into another. DOES NOT check for buffer boundaries.
     * @param Destination Destination buffer.
     * @param Source Source to copy from.
     * @returns The index for the null-termination character.
     */
    static size StringCopyUnsafe(wchar* Destination, const wchar* Source);

    /**
     * Copy a WideChar array into another. DOES NOT check for buffer boundaries.
     * @param Destination Destination buffer.
     * @param Source Source to copy from.
     * @param SourceLength Length of the input string (not counting the null-termination character).
     */
    static void StringCopyUnsafe(wchar* Destination, const wchar* Source, size SourceLength);

    /**
     * Copy a WideChar array into another. DOES NOT check for buffer boundaries.
     * @param Destination Destination buffer.
     * @param BufferSize The size of the destination buffer.
     * @param Source Source to copy from.
     * @returns Whether the buffer was big enough to perform the task.
     */
    static bool8 StringCopy(wchar* Destination, size BufferSize, const wchar* Source);

    /**
     * Copy a WideChar array into another. DOES NOT check for buffer boundaries.
     * @param Destination Destination buffer.
     * @param BufferSize The size of the destination buffer.
     * @param Source Source to copy from.
     * @param SourceLength Length of the input string (not counting the null-termination character).
     * @returns Whether the buffer was big enough to perform the task.
     */
    static bool8 StringCopy(wchar* Destination, size BufferSize, const wchar* Source, size SourceLength);

    /**
     * Get the length of a string.
     * @param InString Source.
     * @returns The index of the null termination character.
     */
    static size StringLength(const wchar* InString);

    /**
     * Get a NativeString from an integer.
     * @param Input Input integer value.
     * @param Base The base to use for the input.
     * @returns NativeString representing the integer.
     */
    static SNativeString FromInteger(int64 Input, uint8 Base = 10);

    /**
     * Get a NativeString from a 64-bit unsigned integer.
     * @param Input Input integer value.
     * @param Base The base to use for the input.
     * @returns NativeString representing the integer.
     */
    static SNativeString FromUnsignedInteger(uint64 Input, uint8 Base = 10);

    /**
     * Get a NativeString from a decimal.
     * @param Input Input decimal value.
     * @param MaxDigitsAfterDecimal The number of digits after the decimal point.
     * @returns NativeString representing the integer.
     */
    static SNativeString FromDecimal(double Input, uint8 MaxDigitsAfterDecimal = 5);

    /**
     * Concatenate two NativeStrings.
     * @param Other The other NativeString.
     * @returns A new NativeString.
     */
    SNativeString operator+(const SNativeString& Other) const;

    /**
     * Append a NativeString to the current one.
     * @param Other The NativeString to append.
     * @returns The current NativeString.
     */
    SNativeString& operator+=(const SNativeString& Other);

    /**
     * Implicitly convert NativeString to WideChar array.
     */
    constexpr operator const wchar*() const
    {
        return Data;
    }
};

static_assert(static_cast<uint8>(26.632f) == 26, "Integer-cast from floating-point value doesn't merely drop the decimal.");