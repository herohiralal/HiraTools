#pragma once

#include "SyntacticMacros.h"

struct SUnityHookInitParams
{
public:
    int HiraObjectRegistryInitReserveSize;
};

class HiraObjectRegistry;

class UnityHook final
{
    DECLARE_SINGLETON(UnityHook)
    PROPERTY(HiraObjectRegistry*, Registry, STD, NONE)
    PROPERTY(float, DeltaTime, STD, NONE)
    PROPERTY(float, UnscaledDeltaTime, STD, NONE)
    PROPERTY(float, FixedDeltaTime, STD, NONE)
    PROPERTY(float, FixedUnscaledDeltaTime, STD, NONE)

public:
    explicit UnityHook(const SUnityHookInitParams& InitParams);
    ~UnityHook();
    void Dispose();

    void Update(float InUnscaledDeltaTime, float InDeltaTime);
    void FixedUpdate(float InFixedUnscaledDeltaTime, float InFixedDeltaTime);
    void LateUpdate(float InUnscaledDeltaTime, float InDeltaTime);
};