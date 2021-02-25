#include "NativeObject.h"
#include "NativeObjectRegistry.h"
#include "UnityHook.h"

NativeObject::NativeObject(const EUpdateType UpdateTypes, const bool8 StartEnabled) : Enabled(StartEnabled), UpdateType(UpdateTypes)
{
}

void NativeObject::Initialize()
{
    UnityHook::Get()
        .GetRegistry()
        ->Register(this);
}

void NativeObject::Destroy()
{
    UnityHook::Get()
        .GetRegistry()
        ->Unregister(this);
}

void NativeObject::SetEnabled(const bool8 InValue)
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

void NativeObject::OnAwake()
{
    // Empty in base class
}

void NativeObject::OnEnable()
{
    // Empty in base class
}

void NativeObject::OnUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void NativeObject::OnFixedUpdate(const float FixedUnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void NativeObject::OnLateUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    // Empty in base class
}

void NativeObject::OnDisable()
{
    // Empty in base class
}

void NativeObject::OnDestroy()
{
    // Empty in base class
}

#pragma endregion Messages
