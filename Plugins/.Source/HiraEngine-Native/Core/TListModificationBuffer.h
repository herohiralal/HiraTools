#pragma once

#include "TList.h"

template <typename T>
struct TListModification
{
    typedef void (*ModificationDelegate)(TList<T>&, T);

    TListModification();
    TListModification(T InTarget, const ModificationDelegate InModification);

    T Target;
    ModificationDelegate Delegate;

    bool operator==(const TListModification& Other) const;
    bool operator!=(const TListModification& Other) const;
    TListModification& operator=(const TListModification& Other);
    TListModification& operator=(const TListModification&& Other);
};

template <typename T>
TListModification<T>::TListModification()
    : Target(),
      Delegate(nullptr)
{
}

template <typename T>
TListModification<T>::TListModification(T InTarget, const ModificationDelegate InModification)
    : Target(InTarget),
      Delegate(InModification)
{
}

template <typename T>
bool TListModification<T>::operator==(const TListModification& Other) const
{
    return Target == Other.Target && Delegate == Other.Delegate;
}

template <typename T>
bool TListModification<T>::operator!=(const TListModification& Other) const
{
    return !(this == Other);
}

template <typename T>
TListModification<T>& TListModification<T>::operator=(const TListModification& Other)
{
    Target = Other.Target;
    Delegate = Other.Delegate;

    return *this;
}

template <typename T>
TListModification<T>& TListModification<T>::operator=(const TListModification&& Other)
{
    Target = Other.Target;
    Delegate = Other.Delegate;

    return *this;
}

template <typename T>
class TListModificationBuffer
{
    typedef void (*ModificationDelegate)(TList<T>&, T);

public:
    TListModificationBuffer();
    explicit TListModificationBuffer(int InitReserveSize,
                                     ModificationDelegate InAdditionDelegate = &TListModificationBuffer<T>::DefaultAddition,
                                     ModificationDelegate InRemovalDelegate = &TListModificationBuffer<T>::DefaultRemoval);

    int GetCommandCount() const;
    int GetBufferSize() const;
    void SetBufferSize(int NewSize);
    
    void ScheduleAddition(T Target);
    void ScheduleRemoval(T Target);
    void ExecuteAndClear(TList<T>& Target);
    void ExecuteOn(TList<T>& Target) const;
    void ClearCommandBuffer();

private:
    TList<TListModification<T>> CommandBuffer;
    const ModificationDelegate AdditionDelegate;
    const ModificationDelegate RemovalDelegate;

    static void DefaultAddition(TList<T>& List, T Target);
    static void DefaultRemoval(TList<T>& List, T Target);
};

template <typename T>
TListModificationBuffer<T>::TListModificationBuffer()
    : TListModificationBuffer<T>(0)
{
}

template <typename T>
TListModificationBuffer<T>::TListModificationBuffer
(
    int InitReserveSize,
    const ModificationDelegate InAdditionDelegate,
    const ModificationDelegate InRemovalDelegate
)
    : CommandBuffer(InitReserveSize),
      AdditionDelegate(InAdditionDelegate),
      RemovalDelegate(InRemovalDelegate)
{
}

template <typename T>
int TListModificationBuffer<T>::GetCommandCount() const
{
    return CommandBuffer.GetElementCount();
}

template <typename T>
int TListModificationBuffer<T>::GetBufferSize() const
{
    return CommandBuffer.GetBufferSize();
}

template <typename T>
void TListModificationBuffer<T>::SetBufferSize(int NewSize)
{
    CommandBuffer.SetBufferSize(NewSize);
}

template <typename T>
void TListModificationBuffer<T>::ScheduleAddition(T Target)
{
    CommandBuffer.Add(TListModification<T>(Target, AdditionDelegate));
}

template <typename T>
void TListModificationBuffer<T>::ScheduleRemoval(T Target)
{
    CommandBuffer.Add(TListModification<T>(Target, RemovalDelegate));
}

template <typename T>
void TListModificationBuffer<T>::ExecuteAndClear(TList<T>& Target)
{
    ExecuteOn(Target);
    ClearCommandBuffer();
}

template <typename T>
void TListModificationBuffer<T>::ExecuteOn(TList<T>& Target) const
{
    const auto Count = CommandBuffer.GetElementCount();
    for (int I = 0; I < Count; I++)
    {
        auto& Current = CommandBuffer[I];
        Current.Delegate(Target, Current.Target);
    }
}

template <typename T>
void TListModificationBuffer<T>::ClearCommandBuffer()
{
    CommandBuffer.Clear();
}

template <typename T>
void TListModificationBuffer<T>::DefaultAddition(TList<T>& List, T Target)
{
    List.Add(Target);
}

template <typename T>
void TListModificationBuffer<T>::DefaultRemoval(TList<T>& List, T Target)
{
    List.Remove(Target);
}
