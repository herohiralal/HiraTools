#pragma once

#include "SyntacticMacros.h"

class HiraObjectRegistry;

DECLARE_FLAGS(int, UpdateType, None = 0, Update = 1 << 0, LateUpdate = 1 << 1, FixedUpdate = 1 << 2)

class HiraObject
{
    friend HiraObjectRegistry;
    
PROPERTY(bool, Enabled, STD, CUSTOM)
PROPERTY(EUpdateType, UpdateType, STD, NONE)

public:
    explicit HiraObject(EUpdateType UpdateTypes = EUpdateType::None, bool StartEnabled = true);
    virtual ~HiraObject() = default;

    void Initialize();
    void Destroy();

    virtual void OnAwake();
    virtual void OnEnable();
    virtual void OnUpdate(const float UnscaledDeltaTime, const float DeltaTime);
    virtual void OnFixedUpdate(float FixedUnscaledDeltaTime, float DeltaTime);
    virtual void OnLateUpdate(const float UnscaledDeltaTime, float DeltaTime);
    virtual void OnDisable();
    virtual void OnDestroy();
};