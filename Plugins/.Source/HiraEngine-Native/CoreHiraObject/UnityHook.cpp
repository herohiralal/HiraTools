#include "UnityHook.h"
#include "ExporterMacros.h"
#include "ImporterMacros.h"
#include "Debug.h"
#include "HiraObject.h"

UnityHook* UnityHook::Instance = nullptr;

IMPLEMENT_EXPORTED_CONSTRUCTOR(UnityHook, CreateUnityHook, const SUnityHookInitParams, InitParams)
{
    UNITY_EDITOR_LOG(Log, "UnityHook created")

    HiraObject::HiraObjectRegistry.SetBufferSize(InitParams.HiraObjectRegistryInitReserveSize);
    Instance = this;
}

IMPLEMENT_EXPORTED_DESTRUCTOR(UnityHook)
{
    HiraObject::HiraObjectRegistry.SetBufferSize(0);
    Instance = nullptr;
    
    UNITY_EDITOR_LOG(Log, "UnityHook destroyed")
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, Update, const float, InDeltaTime)
{
    DeltaTime = InDeltaTime;
}