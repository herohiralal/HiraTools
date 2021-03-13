#include "UnityHook.h"
#include "NativeObjectRegistry.h"

UnityHook* UnityHook::Instance = nullptr;

EXPORT_CONSTRUCTOR(UnityHook, CreateUnityHook, const SUnityHookInitParams&, InitParams)
{
    STACK_WALKER::Initialize();
    UNITY_EDITOR_LOG(Log, L"UnityHook created")
    Instance = this;

    Registry = new NativeObjectRegistry(InitParams.NativeObjectRegistryInitReserveSize);
}

EXPORT_DESTRUCTOR(UnityHook)
{
    Instance = nullptr;
    UNITY_EDITOR_LOG(Log, L"UnityHook destroyed")
}

EXPORT_FUNCTION(void, UnityHook, Dispose)
{
    Registry->Dispose();
    delete Registry;
    Registry = nullptr;
    SStackWalker::Shutdown();
}

EXPORT_FUNCTION(void, UnityHook, Update, const float, InUnscaledDeltaTime, const float, InDeltaTime)
{
    UnscaledDeltaTime = InUnscaledDeltaTime;
    DeltaTime = InDeltaTime;

    Registry->Update(InUnscaledDeltaTime, InDeltaTime);
}

EXPORT_FUNCTION(void, UnityHook, FixedUpdate, const float, InFixedUnscaledDeltaTime, const float, InFixedDeltaTime)
{
    FixedUnscaledDeltaTime = InFixedUnscaledDeltaTime;
    FixedDeltaTime = InFixedDeltaTime;

    Registry->FixedUpdate(InFixedUnscaledDeltaTime, InFixedDeltaTime);
}

EXPORT_FUNCTION(void, UnityHook, LateUpdate, const float, InUnscaledDeltaTime, const float, InDeltaTime)
{
    UnscaledDeltaTime = InUnscaledDeltaTime;
    DeltaTime = InDeltaTime;

    Registry->LateUpdate(InUnscaledDeltaTime, InDeltaTime);
}