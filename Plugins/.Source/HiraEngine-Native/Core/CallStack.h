#pragma once

#include "Platform/Platform.h"

class CallStack
{
public:
    static void Push(const wchar* FunctionName, const wchar* FileName = L"", uint16 Line = -1);
    static void Pop();
};
