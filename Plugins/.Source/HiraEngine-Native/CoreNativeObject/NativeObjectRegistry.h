#pragma once

#include "Core.h"

class NativeObject;

DECLARE_ENUM(uint8, RegistryIterationType, None, Update, FixedUpdate, LateUpdate, Disposal)

class NativeObjectRegistry
{
PROPERTY(TList<NativeObject*>, Registry, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToUpdate, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToFixedUpdate, NONE, NONE)
PROPERTY(TList<NativeObject*>, ToLateUpdate, NONE, NONE)
PROPERTY(ERegistryIterationType, CurrentIterationType, NONE, NONE)
PROPERTY(int32, CurrentIterationIndex, NONE, NONE)

public:
    explicit NativeObjectRegistry(int32 InitReserveSize);
    ~NativeObjectRegistry();

    void Update(float UnscaledDeltaTime, float DeltaTime);
    void FixedUpdate(float FixedUnscaledDeltaTime, float FixedDeltaTime);
    void LateUpdate(float UnscaledDeltaTime, float DeltaTime);
    void Dispose();
    
    void Register(NativeObject* Target);
    void Unregister(NativeObject* Target);
    void Enable(NativeObject* Target);
    void Disable(NativeObject* Target);

private:
    void AddToTickingLists(NativeObject* Target);
    void RemoveFromTickingLists(NativeObject* Target);
    void RemoveFromListAndUpdateIndex(TList<NativeObject*>& List, const NativeObject* Target);
};
