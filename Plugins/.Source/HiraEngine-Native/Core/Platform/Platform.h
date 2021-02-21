#pragma once

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
