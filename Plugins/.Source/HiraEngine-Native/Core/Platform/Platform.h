#pragma once

template<typename T32Bits, typename T64Bits, int TPointerSize>
struct SelectIntPointerType
{
    // nothing here are is it an error if the partial specializations fail
};

template<typename T32Bits, typename T64Bits>
struct SelectIntPointerType<T32Bits, T64Bits, 8>
{
    typedef T64Bits IntPointer;
};

template<typename T32Bits, typename T64Bits>
struct SelectIntPointerType<T32Bits, T64Bits, 4>
{
    typedef T32Bits IntPointer;
};

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

typedef SelectIntPointerType<uint32, uint64, sizeof(void*)>::IntPointer uintptr;
typedef SelectIntPointerType<int32, int64, sizeof(void*)>::IntPointer intptr;

static_assert(sizeof(uint8) == 1, "uint8 size test failed.");
static_assert(sizeof(uint16) == 2, "uint16 size test failed.");
static_assert(sizeof(uint32) == 4, "uint32 size test failed.");
static_assert(sizeof(uint64) == 8, "uint64 size test failed.");

static_assert(sizeof(int8) == 1, "int8 size test failed.");
static_assert(sizeof(int16) == 2, "int16 size test failed.");
static_assert(sizeof(int32) == 4, "int32 size test failed.");
static_assert(sizeof(int64) == 8, "int64 size test failed.");

static_assert(static_cast<int32>(static_cast<uint8>(-1)) == 0xFF, "uint8 sign test failed.");
static_assert(static_cast<int32>(static_cast<uint16>(-1)) == 0xFFFF, "uint16 sign test failed.");
static_assert(static_cast<int64>(static_cast<uint32>(-1)) == static_cast<int64>(0xFFFFFFFF), "uint32 sign test failed.");
static_assert(static_cast<uint64>(-1) > static_cast<uint64>(0), "uint64 sign test failed.");

static_assert(static_cast<int32>(static_cast<int8>(-1)) == -1, "int8 sign test failed.");
static_assert(static_cast<int32>(static_cast<int16>(-1)) == -1, "int16 sign test failed.");
static_assert(static_cast<int64>(static_cast<int32>(-1)) == static_cast<int64>(-1), "int32 sign test failed.");
static_assert(static_cast<int64>(-1) < static_cast<int64>(0), "int64 sign test failed.");

static_assert(static_cast<char>(-1) < static_cast<char>(0), "Unsigned char type test failed.");

static_assert(sizeof(AnsiChar) == 1, "ANSI Char size test failed.");
static_assert(static_cast<int32>(static_cast<AnsiChar>(-1)) == -1, "ANSI Char sign test failed.");
static_assert(sizeof(wchar) == 2 || sizeof(wchar) == 4, "Wchar size test failed.");

static_assert(sizeof(intptr) == sizeof(void*), "IntPtr size test failed.");
static_assert(sizeof(uintptr) == sizeof(void*), "UIntPtr size test failed.");
static_assert(static_cast<intptr>(-1) < static_cast<intptr>(0), "Intptr sign test failed.");
static_assert(static_cast<uintptr>(-1) > static_cast<uintptr>(0), "UIntPtr sign test failed.");