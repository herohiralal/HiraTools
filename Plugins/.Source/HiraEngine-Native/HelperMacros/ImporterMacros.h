#pragma once

#include "InteropCommonMacros.h"

#define IMPORT_FUNCTION(type, name, ...) \
    inline type (__stdcall*__Internal_##name) PASS_TYPES(__VA_ARGS__); \
    inline bool name##IsValid = false; \
    DLLEXPORT_INLINE(void) Init##name( type (__stdcall*InDelegate) PASS_TYPES(__VA_ARGS__) ) \
    { \
        if (InDelegate == nullptr) return; \
        __Internal_##name = InDelegate; \
        name##IsValid = true; \
    } \
    extern inline type name DECLARE_ARGUMENTS(__VA_ARGS__) \
    { \
        return __Internal_##name PASS_ARGUMENTS(__VA_ARGS__); \
    }

#define DECLARE_IMPORTED_LIBRARY_FUNCTION(returnType, name, ...) \
    public: \
        static returnType (__stdcall*__Internal##name) PASS_TYPES(__VA_ARGS__); \
        static bool name##IsValid(); \
        static returnType name DECLARE_ARGUMENTS(__VA_ARGS__);

#define IMPLEMENT_IMPORTED_LIBRARY_FUNCTION(returnType, typeName, functionName, ...) \
    returnType (__stdcall*typeName::__Internal##functionName) PASS_TYPES(__VA_ARGS__) = nullptr; \
    bool typeName::functionName##IsValid() { return __Internal##functionName != nullptr; } \
    DLLEXPORT(void) Init##typeName##functionName( returnType (__stdcall*InDelegate) PASS_TYPES(__VA_ARGS__) ) \
    { typeName::__Internal##functionName = InDelegate; } \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__) \
    { return __Internal##functionName PASS_ARGUMENTS(__VA_ARGS__); }