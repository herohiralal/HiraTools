#pragma once

#include "SyntacticMacros.h"
#include "TList.h"

class HiraObject;

DECLARE_ENUM(int, RegistryIterationType, None, Update, FixedUpdate, LateUpdate, Disposal)

class HiraObjectRegistry
{
PROPERTY(TList<HiraObject*>, Registry, NONE, NONE)
PROPERTY(TList<HiraObject*>, ToUpdate, NONE, NONE)
PROPERTY(TList<HiraObject*>, ToFixedUpdate, NONE, NONE)
PROPERTY(TList<HiraObject*>, ToLateUpdate, NONE, NONE)
PROPERTY(ERegistryIterationType, CurrentIterationType, NONE, NONE)
PROPERTY(int, CurrentIterationIndex, NONE, NONE)

public:
    explicit HiraObjectRegistry(int InitReserveSize);
    ~HiraObjectRegistry();

    void Update(const float UnscaledDeltaTime, const float DeltaTime);
    void FixedUpdate(const float FixedUnscaledDeltaTime, const float FixedDeltaTime);
    void LateUpdate(const float UnscaledDeltaTime, const float DeltaTime);
    void Dispose();
    
    void Register(HiraObject* Target);
    void Unregister(HiraObject* Target);
    void Enable(HiraObject* Target);
    void Disable(HiraObject* Target);

private:
    void AddToTickingLists(HiraObject* Target);
    void RemoveFromTickingLists(HiraObject* Target);
    void RemoveFromListAndUpdateIndex(TList<HiraObject*>& List, HiraObject* Target);
};
