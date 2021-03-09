#pragma once

#include "Core.h"

#define EXPORT_NATIVEOBJECT_CONSTRUCTOR(typeName, functionName, ...) \
    DLLEXPORT(typeName*) functionName DECLARE_ARGUMENTS(__VA_ARGS__) \
    { \
        typeName* NewObject = new typeName PASS_ARGUMENTS(__VA_ARGS__); \
        NewObject->Initialize(); \
        return NewObject; \
    } \
    typeName::typeName DECLARE_ARGUMENTS(__VA_ARGS__)

#define EXPORT_NATIVEOBJECT_DESTRUCTOR(typeName) \
    DLLEXPORT(void) Destroy##typeName(typeName* Target) \
    { \
        Target->Destroy(); \
    } \
    typeName::~typeName()