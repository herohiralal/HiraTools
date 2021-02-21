#include "HiraObject.h"
#include "HiraObjectRegistry.h"
#include "UnityHook.h"

HiraObject::HiraObject(const EUpdateType UpdateTypes, const bool StartEnabled) : Enabled(StartEnabled), UpdateType(UpdateTypes)
{
}

void HiraObject::Initialize()
{
    UnityHook::Get()
        .GetRegistry()
        ->Register(this);
}

void HiraObject::Destroy()
{
    UnityHook::Get()
        .GetRegistry()
        ->Unregister(this);
}

void HiraObject::SetEnabled(const bool InValue)
{
    if (GetEnabled() == InValue) return;

    if (InValue)
    {
        UnityHook::Get()
            .GetRegistry()
            ->Enable(this);
    }
    else
    {
        UnityHook::Get()
            .GetRegistry()
            ->Disable(this);
    }
}


#pragma region Messages

void HiraObject::OnAwake()
{
    // Empty in base class
}

void HiraObject::OnEnable()
{
    // Empty in base class
}

void HiraObject::OnUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void HiraObject::OnFixedUpdate(const float FixedUnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void HiraObject::OnLateUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void HiraObject::OnDisable()
{
    // Empty in base class
}

void HiraObject::OnDestroy()
{
    // Empty in base class
}

#pragma endregion Messages
