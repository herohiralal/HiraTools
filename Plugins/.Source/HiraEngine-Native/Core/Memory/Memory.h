#pragma once

#include "Platform/Platform.h"

/**
 * Utility functions for memory manipulation.
 */
struct SMemory
{
    /**
     * Copy an array of elements onto another, byte-by-byte.
     * @param Destination Destination address.
     * @param Source Source address.
     * @param Count Number of elements.
     */
    template <typename T>
    static void Copy(T* Destination, const T* Source, size Count);

    /**
     * Copy a block of memory onto another, byte-by-byte.
     * @param Destination Destination block address.
     * @param Source Source block address.
     * @param Size The size of the block in bytes.
     */
    static void Copy(uint8* Destination, const uint8* Source, size Size);
};

template <typename T>
void SMemory::Copy(T* Destination, const T* Source, const size Count)
{
    Copy(reinterpret_cast<uint8*>(Destination), reinterpret_cast<const uint8*>(Source), Count * sizeof(T));
}

namespace MoveUtilityInternals
{
    template <typename T>
    struct RemoveReference
    {
        using Type = T;
        using ConstThroughRefType = const T;
    };

    template <typename T>
    struct RemoveReference<T&>
    {
        using Type = T;
        using ConstThroughRefType = const T&;
    };

    template <typename T>
    struct RemoveReference<T&&>
    {
        using Type = T;
        using ConstThroughRefType = const T&&;
    };

    template <typename T>
    using RemoveReferenceT = typename RemoveReference<T>::Type;

    template <typename T>
    using ConstThroughRef = typename RemoveReference<T>::ConstThroughRefType;
}

template <typename T>
constexpr MoveUtilityInternals::RemoveReferenceT<T>&& Move(T&& Arg) noexcept
{
    return static_cast<MoveUtilityInternals::RemoveReferenceT<T>&&>(Arg);
}