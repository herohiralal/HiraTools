#pragma once

#include "SyntacticMacros.h"

struct SUnityHookInitParams
{
public:
    int HiraObjectRegistryInitReserveSize;
};

class UnityHook final
{
    DECLARE_SINGLETON(UnityHook)
    PROPERTY(float, DeltaTime, STD, NONE)

public:
    explicit UnityHook(SUnityHookInitParams InitParams);
    ~UnityHook();

    void Update(float InDeltaTime);
};