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
    explicit TList(int StartBufferSize);
    ~TList();

    T& operator[](int Index);

    bool Contains(T Item);
    int FindIndex(T Item);
    int Add(T Item);
    bool Remove(T Item);
    bool RemoveAt(int Index);
};

template <typename T>
TList<T>::TList(const int StartBufferSize)
{
    BufferSize = StartBufferSize;
    Container = new T[StartBufferSize];
    ElementCount = 0;
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
bool TList<T>::Contains(const T Item)
{
    for (auto I = 0; I < ElementCount; I++)
    {
        if (Container[I] == Item)
            return true;
    }

    return false;
}

template <typename T>
int TList<T>::FindIndex(T Item)
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
    if(Index>=ElementCount) return false;
    
    const auto LastIndex = ElementCount - 1;
    for (auto I = Index; I < LastIndex; I++)
        Container[I] = Container[I + 1];

    ElementCount--;
    return true;
}
