#pragma once

#include "InteropCommonMacros.h"

/**
 * Expose a constructor to be called by Managed API.
 * @param typeName              The enclosing type.
 * @param functionName          Name of the factory-method style function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define EXPORT_CONSTRUCTOR(typeName, functionName, ...) \
    DLLEXPORT(typeName*) functionName DECLARE_ARGUMENTS(__VA_ARGS__) \
    { \
        return new typeName PASS_ARGUMENTS(__VA_ARGS__); \
    } \
    typeName::typeName DECLARE_ARGUMENTS(__VA_ARGS__)

/**
 * Expose a destructor to be called by Managed API.
 * @param typeName              The enclosing type.
 */
#define EXPORT_DESTRUCTOR(typeName) \
    DLLEXPORT(void) Destroy##typeName(typeName* Target) \
    { \
        delete Target; \
    } \
    typeName::~typeName()

/**
 * Expose a function to be called by Managed API.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define EXPORT_FUNCTION(returnType, typeName, functionName, ...) \
    DLLEXPORT(returnType) typeName##functionName DECLARE_ARGUMENTS(typeName*, Target, __VA_ARGS__) \
    { \
        return Target->functionName PASS_ARGUMENTS(__VA_ARGS__); \
    } \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__)

/**
 * Expose a getter-function of a property to be called by Managed API.
 * @param type                  The type of the property.
 * @param typeName              The enclosing type.
 * @param varName               The variable name.
 */
#define EXPORT_GETTER(type, typeName, varName) \
    DLLEXPORT(type) Get##typeName##varName(typeName* Target) \
    { \
        return Target->Get##varName PASS_ARGUMENTS() ; \
    }

/**
 * Expose a setter-function of a property to be called by Managed API.
 * @param type                  The type of the property.
 * @param typeName              The enclosing type.
 * @param varName               The variable name.
 */
#define EXPORT_SETTER(type, typeName, varName) \
    DLLEXPORT(void) Set##typeName##varName(typeName* Target, type InValue) \
    { \
        Target->Set##varName PASS_ARGUMENTS(type, InValue) ; \
    }

/**
 * Expose a custom getter-function of a property to be called by Managed API.
 * @param type                  The type of the property.
 * @param typeName              The enclosing type.
 * @param varName               The variable name.
 */
#define EXPORT_CUSTOM_GETTER(type, typeName, varName) \
    EXPORT_GETTER(type, typeName, varName) \
    type typeName::Get##varName()

/**
 * Expose a custom setter-function of a property to be called by Managed API.
 * @param type                  The type of the property.
 * @param typeName              The enclosing type.
 * @param varName               The variable name.
 */
#define EXPORT_CUSTOM_SETTER(type, typeName, varName) \
    EXPORT_SETTER(type, typeName, varName) \
    void typeName::Set##varName(type InValue)

/**
 * Expose a static function to be called by managed API.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define EXPORT_LIBRARY_FUNCTION(returnType, typeName, functionName, ...) \
    DLLEXPORT(returnType) typeName##functionName DECLARE_ARGUMENTS(__VA_ARGS__) \
    { \
        return typeName::functionName PASS_ARGUMENTS(__VA_ARGS__); \
    } \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__)

/**
 * Import a function with a specific signature.
 * @param returnType            The return type of the function.
 * @param name                  What the function should be called.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define IMPORT_FUNCTION_WITH_SIGNATURE(returnType, name, ...) \
    returnType (CALLING_CONVENTION*name) DECLARE_ARGUMENTS(__VA_ARGS__) = nullptr; \
    DLLEXPORT(void) Init##name(returnType (CALLING_CONVENTION*InDelegate) DECLARE_ARGUMENTS(__VA_ARGS__)) \
    { \
        name = InDelegate; \
    }

/**
 * Allow a function to be overriden by managed code.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define IMPORT_FUNCTION(returnType, typeName, functionName, ...) \
    IMPORT_FUNCTION_WITH_SIGNATURE(returnType, typeName##functionName##ManagedOverride, typeName*, Target, __VA_ARGS__) \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__)


/**
 * Allow a static function to be overriden by managed code.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define IMPORT_LIBRARY_FUNCTION(returnType, typeName, functionName, ...) \
    IMPORT_FUNCTION_WITH_SIGNATURE(returnType, typeName##functionName##ManagedOverride, __VA_ARGS__) \
    returnType typeName::functionName DECLARE_ARGUMENTS(__VA_ARGS__)

/**
 * Call the managed override of a function.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define CALL_MANAGED_OVERRIDE_IF_AVAILABLE(returnType, typeName, functionName, ...) \
    if (typeName##functionName##ManagedOverride != nullptr) \
        return typeName##functionName##ManagedOverride(this, __VA_ARGS__);

/**
 * Call the managed override of a function.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define CALL_MANAGED_LIBRARY_OVERRIDE_IF_AVAILABLE(returnType, typeName, functionName, ...) \
    if (typeName##functionName##ManagedOverride != nullptr) \
        return typeName##functionName##ManagedOverride(__VA_ARGS__);

/**
 * Allow a function to be overriden by managed code and call it.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define IMPORT_FUNCTION_AND_CALL(returnType, typeName, functionName, ...) \
    IMPORT_FUNCTION(returnType, typeName, functionName, __VA_ARGS__) \
    { \
        if (typeName##functionName##ManagedOverride != nullptr) \
            return typeName##functionName##ManagedOverride PASS_ARGUMENTS(typeName*, this, __VA_ARGS__); \
    }


/**
 * Allow a static function to be overriden by managed code and call it.
 * @param returnType            The return type of the function.
 * @param typeName              The enclosing type.
 * @param functionName          The name of the function.
 * @param ...                   The arguments, with types/names separated by commas.
 */
#define IMPORT_LIBRARY_FUNCTION_AND_CALL(returnType, typeName, functionName, ...) \
    IMPORT_LIBRARY_FUNCTION(returnType, typeName, functionName, __VA_ARGS__) \
    { \
        if (typeName##functionName##ManagedOverride != nullptr) \
            return typeName##functionName##ManagedOverride PASS_ARGUMENTS(__VA_ARGS__); \
    }