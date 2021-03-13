#include "StackWalker.h"

SStackWalker* SStackWalker::Instance = nullptr;

void SStackWalker::Initialize()
{
    Instance = new SStackWalker();
}

void SStackWalker::Shutdown()
{
    Get().ShutdownImplementation();
}

void SStackWalker::AppendCallStack(NativeStringBuilder& OutBuilder, const uint8 FramesToSkip)
{
    Get().AppendCallStackImplementation(OutBuilder, FramesToSkip + 1);
}

SStackWalker& SStackWalker::Get()
{
    return *Instance;
}

void SStackWalker::ShutdownImplementation()
{
    delete Instance;
    Instance = nullptr;
}

void SStackWalker::AppendCallStackImplementation(NativeStringBuilder& OutBuilder, const uint8 FramesToSkip)
{
    
}
