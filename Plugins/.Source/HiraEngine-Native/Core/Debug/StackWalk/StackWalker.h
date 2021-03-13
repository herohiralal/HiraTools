#pragma once
#include "PropertyMacros.h"
#include "Platform/Platform.h"

class NativeStringBuilder;

struct SStackWalker
{
protected:
    static SStackWalker* Instance;
    static SStackWalker& Get();
    virtual void ShutdownImplementation();
    virtual void AppendCallStackImplementation(NativeStringBuilder& OutBuilder, uint8 FramesToSkip);

public:
    virtual ~SStackWalker() = default;
    static void Initialize();
    static void Shutdown();
    static void AppendCallStack(NativeStringBuilder& OutBuilder, uint8 FramesToSkip);
};

#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
#if defined(STACK_WALKER)
#error STACK_WALKER has been incorrectly defined.
#endif
#define STACK_WALKER SStackWalker
#endif
