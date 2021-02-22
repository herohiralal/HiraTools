#pragma once

#include "TList.h"

template <typename T>
struct TListModification
{
    typedef void (*ModificationDelegate)(TList<T>&, T);

    TListModification();
    TListModification(T InTarget, ModificationDelegate InModification);

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
    explicit TListModificationBuffer(int32 InitReserveSize,
                                     ModificationDelegate InAdditionDelegate = &TListModificationBuffer<T>::DefaultAddition,
                                     ModificationDelegate InRemovalDelegate = &TListModificationBuffer<T>::DefaultRemoval);

    int32 GetCommandCount() const;
    int32 GetBufferSize() const;
    void SetBufferSize(int32 NewSize);
    
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
    int32 InitReserveSize,
    const ModificationDelegate InAdditionDelegate,
    const ModificationDelegate InRemovalDelegate
)
    : CommandBuffer(InitReserveSize),
      AdditionDelegate(InAdditionDelegate),
      RemovalDelegate(InRemovalDelegate)
{
}

template <typename T>
int32 TListModificationBuffer<T>::GetCommandCount() const
{
    return CommandBuffer.GetElementCount();
}

template <typename T>
int32 TListModificationBuffer<T>::GetBufferSize() const
{
    return CommandBuffer.GetBufferSize();
}

template <typename T>
void TListModificationBuffer<T>::SetBufferSize(int32 NewSize)
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
    const int32 Count = CommandBuffer.GetElementCount();
    for (uint16 I = 0; I < Count; I++)
    {
        const TListModification<T>& Current = CommandBuffer[I];
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
