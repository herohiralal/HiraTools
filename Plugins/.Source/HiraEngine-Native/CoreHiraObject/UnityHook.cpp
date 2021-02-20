#include "UnityHook.h"
#include "ExporterMacros.h"
#include "ImporterMacros.h"
#include "Debug.h"

UnityHook* UnityHook::Instance = nullptr;

IMPLEMENT_EXPORTED_CONSTRUCTOR(UnityHook, CreateUnityHook)
{
    UNITY_EDITOR_LOG(Log, "UnityHook created")

    Instance = this;
}

IMPLEMENT_EXPORTED_DESTRUCTOR(UnityHook)
{
    Instance = nullptr;
    
    UNITY_EDITOR_LOG(Log, "UnityHook destroyed")
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, Update, const float, InDeltaTime)
{
    DeltaTime = InDeltaTime;
}