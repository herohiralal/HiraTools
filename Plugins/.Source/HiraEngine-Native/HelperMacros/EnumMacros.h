#pragma once

#include "BasicMacros.h"

#define WRITE_ENUM_NAME(x) x, 
#define WRITE_ENUM_TEXT(x) L#x, 

#define DECLARE_ENUM(type, name, ...) \
    enum class E##name : type \
    { \
        FOR_EACH(WRITE_ENUM_NAME, __VA_ARGS__) \
        name##Max \
    }; \
    static const wchar* name##Text[] = \
    { \
        FOR_EACH(WRITE_ENUM_TEXT, __VA_ARGS__) \
        L"Invalid" \
    };

#define DECLARE_FLAGS(type, name, ...) \
    enum class E##name : type \
    { \
        FOR_EACH(WRITE_ENUM_NAME, __VA_ARGS__) \
    }; \
    constexpr E##name operator~(const E##name A) { return static_cast<E##name>(~static_cast<type>(A)); } \
    constexpr E##name operator|(const E##name A, const E##name B) { return static_cast<E##name>(static_cast<type>(A) | static_cast<type>(B)); } \
    constexpr E##name operator&(const E##name A, const E##name B) { return static_cast<E##name>(static_cast<type>(A) & static_cast<type>(B)); } \
    constexpr E##name operator^(const E##name A, const E##name B) { return static_cast<E##name>(static_cast<type>(A) ^ static_cast<type>(B)); } \
    constexpr E##name& operator|=(E##name& A, const E##name B) { A = A | B; return A; } \
    constexpr E##name& operator&=(E##name& A, const E##name B) { A = A & B; return A; } \
    constexpr E##name& operator^=(E##name& A, const E##name B) { A = A ^ B; return A; } \
    constexpr bool8 HasFlag(const E##name A, const E##name B) { return ((A & B) == B); } \
    constexpr void AddFlag(E##name& A, const E##name B) { A |= B; } \
    constexpr void RemoveFlag(E##name& A, const E##name B) { A &= ~B; }