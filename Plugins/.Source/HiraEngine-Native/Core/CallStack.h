#pragma once

#include "Platform/Platform.h"

class CallStack final
{
public:
    static uint16* Push(const wchar* FunctionName, const wchar* FileName);
    static void Pop(const uint16* Target);
};

struct CallStackElement final
{
    explicit CallStackElement(const wchar* FunctionName, const wchar* FileName);
    explicit CallStackElement(const wchar* FunctionName, const wchar* FileName, uint16 PreviousLineNumber);
    ~CallStackElement();

private:
    uint16* const PreviousLineNumber = nullptr;
    uint16* const Element = nullptr;
};

#if _EDITOR

#define STACK_DATA uint16 __StackDataPreviousLineNumber
#define STACK_INFO __LINE__

#define COMMA_STACK_DATA , uint16 __StackDataPreviousLineNumber
#define COMMA_STACK_INFO , __LINE__

#define START_STACK_TRACE CallStackElement __CallStackHandle(WFUNC, WFILE);
#define UPDATE_STACK_TRACE CallStackElement __CallStackHandle(WFUNC, WFILE, __StackDataPreviousLineNumber);

#else

#define STACK_DATA
#define STACK_INFO

#define COMMA_STACK_DATA
#define COMMA_STACK_INFO

#define START_STACK_TRACE
#define UPDATE_STACK_TRACE

#endif