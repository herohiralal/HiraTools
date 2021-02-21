#include "HiraObject.h"
#include "HiraObjectRegistry.h"

HiraObjectRegistry::HiraObjectRegistry(const int InitReserveSize)
    : Registry(InitReserveSize),
      ToUpdate(InitReserveSize * 0.5f),
      ToFixedUpdate(5),
      ToLateUpdate(5),
      CurrentIterationType(ERegistryIterationType::None),
      CurrentIterationIndex(-1)
{
}

HiraObjectRegistry::~HiraObjectRegistry()
{
}

void HiraObjectRegistry::Update(const float UnscaledDeltaTime, const float DeltaTime)
{
    CurrentIterationType = ERegistryIterationType::Update;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToUpdate.GetElementCount(); CurrentIterationIndex++)
            ToUpdate[CurrentIterationIndex]->OnUpdate(UnscaledDeltaTime, DeltaTime);

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void HiraObjectRegistry::FixedUpdate(const float FixedUnscaledDeltaTime, const float FixedDeltaTime)
{
    CurrentIterationType = ERegistryIterationType::FixedUpdate;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToFixedUpdate.GetElementCount(); CurrentIterationIndex++)
            ToFixedUpdate[CurrentIterationIndex]->OnFixedUpdate(FixedUnscaledDeltaTime, FixedDeltaTime);

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void HiraObjectRegistry::LateUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    CurrentIterationType = ERegistryIterationType::LateUpdate;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToLateUpdate.GetElementCount(); CurrentIterationIndex++)
            ToLateUpdate[CurrentIterationIndex]->OnLateUpdate(UnscaledDeltaTime, DeltaTime);

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void HiraObjectRegistry::Dispose()
{
    CurrentIterationType = ERegistryIterationType::Disposal;
    {
        ToUpdate.SetBufferSize(0);
        ToLateUpdate.SetBufferSize(0);
        ToFixedUpdate.SetBufferSize(0);

        for (CurrentIterationIndex = 0; CurrentIterationIndex < Registry.GetElementCount(); CurrentIterationIndex++)
        {
            auto Current = Registry[CurrentIterationIndex];

            const auto Enabled = Current->GetEnabled();
            if (Enabled)
            {
                Current->Enabled = false;
                Current->OnDisable();
            }
            Current->OnDestroy();

            delete Current;
        }
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void HiraObjectRegistry::Register(HiraObject* Target)
{
    const auto Enabled = Target->GetEnabled();

    if (CurrentIterationType == ERegistryIterationType::Disposal)
    {
        Target->OnAwake();
        if (Target->GetEnabled())
        {
            Target->OnEnable();
            Target->Enabled = false;
            Target->OnDisable();
        }
        Target->OnDestroy();
    }
    else
    {
        Registry.Add(Target);

        if (Enabled)
            AddToTickingLists(Target);

        Target->OnAwake();
        if (Enabled)
        {
            Target->Enabled = true;
            Target->OnEnable();
        }
    }
}

void HiraObjectRegistry::Unregister(HiraObject* Target)
{
    const auto Enabled = Target->GetEnabled();

    if (CurrentIterationType == ERegistryIterationType::Disposal)
    {
        RemoveFromListAndUpdateIndex(Registry, Target);
    }
    else
    {
        if (Enabled)
            RemoveFromTickingLists(Target);

        Registry.Remove(Target);
    }

    if (Enabled)
    {
        Target->Enabled = false;
        Target->OnDisable();
    }
    Target->OnDestroy();

    delete Target;
}

void HiraObjectRegistry::Enable(HiraObject* Target)
{
    if (!Target->GetEnabled())
    {
        if (CurrentIterationType != ERegistryIterationType::Disposal)
            AddToTickingLists(Target);

        Target->Enabled = true;
        Target->OnEnable();
    }
}

void HiraObjectRegistry::Disable(HiraObject* Target)
{
    if (Target->GetEnabled())
    {
        if (CurrentIterationType != ERegistryIterationType::Disposal)
            RemoveFromTickingLists(Target);

        Target->Enabled = false;
        Target->OnDisable();
    }
}

#define MODIFY_LIST_IF_APPROPRIATE(type, function) \
    if (HasFlag(TargetUpdateType, EUpdateType::type)) \
        To##type.function(Target);

#define REMOVE_FROM_TICKING_LIST_IF_APPROPRIATE(type) \
    if (HasFlag(TargetUpdateType, EUpdateType::type)) \
        RemoveFromListAndUpdateIndex(To##type, Target);

void HiraObjectRegistry::AddToTickingLists(HiraObject* Target)
{
    const auto TargetUpdateType = Target->GetUpdateType();
    MODIFY_LIST_IF_APPROPRIATE(Update, Add)
    MODIFY_LIST_IF_APPROPRIATE(FixedUpdate, Add)
    MODIFY_LIST_IF_APPROPRIATE(LateUpdate, Add)
}

void HiraObjectRegistry::RemoveFromTickingLists(HiraObject* Target)
{
    const auto TargetUpdateType = Target->GetUpdateType();
    switch (CurrentIterationType)
    {
        case ERegistryIterationType::None:
            MODIFY_LIST_IF_APPROPRIATE(Update, Remove)
            MODIFY_LIST_IF_APPROPRIATE(FixedUpdate, Remove)
            MODIFY_LIST_IF_APPROPRIATE(LateUpdate, Remove)
            break;
        case ERegistryIterationType::Update:
            MODIFY_LIST_IF_APPROPRIATE(FixedUpdate, Remove)
            MODIFY_LIST_IF_APPROPRIATE(LateUpdate, Remove)

            REMOVE_FROM_TICKING_LIST_IF_APPROPRIATE(Update)

            break;
        case ERegistryIterationType::LateUpdate:
            MODIFY_LIST_IF_APPROPRIATE(Update, Remove)
            MODIFY_LIST_IF_APPROPRIATE(FixedUpdate, Remove)

            REMOVE_FROM_TICKING_LIST_IF_APPROPRIATE(LateUpdate)

            break;
        case ERegistryIterationType::FixedUpdate:
            MODIFY_LIST_IF_APPROPRIATE(Update, Remove)
            MODIFY_LIST_IF_APPROPRIATE(LateUpdate, Remove)

            REMOVE_FROM_TICKING_LIST_IF_APPROPRIATE(FixedUpdate)

            break;
        default:
            break;
    }
}

#undef REMOVE_FROM_TICKING_LIST_IF_APPROPRIATE

#undef MODIFY_LIST_IF_APPROPRIATE

void HiraObjectRegistry::RemoveFromListAndUpdateIndex(TList<HiraObject*>& List, HiraObject* Target)
{
    const int TargetIndex = List.FindIndex(Target);
    if (TargetIndex < 0)
    {
        UNITY_EDITOR_LOG(Error, "Attempted to remove an index from a ticking list that didn't contain it.")
    }
    else
    {
        List.RemoveAt(TargetIndex);
        CurrentIterationIndex -= TargetIndex <= CurrentIterationIndex
                                     ? 1
                                     : 0;
    }
}
