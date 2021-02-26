#include "CallStack.h"
#include "Debug.h"

static uint16 GAllocatedSizeInBytes = 0;
static uint16 GUsedSizeInBytes = 0;
static wchar* GData = nullptr;
static wchar* GDataMax = nullptr;

namespace CallStackHelpers
{
    static constexpr uint16 GFunctionNameSizeHint = 255;
    static constexpr uint16 GFileNameSizeHint = 511;

    constexpr uint16 CalculateSizeToAllocate(const uint16 MaxStackSizeHint)
    {
        constexpr uint16 StackElementSizeHint = 0
            + sizeof(uint16) // Actual function-name size
            + sizeof(uint16) // Actual file-name size
            + sizeof(uint16) // Line number
            + (GFunctionNameSizeHint * sizeof(uint16)) // Function-name (based on hinted size)
            + (GFileNameSizeHint * sizeof(uint16)); // File-name (based on hinted size)

        return sizeof(uint16) // number of elements in stack
            + (StackElementSizeHint * MaxStackSizeHint); // rough number of stack elements
    }
}

DLLEXPORT(void*) CallStackInitialize(const uint16 MaxStackSizeHint)
{
    GAllocatedSizeInBytes = CallStackHelpers::CalculateSizeToAllocate(MaxStackSizeHint);
    GUsedSizeInBytes = 1;
    GData = new wchar[GAllocatedSizeInBytes / sizeof(uint16)];
    *GData = 0;
    GDataMax = GData + (GAllocatedSizeInBytes / sizeof(uint16));

    return GData;
}

DLLEXPORT(void) CallStackShutdown()
{
    *GData = 0;
    delete[] GData;
    GData = nullptr;
    GDataMax = nullptr;
    GAllocatedSizeInBytes = 0;
    GUsedSizeInBytes = 0;
}

void CallStack::Push(const wchar* FunctionName, const wchar* FileName, const uint16 Line)
{
    // get index to use
    wchar* Index = GData + GUsedSizeInBytes;

    // line number
    Index[2] = Line;

    // function name
    uint16 FunctionNameSize;
    wchar* const Function = Index + 3;

    for (FunctionNameSize = 0; FunctionName[FunctionNameSize] != 0; ++FunctionNameSize)
    {
        Function[FunctionNameSize] = FunctionName[FunctionNameSize];
    }
    Function[FunctionNameSize++] = 0;
    Index[0] = FunctionNameSize; // function name size meta-data

    // file name
    uint16 FileNameSize;
    wchar* File = Function + FunctionNameSize;

    for (FileNameSize = 0; FileName[FileNameSize] != 0; ++FileNameSize)
    {
        File[FileNameSize] = FileName[FileNameSize];
    }
    File[FileNameSize++] = 0;
    Index[1] = FileNameSize; // file name size meta-data

    // update stack size
    (*GData)++;

    GUsedSizeInBytes += 0
        + sizeof(uint16) // function-name size
        + sizeof(uint16) // file-name size
        + sizeof(uint16) // line number
        + (FunctionNameSize * sizeof(uint16)) // function-name
        + (FileNameSize * sizeof(uint16)); // file-name
}

void CallStack::Pop()
{
    // update stack size; we're not particularly concerned with zero-ing the freed data because it will get reused
    (*GData)--;

    wchar* Index = GData + GUsedSizeInBytes;

    GUsedSizeInBytes -= 0
        + sizeof(uint16) // function-name size
        + sizeof(uint16) // file-name size
        + sizeof(uint16) // line number
        + (Index[0] * sizeof(uint16)) // function-name
        + (Index[1] * sizeof(uint16)); // file-name
}

void (CALLING_CONVENTION*GCheckCallStack)() = nullptr;

#define CHECK_CALL_STACK { if (GCheckCallStack != nullptr) GCheckCallStack(); }

DLLEXPORT(void) InitCheckCallStack(void (CALLING_CONVENTION*InDelegate)())
{
    GCheckCallStack = InDelegate;
}

void TestCallStack2(const wchar* FileName, const uint16 Line)
{
    CHECK_CALL_STACK
    CallStack::Push(L"TestCallStack2", FileName, Line);
    CHECK_CALL_STACK
    CallStack::Pop();
}

void TestCallStack1(const wchar* FileName, const uint16 Line)
{
    CHECK_CALL_STACK
    CallStack::Push(L"TestCallStack1", FileName, Line);
    TestCallStack2(L"CallStack.cpp", 126);
    CallStack::Pop();
}

DLLEXPORT(void) TestCallStack()
{
    CHECK_CALL_STACK
    CallStack::Push(L"TestCallStack");
    TestCallStack1(L"CallStack.cpp", 134);
    CallStack::Pop();
}
