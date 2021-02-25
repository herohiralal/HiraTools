#pragma once

#include "Core.h"

class NativeObjectRegistry;

DECLARE_FLAGS(uint8, UpdateType, None = 0, Update = 1 << 0, LateUpdate = 1 << 1, FixedUpdate = 1 << 2)

class NativeObject
{
    friend NativeObjectRegistry;
    
BITFIELD(uint8, Enabled, STD, CUSTOM)
PROPERTY(EUpdateType, UpdateType, STD, NONE)

public:
    explicit NativeObject(EUpdateType UpdateTypes = EUpdateType::None, bool8 StartEnabled = true);
    virtual ~NativeObject() = default;

    void Initialize();
    void Destroy();

    virtual void OnAwake();
    virtual void OnEnable();
    virtual void OnUpdate(float UnscaledDeltaTime, float DeltaTime);
    virtual void OnFixedUpdate(float FixedUnscaledDeltaTime, float DeltaTime);
    virtual void OnLateUpdate(float UnscaledDeltaTime, float DeltaTime);
    virtual void OnDisable();
    virtual void OnDestroy();
};