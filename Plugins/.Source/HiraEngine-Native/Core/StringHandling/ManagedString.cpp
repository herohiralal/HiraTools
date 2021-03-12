#include "ManagedString.h"

SManagedString::SManagedString() : Data(nullptr)
{
}

SManagedString::~SManagedString()
{
}

SNativeString SManagedString::ToNativeString() const
{
    return SNativeString(Data);
}

const wchar* SManagedString::GetRaw() const
{
    return Data;
}
