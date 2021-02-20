#pragma once

#include "Debug.h"
#include "SyntacticMacros.h"

template <typename T>
class TList
{
PROPERTY(T*, Container, STD, NONE)
PROPERTY(int, BufferSize, STD, CUSTOM)
PROPERTY(int, ElementCount, STD, NONE)

public:
    TList(); // parameterless constructor
    explicit TList(int StartBufferSize); // basic constructor
    TList(const TList& Other); // copy constructor
    TList& operator=(const TList& Other); // copy assignment
    TList(TList&& Other) noexcept; // move constructor
    TList& operator=(TList&& Other) noexcept; // move assignment
    ~TList(); // destructor

    T& operator[](int Index);
    const T& operator[](int Index) const;

    bool Contains(T Item) const;
    int FindIndex(T Item) const;
    int Add(T Item);
    int Append(const TList<T>& ToAppend);
    bool Remove(T Item);
    bool RemoveAt(int Index);

    static TList<T> Combine(const TList<T>& A, const TList<T>& B);
};

template <typename T>
TList<T>::TList() : TList(0)
{
}

template <typename T>
TList<T>::TList(const int StartBufferSize)
{
    BufferSize = StartBufferSize;
    Container = new T[StartBufferSize];
    ElementCount = 0;
}

template <typename T>
TList<T>::TList(const TList& Other)
{
    // acquire new container
    const int OtherBufferSize = Other.BufferSize;
    auto NewContainer = new T[OtherBufferSize];

    // initialize new container
    const int OtherElementCount = Other.ElementCount;
    for (int I = 0; I < OtherElementCount; I++)
        NewContainer[I] = Other.Container[I];

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
        const int OtherBufferSize = Other.BufferSize;
        auto NewContainer = new T[OtherBufferSize];

        // initialize new container
        const int OtherElementCount = Other.ElementCount;
        for (int I = 0; I < OtherElementCount; I++)
            NewContainer[I] = Other.Container[I];

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
T& TList<T>::operator[](const int Index)
{
    if (Index >= BufferSize)
    {
        if (BufferSize)
        {
            UNITY_LOG(Exception, "Attempted to access outside HiraList buffer.")
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
const T& TList<T>::operator[](int Index) const
{
    if (Index >= BufferSize)
    {
        if (BufferSize)
        {
            UNITY_LOG(Exception, "Attempted to access outside HiraList buffer.")
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
void TList<T>::SetBufferSize(const int InValue)
{
    ElementCount = ElementCount < InValue ? ElementCount : InValue;
    T* NewContainer = new T[InValue];
    for (auto I = 0; I < ElementCount; I++)
        NewContainer[I] = Container[I];
    delete[] Container;
    Container = NewContainer;
    BufferSize = InValue;
}

template <typename T>
bool TList<T>::Contains(const T Item) const
{
    for (auto I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
            return true;
    }

    return false;
}

template <typename T>
int TList<T>::FindIndex(T Item) const
{
    for (auto I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
            return I;
    }

    return -1;
}

template <typename T>
int TList<T>::Add(T Item)
{
    if (BufferSize == ElementCount)
        SetBufferSize(BufferSize * 2);

    Container[ElementCount] = Item;
    return ElementCount++;
}

template <typename T>
int TList<T>::Append(const TList<T>& ToAppend)
{
    const auto CurrentElementCount = ElementCount;
    const auto OtherElementCount = ToAppend.ElementCount;
    const auto TotalElementCount = CurrentElementCount + OtherElementCount;
    
    if (BufferSize < TotalElementCount) SetBufferSize(TotalElementCount);

    for (auto I = 0; I < OtherElementCount; I++)
        Container[CurrentElementCount + I] = ToAppend.Container[I];

    ElementCount = TotalElementCount;
    return CurrentElementCount;
}

template <typename T>
bool TList<T>::Remove(const T Item)
{
    for (auto I = 0; I < ElementCount; I++)
        if (Container[I] == Item)
        {
            return RemoveAt(I);
        }

    return false;
}

template <typename T>
bool TList<T>::RemoveAt(const int Index)
{
    if (Index >= ElementCount) return false;

    const auto LastIndex = ElementCount - 1;
    for (auto I = Index; I < LastIndex; I++)
        Container[I] = Container[I + 1];

    ElementCount--;
    return true;
}

template <typename T>
TList<T> TList<T>::Combine(const TList<T>& A, const TList<T>& B)
{
    TList<T> Output(A.ElementCount + B.ElementCount);

    Output.Append(A);
    Output.Append(B);

    return Output;
}
