#pragma once

#include "TemplateConstraintsHelper.h"
#include "TList.h"
#include "Internal/HiraObjectRegistryModification.h"

class UnityHook;

class HiraObject
{
    friend UnityHook;

private:
    static TList<HiraObject*> HiraObjectRegistry;
    static TList<IHiraObjectRegistryModification*> HiraObjectRegistryCommandBuffer;

public:
    virtual ~HiraObject() = default;

    template <typename T>
    static HiraObject* CreateHiraObject();

    static void DestroyHiraObject(HiraObject* ToDestroy);
};

template <typename T>
HiraObject* HiraObject::CreateHiraObject()
{
    AssignableTo<T, HiraObject>();

    T* NewObject = new T();
    HiraObjectRegistryCommandBuffer.Add(NewObject);
    return NewObject;
}