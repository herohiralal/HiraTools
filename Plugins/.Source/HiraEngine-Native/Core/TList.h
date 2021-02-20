#pragma once

#include "Debug.h"
#include "SyntacticMacros.h"

template <typename T>
class TList
{
PROPERTY(T*, Container, NONE, NONE)
PROPERTY(int, BufferSize, STD, CUSTOM)
PROPERTY(int, ElementCount, STD, NONE)

public:
    explicit TList(int StartBufferSize);
    ~TList();

    T& operator[](int Index);

    void Add(T Item);
    void Remove(T Item);
    void RemoveAt(int Index);
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
void TList<T>::Add(T Item)
{
    if (BufferSize == ElementCount)
        SetBufferSize(BufferSize * 2);

    Container[ElementCount] = Item;
    ElementCount++;
}

template <typename T>
void TList<T>::Remove(const T Item)
{
    for (auto I = 0; I < ElementCount; I++)
        if (Container[I] == Item)
        {
            RemoveAt(I);
            return;
        }
}

template <typename T>
void TList<T>::RemoveAt(const int Index)
{
    const auto LastIndex = ElementCount - 1;
    for (auto I = Index; I < LastIndex; I++)
        Container[I] = Container[I + 1];

    ElementCount--;
}
