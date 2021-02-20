#include "HiraObject.h"

TList<HiraObject*> HiraObject::HiraObjectRegistry = TList<HiraObject*>(0);
TList<IHiraObjectRegistryModification*> HiraObject::HiraObjectRegistryCommandBuffer = TList<IHiraObjectRegistryModification*>(10);

void HiraObject::DestroyHiraObject(HiraObject* ToDestroy)
{
    HiraObjectRegistryCommandBuffer.Add(new HiraObjectRegistryRemoval(ToDestroy));
}
