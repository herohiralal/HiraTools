#pragma once

#include "InteropCommonMacros.h"

#define IMPLEMENT_EXPORTED_CONSTRUCTOR(typeName, functionName, ...) \
    DLLEXPORT(typeName*) functionName DECLARE_ARGUMENTS(__VA_ARGS__) \
    { \
        return new typeName PASS_ARGUMENTS(__VA_ARGS__); \
    } \
    typeName::typeName DECLARE_ARGUMENTS(__VA_ARGS__)

#define IMPLEMENT_EXPORTED_DESTRUCTOR(typeName) \
    DLLEXPORT(void) Destroy##typeName(typeName* Target) \
    { \
        delete Target; \
    } \
    typeName::~typeName()

#define IMPLEMENT_EXPORTED_FUNCTION(returnType, typeName, functionName, ...) \
    DLLEXPORT(returnType) typeName##functionName DECLARE_ARGUMENTS(typeName*, Target, __VA_ARGS__) \
    { \
        return Target->functionName PASS_ARGUMENTS(__VA_ARGS__); \
    } \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__)

#define IMPLEMENT_GETTER_DEFAULT(type, typeName, varName) \
    DLLEXPORT(type) Get##typeName##varName(typeName* Target) \
    { \
        return Target->Get##varName(); \
    } \

#define IMPLEMENT_GETTER(type, typeName, varName) \
    IMPLEMENT_GETTER_DEFAULT(type, typeName, varName) \
    type typeName::Get##varName()

#define IMPLEMENT_SETTER_DEFAULT(type, typeName, varName) \
    DLLEXPORT(void) Set##typeName##varName(typeName* Target, type InValue) \
    { \
        Target->Set##varName(InValue); \
    }

#define IMPLEMENT_SETTER(type, typeName, varName) \
    IMPLEMENT_SETTER_DEFAULT(type, typeName, varName) \
    void typeName::Set##varName(type InValue)