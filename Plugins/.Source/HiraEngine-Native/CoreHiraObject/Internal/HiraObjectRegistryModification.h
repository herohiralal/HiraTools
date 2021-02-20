#pragma once

#include "TList.h"

class HiraObject;

class IHiraObjectRegistryModification
{
public:
    virtual ~IHiraObjectRegistryModification() = default;
    virtual void ApplyTo(TList<HiraObject*>& Registry) = 0;
};

class final HiraObjectRegistryAddition : public IHiraObjectRegistryModification
{
    HiraObject* ToAdd = nullptr;
public:
    explicit HiraObjectRegistryAddition(HiraObject* InToAdd) : ToAdd(InToAdd)
    { }

    void ApplyTo(TList<HiraObject*>& Registry) override;
};

class final HiraObjectRegistryRemoval : public IHiraObjectRegistryModification
{
    HiraObject* ToRemove = nullptr;
public:
    explicit HiraObjectRegistryRemoval(HiraObject* InToRemove) : ToRemove(InToRemove)
    { }

    void ApplyTo(TList<HiraObject*>& Registry) override;
};
