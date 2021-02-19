#pragma once

#include "SyntacticMacros.h"

class UnityHook final
{
    DECLARE_SINGLETON(UnityHook)
    PROPERTY(float, DeltaTime, STD, NONE)

public:
    UnityHook();
    ~UnityHook();

    void Update(float InDeltaTime);
};