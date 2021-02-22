#pragma once

#include "SyntacticMacros.h"

class NativeObjectRegistry;

DECLARE_FLAGS(int, UpdateType, None = 0, Update = 1 << 0, LateUpdate = 1 << 1, FixedUpdate = 1 << 2)

class NativeObject
{
    friend NativeObjectRegistry;
    
PROPERTY(bool, Enabled, STD, CUSTOM)
PROPERTY(EUpdateType, UpdateType, STD, NONE)

public:
    explicit NativeObject(EUpdateType UpdateTypes = EUpdateType::None, bool StartEnabled = true);
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