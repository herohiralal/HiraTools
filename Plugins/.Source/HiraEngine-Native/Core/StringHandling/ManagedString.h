#pragma once

#include "Platform/Platform.h"
#include "NativeString.h"

class NativeStringBuilder;

struct SManagedString
{
    friend NativeStringBuilder;
private:
    const wchar* Data;

public:
    SManagedString();
    ~SManagedString();
    SNativeString ToNativeString() const;
    const wchar* GetRaw() const;
};

static_assert(sizeof(SManagedString) == sizeof(wchar*), "ManagedString size does not match the size of a raw pointer.");