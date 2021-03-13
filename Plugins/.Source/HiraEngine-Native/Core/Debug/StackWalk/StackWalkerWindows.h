#pragma once

#include "StackWalker.h"
#include "Platform/Platform.h"

struct SStackWalkerWindows final : public SStackWalker
{
    typedef SStackWalker Super;
private:
    static bool8 AttemptInit();
protected:
    virtual void ShutdownImplementation() override;
    virtual void AppendCallStackImplementation(NativeStringBuilder& OutBuilder, uint8 FramesToSkip) override;
public:
    static void Initialize();
};

#if UNITY_EDITOR_WIN ||  (UNITY_STANDALONE_WIN && DEVELOPMENT_BUILD)

#if defined(STACK_WALKER)
#error STACK_WALKER has been incorrectly defined.
#endif

#define STACK_WALKER SStackWalkerWindows

#endif
