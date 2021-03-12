#pragma once

#include "HelperMacros.h"

namespace Platform
{
    namespace PointerSizeHelper
    {
        template <typename T32Bits, typename T64Bits, int TPointerSize>
        struct SelectIntPointerType
        {
            // nothing here are is it an error if the partial specializations fail
        };

        template <typename T32Bits, typename T64Bits>
        struct SelectIntPointerType<T32Bits, T64Bits, 8>
        {
            typedef T64Bits IntPointer;
        };

        template <typename T32Bits, typename T64Bits>
        struct SelectIntPointerType<T32Bits, T64Bits, 4>
        {
            typedef T32Bits IntPointer;
        };
    }
}

typedef unsigned char uint8;
typedef unsigned short int uint16;
typedef unsigned int uint32;
typedef unsigned long long uint64;

typedef signed char int8;
typedef signed short int int16;
typedef signed int int32;
typedef signed long long int64;

typedef char AnsiChar;
typedef wchar_t wchar;
typedef uint8 char8;
typedef uint16 char16;
typedef uint32 char32;

typedef Platform::PointerSizeHelper::SelectIntPointerType<uint32, uint64, sizeof(void*)>::IntPointer uintptr;
typedef Platform::PointerSizeHelper::SelectIntPointerType<int32, int64, sizeof(void*)>::IntPointer intptr;

typedef uintptr size;
typedef intptr ssize;

struct bool8
{
public:
    constexpr bool8() : Data(0)
    {
    }

    constexpr bool8(const uint8 InData) : Data(InData != 0)
    {
    }

    explicit constexpr bool8(const bool InData) : Data(InData)
    {
    }

    explicit constexpr operator bool() const
    {
        return Data;
    }

    operator uint8() const
    {
        return Data;
    }

private:
    uint8 Data;
};

#define SIZE_TEST(name, comparison) static_assert(sizeof(name) == comparison, MAKE_STRING(name size test failed.));

FOR_EACH_2_ARGUMENTS(SIZE_TEST,
                     bool8, 1,
                     uint8, 1,
                     uint16, 2,
                     uint32, 4,
                     uint64, 8,
                     int8, 1,
                     int16, 2,
                     int32, 4,
                     int64, 8,
                     AnsiChar, 1,
                     wchar, 2,
                     intptr, sizeof(void*),
                     uintptr, sizeof(void*))

#undef SIZE_TEST

#define SIGN_TEST(name, expr) static_assert(expr, MAKE_STRING(name sign test failed.));

FOR_EACH_2_ARGUMENTS(SIGN_TEST,
                     bool8, bool8(static_cast<uint8>(1)) && !bool8(static_cast<uint8>(0)),
                     uint8, static_cast<int32>(static_cast<uint8>(-1)) == 0xFF,
                     uint16, static_cast<int32>(static_cast<uint16>(-1)) == 0xFFFF,
                     uint32, static_cast<int64>(static_cast<uint32>(-1)) == static_cast<int64>(0xFFFFFFFF),
                     uint64, static_cast<uint64>(-1) > static_cast<uint64>(0),
                     int8, static_cast<int32>(static_cast<int8>(-1)) == -1,
                     int16, static_cast<int32>(static_cast<int16>(-1)) == -1,
                     int32, static_cast<int64>(static_cast<int32>(-1)) == static_cast<int64>(-1),
                     int64, static_cast<int64>(-1) < static_cast<int64>(0),
                     Unsigned Character, static_cast<char>(-1) < static_cast<char>(0),
                     ANSI Character, static_cast<int32>(static_cast<AnsiChar>(-1)) == -1,
                     IntPtr, static_cast<intptr>(-1) < static_cast<intptr>(0),
                     UIntPtr, static_cast<uintptr>(-1) > static_cast<uintptr>(0))

#undef SIGN_TEST

/**
 * Convert a char/char-array to a WideChar/WideChar-array.
 * @param x Input
 * @returns WideChar variant of string or character literal.
 */
#define TEXT(x) L##x