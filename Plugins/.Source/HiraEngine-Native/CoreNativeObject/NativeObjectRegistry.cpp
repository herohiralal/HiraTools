#include "NativeObject.h"
#include "NativeObjectRegistry.h"

NativeObjectRegistry::NativeObjectRegistry(const int32 InitReserveSize)
    : Registry(InitReserveSize),
      ToUpdate(InitReserveSize * 0.5f),
      ToFixedUpdate(5),
      ToLateUpdate(5),
      CurrentIterationType(ERegistryIterationType::None),
      CurrentIterationIndex(-1)
{
}

NativeObjectRegistry::~NativeObjectRegistry()
{
}

void NativeObjectRegistry::Update(const float UnscaledDeltaTime, const float DeltaTime)
{
    CurrentIterationType = ERegistryIterationType::Update;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToUpdate.GetElementCount(); CurrentIterationIndex++)
        {
            ToUpdate[CurrentIterationIndex]->OnUpdate(UnscaledDeltaTime, DeltaTime);
        }

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void NativeObjectRegistry::FixedUpdate(const float FixedUnscaledDeltaTime, const float FixedDeltaTime)
{
    CurrentIterationType = ERegistryIterationType::FixedUpdate;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToFixedUpdate.GetElementCount(); CurrentIterationIndex++)
        {
            ToFixedUpdate[CurrentIterationIndex]->OnFixedUpdate(FixedUnscaledDeltaTime, FixedDeltaTime);
        }

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void NativeObjectRegistry::LateUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    CurrentIterationType = ERegistryIterationType::LateUpdate;
    {
        for (CurrentIterationIndex = 0; CurrentIterationIndex < ToLateUpdate.GetElementCount(); CurrentIterationIndex++)
        {
            ToLateUpdate[CurrentIterationIndex]->OnLateUpdate(UnscaledDeltaTime, DeltaTime);
        }

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void NativeObjectRegistry::Dispose()
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

        CurrentIterationIndex = -1;
    }
    CurrentIterationType = ERegistryIterationType::None;
}

void NativeObjectRegistry::Register(NativeObject* Target)
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

void NativeObjectRegistry::Unregister(NativeObject* Target)
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

void NativeObjectRegistry::Enable(NativeObject* Target)
{
    if (!Target->GetEnabled())
    {
        if (CurrentIterationType != ERegistryIterationType::Disposal)
            AddToTickingLists(Target);

        Target->Enabled = true;
        Target->OnEnable();
    }
}

void NativeObjectRegistry::Disable(NativeObject* Target)
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

void NativeObjectRegistry::AddToTickingLists(NativeObject* Target)
{
    const auto TargetUpdateType = Target->GetUpdateType();
    MODIFY_LIST_IF_APPROPRIATE(Update, Add)
    MODIFY_LIST_IF_APPROPRIATE(FixedUpdate, Add)
    MODIFY_LIST_IF_APPROPRIATE(LateUpdate, Add)
}

void NativeObjectRegistry::RemoveFromTickingLists(NativeObject* Target)
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

void NativeObjectRegistry::RemoveFromListAndUpdateIndex(TList<NativeObject*>& List, const NativeObject* Target)
{
    const int32 TargetIndex = List.FindIndex(Target);
    if (TargetIndex < 0)
    {
        UNITY_EDITOR_LOG(Error, L"Attempted to remove an index from a ticking list that didn't contain it.")
    }
    else
    {
        List.RemoveAt(TargetIndex);
        CurrentIterationIndex -= TargetIndex <= CurrentIterationIndex
                                     ? 1
                                     : 0;
    }
}
