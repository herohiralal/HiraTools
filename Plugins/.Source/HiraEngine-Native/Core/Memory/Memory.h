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
