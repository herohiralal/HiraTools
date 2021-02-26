#include "CallStack.h"
#include "Debug.h"

static uint16 GAllocatedSizeInBytes = 0;
static uint16 GUsedSizeInBytes = 0;
static uint16* GData = nullptr;
static uint16* GDataMax = nullptr;
static uint16* GPreviousLineNumber = nullptr;

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
    GUsedSizeInBytes = 2;
    GData = new uint16[GAllocatedSizeInBytes / sizeof(uint16)];
    *GData = 0;
    GDataMax = GData + (GAllocatedSizeInBytes / sizeof(uint16));
    GPreviousLineNumber = GDataMax - 1;

    return GData;
}

DLLEXPORT(void) CallStackShutdown()
{
    *GData = 0;
    delete[] GData;
    GData = nullptr;
    GDataMax = nullptr;
    GPreviousLineNumber = nullptr;
    GAllocatedSizeInBytes = 0;
    GUsedSizeInBytes = 0;
}

uint16* CallStack::Push(const wchar* FunctionName, const wchar* FileName)
{
    // get index to use
    uint16* const Index = GData + GUsedSizeInBytes / 2;

    // function name
    uint16 FunctionNameSize;
    uint16* const Function = Index + 3;

    for (FunctionNameSize = 0; FunctionName[FunctionNameSize] != 0; ++FunctionNameSize)
    {
        Function[FunctionNameSize] = FunctionName[FunctionNameSize];
    }
    Function[FunctionNameSize++] = 0;
    Index[0] = FunctionNameSize; // function name size meta-data

    // file name
    uint16 FileNameSize;
    uint16* File = Function + FunctionNameSize;

    for (FileNameSize = 0; FileName[FileNameSize] != 0; ++FileNameSize)
    {
        File[FileNameSize] = FileName[FileNameSize];
    }
    File[FileNameSize++] = 0;
    Index[1] = FileNameSize; // file name size meta-data

    // update stack size
    *GData = *GData + 1;

    GUsedSizeInBytes += 0
        + sizeof(uint16) // function-name size
        + sizeof(uint16) // file-name size
        + sizeof(uint16) // line number
        + (FunctionNameSize * sizeof(uint16)) // function-name
        + (FileNameSize * sizeof(uint16)); // file-name

    return Index;
}

void CallStack::Pop(const uint16* Target)
{
    // update stack size; we're not particularly concerned with zero-ing the freed data because it will get reused
    *GData = *GData - 1;

    GUsedSizeInBytes = (Target - GData) * sizeof(uint16);
}

CallStackElement::CallStackElement(const wchar* FunctionName, const wchar* FileName)
    : PreviousLineNumber(GPreviousLineNumber),
      Element(CallStack::Push(FunctionName, FileName))
{
    GPreviousLineNumber = Element + 2;
}

CallStackElement::CallStackElement(const wchar* FunctionName, const wchar* FileName, const uint16 PreviousLineNumber)
    : PreviousLineNumber(GPreviousLineNumber),
      Element(CallStack::Push(FunctionName, FileName))
{
    *GPreviousLineNumber = PreviousLineNumber;
    GPreviousLineNumber = Element + 2;
}

CallStackElement::~CallStackElement()
{
    CallStack::Pop(Element);
    GPreviousLineNumber = PreviousLineNumber;
}

void (CALLING_CONVENTION*GCheckCallStack)() = nullptr;

#define CHECK_CALL_STACK { if (GCheckCallStack != nullptr) GCheckCallStack(); }

DLLEXPORT(void) InitCheckCallStack(void (CALLING_CONVENTION*InDelegate)())
{
    GCheckCallStack = InDelegate;
}

void TestCallStack2(STACK_DATA)
{
    CHECK_CALL_STACK
    UPDATE_STACK_TRACE
    CHECK_CALL_STACK
}

void TestCallStack1(STACK_DATA)
{
    CHECK_CALL_STACK
    UPDATE_STACK_TRACE
    TestCallStack2(STACK_INFO);
    CHECK_CALL_STACK
}

DLLEXPORT(void) TestCallStack()
{
    CHECK_CALL_STACK
    START_STACK_TRACE
    TestCallStack1(STACK_INFO);
    CHECK_CALL_STACK
}