#include "UnityHook.h"
#include "ExporterMacros.h"
#include "Debug.h"
#include "HiraObjectRegistry.h"

UnityHook* UnityHook::Instance = nullptr;

IMPLEMENT_EXPORTED_CONSTRUCTOR(UnityHook, CreateUnityHook, const SUnityHookInitParams&, InitParams)
{
    UNITY_EDITOR_LOG(Log, "UnityHook created")
    Instance = this;

    Registry = new HiraObjectRegistry(InitParams.HiraObjectRegistryInitReserveSize);
}

IMPLEMENT_EXPORTED_DESTRUCTOR(UnityHook)
{
    Instance = nullptr;
    UNITY_EDITOR_LOG(Log, "UnityHook destroyed")
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, Dispose)
{
    Registry->Dispose();
    delete Registry;
    Registry = nullptr;
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, Update, const float, InUnscaledDeltaTime, const float, InDeltaTime)
{
    UnscaledDeltaTime = InUnscaledDeltaTime;
    DeltaTime = InDeltaTime;

    Registry->Update(InUnscaledDeltaTime, InDeltaTime);
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, FixedUpdate, const float, InFixedUnscaledDeltaTime, const float, InFixedDeltaTime)
{
    FixedUnscaledDeltaTime = InFixedUnscaledDeltaTime;
    FixedDeltaTime = InFixedDeltaTime;

    Registry->FixedUpdate(InFixedUnscaledDeltaTime, InFixedDeltaTime);
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, LateUpdate, const float, InUnscaledDeltaTime, const float, InDeltaTime)
{
    UnscaledDeltaTime = InUnscaledDeltaTime;
    DeltaTime = InDeltaTime;

    Registry->LateUpdate(InUnscaledDeltaTime, InDeltaTime);
}