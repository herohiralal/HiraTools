#pragma once

#include "Debug.h"
#include "SyntacticMacros.h"
#include "Platform/Platform.h"

template <typename T>
class TList
{
PROPERTY(T*, Container, STD, NONE)
PROPERTY(int32, BufferSize, STD, CUSTOM)
PROPERTY(int32, ElementCount, STD, STD)

    template <typename TRandom>
    struct ConstArgumentType { using Type = const TRandom&; };
    template <typename TRandom>
    struct ConstArgumentType<TRandom*> { using Type = const TRandom*; };

    typedef typename ConstArgumentType<T>::Type ConstT;

public:
    TList(); // parameterless constructor
    explicit TList(int32 StartBufferSize); // basic constructor
    TList(const TList& Other); // copy constructor
    TList& operator=(const TList& Other); // copy assignment
    TList(TList&& Other) noexcept; // move constructor
    TList& operator=(TList&& Other) noexcept; // move assignment
    ~TList(); // destructor

    T& operator[](int32 Index);
    const T& operator[](int32 Index) const;

int32 ModifyBufferSize(int32 Delta);

    bool Contains(ConstT Item) const;
    int32 FindIndex(ConstT Item) const;

    void Clear();
    int32 Add(T Item);
    void Push(T Item);
    int32 Append(const TList<T>& ToAppend);
    bool Remove(T Item);
    bool RemoveAt(int32 Index);
    T Pop();

    static TList<T> Combine(const TList<T>& A, const TList<T>& B);
};

template <typename T>
TList<T>::TList() : TList(0)
{
}

template <typename T>
TList<T>::TList(int32 StartBufferSize)
{
    StartBufferSize = StartBufferSize < 0 ? 0 : StartBufferSize;
    BufferSize = StartBufferSize;
    Container = new T[StartBufferSize];
    ElementCount = 0;
}

template <typename T>
TList<T>::TList(const TList& Other)
{
    // acquire new container
    const int32 OtherBufferSize = Other.BufferSize;
    T* NewContainer = new T[OtherBufferSize];

    // initialize new container
    const int32 OtherElementCount = Other.ElementCount;
    for (uint16 I = 0; I < OtherElementCount; I++)
    {
        NewContainer[I] = Other.Container[I];
    }

    Container = NewContainer;
    BufferSize = OtherBufferSize;
    ElementCount = OtherElementCount;
}

template <typename T>
TList<T>& TList<T>::operator=(const TList& Other)
{
    if (this != &Other)
    {
        // acquire new container
        const int32 OtherBufferSize = Other.BufferSize;
        T* NewContainer = new T[OtherBufferSize];

        // initialize new container
        const int32 OtherElementCount = Other.ElementCount;
        for (uint16 I = 0; I < OtherElementCount; I++)
        {
            NewContainer[I] = Other.Container[I];
        }

        delete[] Container;
        Container = NewContainer;
        BufferSize = OtherBufferSize;
        ElementCount = OtherElementCount;
    }

    return *this;
}

template <typename T>
TList<T>::TList(TList&& Other) noexcept
{
    BufferSize = Other.BufferSize;
    ElementCount = Other.ElementCount;
    Container = Other.Container;

    Other.Container = nullptr;
}

template <typename T>
TList<T>& TList<T>::operator=(TList&& Other) noexcept
{
    if (this != &Other)
    {
        delete[] Container;

        BufferSize = Other.BufferSize;
        ElementCount = Other.ElementCount;
        Container = Other.Container;

        Other.Container = nullptr;
    }

    return *this;
}

template <typename T>
TList<T>::~TList()
{
    delete[] Container;
    BufferSize = 0;
    ElementCount = 0;
}

template <typename T>
T& TList<T>::operator[](const int32 Index)
{
    if (Index < 0)
    {
        UNITY_LOG(Exception, "Attempted to access a negative index.")
    }

    if (Index >= BufferSize)
    {
        if (BufferSize)
        {
            UNITY_LOG(Exception, "Attempted to access outside List buffer.")
            return Container[0];
        }
        else
        {
            UNITY_LOG(Exception, "Attempted to access an item from an empty array.")
            return *new T();
        }
    }

    return Container[Index];
}

template <typename T>
const T& TList<T>::operator[](int32 Index) const
{
    if (Index < 0)
    {
        UNITY_LOG(Exception, "Attempted to access a negative index.")
    }

    if (Index >= BufferSize)
    {
        if (BufferSize)
        {
            UNITY_LOG(Exception, "Attempted to access outside List buffer.")
            return Container[0];
        }
        else
        {
            UNITY_LOG(Exception, "Attempted to access an item from an empty array.")
            return *new T();
        }
    }

    return Container[Index];
}

template <typename T>
int32 TList<T>::ModifyBufferSize(const int32 Delta)
{
    const int32 NewBufferSize = BufferSize + Delta;
    SetBufferSize(NewBufferSize);
    return NewBufferSize;
}

template <typename T>
void TList<T>::SetBufferSize(int32 InValue)
{
    InValue = InValue < 0 ? 0 : InValue;

    ElementCount = ElementCount < InValue ? ElementCount : InValue;
    T* NewContainer = new T[InValue];
    for (uint16 I = 0; I < ElementCount; I++)
    {
        NewContainer[I] = Container[I];
    }
    delete[] Container;
    Container = NewContainer;
    BufferSize = InValue;
}

template <typename T>
bool TList<T>::Contains(ConstT Item) const
{
    for (uint16 I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
        {
            return true;
        }
    }

    return false;
}

template <typename T>
int32 TList<T>::FindIndex(ConstT Item) const
{
    for (uint16 I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
        {
            return I;
        }
    }

    return -1;
}

template <typename T>
void TList<T>::Clear()
{
    ElementCount = 0;
}

template <typename T>
int32 TList<T>::Add(T Item)
{
    Push(Item);
    return ElementCount - 1;
}

template <typename T>
void TList<T>::Push(T Item)
{
    if (BufferSize == ElementCount)
    {
        SetBufferSize(BufferSize * 2);
    }

    Container[ElementCount] = Item;
    ElementCount++;
}

template <typename T>
int32 TList<T>::Append(const TList<T>& ToAppend)
{
    const int32 CurrentElementCount = ElementCount;
    const int32 OtherElementCount = ToAppend.ElementCount;
    const int32 TotalElementCount = CurrentElementCount + OtherElementCount;

    if (BufferSize < TotalElementCount) SetBufferSize(TotalElementCount);

    for (uint16 I = 0; I < OtherElementCount; I++)
    {
        Container[CurrentElementCount + I] = ToAppend.Container[I];
    }

    ElementCount = TotalElementCount;
    return CurrentElementCount;
}

template <typename T>
bool TList<T>::Remove(const T Item)
{
    for (uint16 I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
        {
            return RemoveAt(I);
        }
    }

    return false;
}

template <typename T>
bool TList<T>::RemoveAt(const int32 Index)
{
    if (Index >= ElementCount) return false;

    const int32 LastIndex = ElementCount - 1;
    for (int32 I = Index; I < LastIndex; I++)
    {
        Container[I] = Container[I + 1];
    }

    ElementCount--;
    return true;
}

template <typename T>
T TList<T>::Pop()
{
    return Container[--ElementCount];
}

template <typename T>
TList<T> TList<T>::Combine(const TList<T>& A, const TList<T>& B)
{
    TList<T> Output(A.ElementCount + B.ElementCount);

    Output.Append(A);
    Output.Append(B);

    return Output;
}
