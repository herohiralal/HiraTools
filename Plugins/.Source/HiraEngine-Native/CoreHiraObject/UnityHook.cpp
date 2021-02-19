#include "UnityHook.h"
#include "Debug.h"
#include "ExporterMacros.h"
#include "ImporterMacros.h"

UnityHook* UnityHook::Instance = nullptr;

IMPLEMENT_EXPORTED_CONSTRUCTOR(UnityHook, CreateUnityHook)
{
    if (Debug::LogToUnityIsValid())
        Debug::LogToUnity("UnityHook created");

    Instance = this;
}

IMPLEMENT_EXPORTED_DESTRUCTOR(UnityHook)
{
    Instance = nullptr;
    
    if (Debug::LogToUnityIsValid())
        Debug::LogToUnity("UnityHook destroyed");
}

IMPLEMENT_EXPORTED_FUNCTION(void, UnityHook, Update, const float, InDeltaTime)
{
    DeltaTime = InDeltaTime;
}