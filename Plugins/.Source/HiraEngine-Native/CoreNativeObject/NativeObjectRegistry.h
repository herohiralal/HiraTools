#pragma once

#include "SyntacticMacros.h"
#include "TList.h"

class NativeObject;

DECLARE_ENUM(int, RegistryIterationType, None, Update, FixedUpdate, LateUpdate, Disposal)

class NativeObjectRegistry
{
PROPERTY(TList<NativeObject*>, Registry, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToUpdate, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToFixedUpdate, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToLateUpdate, NONE, NONE)
PROPERTY(ERegistryIterationType, CurrentIterationType, NONE, NONE)
PROPERTY(int, CurrentIterationIndex, NONE, NONE)

public:
    explicit NativeObjectRegistry(int InitReserveSize);
    ~NativeObjectRegistry();

    void Update(const float UnscaledDeltaTime, const float DeltaTime);
    void FixedUpdate(const float FixedUnscaledDeltaTime, const float FixedDeltaTime);
    void LateUpdate(const float UnscaledDeltaTime, const float DeltaTime);
    void Dispose();
    
    void Register(NativeObject* Target);
    void Unregister(NativeObject* Target);
    void Enable(NativeObject* Target);
    void Disable(NativeObject* Target);

private:
    void AddToTickingLists(NativeObject* Target);
    void RemoveFromTickingLists(NativeObject* Target);
    void RemoveFromListAndUpdateIndex(TList<NativeObject*>& List, NativeObject* Target);
};
