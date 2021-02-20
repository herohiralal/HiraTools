#include "HiraObjectRegistryModification.h"

void HiraObjectRegistryAddition::ApplyTo(TList<HiraObject*>& Registry)
{
    Registry.Add(ToAdd);
}

void HiraObjectRegistryRemoval::ApplyTo(TList<HiraObject*>& Registry)
{
    Registry.Remove(ToRemove);
}
