#pragma once

#include "InteropCommonMacros.h"

// base

#define GETTER_BASE(type, name) type Get##name()

#define SETTER_BASE(type, name) void Set##name(type InValue)

// ref

#define GETTER_REF(type, name) type& Get##name() { return name; }

#define SETTER_REF(type, name) void Set##name(type& InValue) { name = InValue; }

// ref const

#define GETTER_REF_CONST(type, name) const type& Get##name() const { return name; }

// default

#define GETTER_DEFAULT_IMPL(type, name) const { return name; }

#define SETTER_DEFAULT_IMPL(type, name) { name = InValue; }

// none

#define ACCESSOR_NONE(what, type, name)

// standard

#define ACCESSOR_STD(what, type, name) what##_BASE(type, name) what##_DEFAULT_IMPL(type, name)

// ref

#define ACCESSOR_STD_REF(what, type, name) what##_REF(type, name)

// ref const

#define ACCESSOR_STD_REF_CONST(what, type, name) what##_REF_CONST(type, name)

// virtual

#define ACCESSOR_STD_VIRTUAL(what, type, name) virtual ACCESSOR_STD(what, type, name)

// custom

#define ACCESSOR_CUSTOM(what, type, name) what##_BASE(type, name);

// custom const

#define ACCESSOR_CUSTOM_CONST(what, type, name) what##_BASE(type, name) const;

// custom virtual

#define ACCESSOR_CUSTOM_VIRTUAL(what, type, name) virtual ACCESSOR_CUSTOM(what, type, name)

// custom virtual const

#define ACCESSOR_CUSTOM_VIRTUAL_CONST(what, type, name) virtual ACCESSOR_CUSTOM_CONST(what, type, name)

// override

#define ACCESSOR_OVERRIDE(what, type, name) virtual what##_BASE(type, name) override;

// const override

#define ACCESSOR_CONST_OVERRIDE(what, type, name) virtual what##_BASE(type, name) const override;

// main

#define PROPERTY(type, name, get, set) \
    private: type name; \
    public: \
        ACCESSOR_##get(GETTER, type, name) \
        ACCESSOR_##set(SETTER, type, name)

#define BITFIELD(intType, name, get, set) \
    private: intType name:1; \
    public: \
        ACCESSOR_##get(GETTER, bool8, name) \
        ACCESSOR_##set(SETTER, bool8, name)

#define DECLARE_SINGLETON( typeName) \
    private: \
        static typeName* Instance; \
    public: \
        typeName(const typeName&) = delete; \
        typeName& operator=(const typeName&) = delete; \
        static typeName& Get() { return *Instance; }

#define WRITE_ENUM_NAME(x) x, 
#define WRITE_ENUM_TEXT(x) #x, 

#define DECLARE_ENUM(type, name, ...) \
    enum class E##name : type \
    { \
        FOR_EACH(WRITE_ENUM_NAME, __VA_ARGS__) \
        name##Max \
    }; \
    static const char* name##Text[] = \
    { \
        FOR_EACH(WRITE_ENUM_TEXT, __VA_ARGS__) \
        "Invalid" \
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