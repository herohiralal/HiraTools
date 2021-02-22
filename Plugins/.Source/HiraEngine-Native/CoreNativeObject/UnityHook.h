#pragma once

#include "Core.h"

struct SUnityHookInitParams
{
public:
    int32 NativeObjectRegistryInitReserveSize;
};

class NativeObjectRegistry;

class UnityHook final
{
DECLARE_SINGLETON(UnityHook)
PROPERTY(NativeObjectRegistry*, Registry, STD, NONE)
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