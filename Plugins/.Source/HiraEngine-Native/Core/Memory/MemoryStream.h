#pragma once

#include "Platform/Platform.h"

/**
 * Provides a clean interface to deal with raw memory and append different
 * objects to it.
 * NOTE - By intentional design, this utility does not call any copy or move
 * constructors, and instead relies purely on copying the memory.
 */
class MemoryStream
{
    /** The handle for the memory stream. */
    uint8* Handle;

    /** The size of the allocated buffer. */
    size BufferSize;

    /** The size of buffer currently occupied. */
    size FilledSize;

public:

    /**
     * Create a new MemoryStream.
     * @param StartingSize      The size (in bytes) to allocate.
     */
    explicit MemoryStream(size StartingSize = 0);

    /**
     * Destroy the MemoryStream.
     * NOTE - IF AcquireHandle() IS NOT CALLED, IT WILL FREE THE ALLOCATED MEMORY.
     */
    ~MemoryStream();

    /** Deleted copy constructor. */
    MemoryStream(const MemoryStream& Other) = delete;

    /** Deleted copy-assignment operator. */
    MemoryStream& operator=(const MemoryStream& Other) = delete;

    /**
     * Resize the allocated buffer.
     * @param NewSize           The new size (in bytes) to allocate.
     */
    void Resize(size NewSize);

    /**
     * Append an object to the memory stream.
     * @tparam T                The type of object to append.
     * @param Input             The object to copy to the stream.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    template <typename T>
    MemoryStream& Append(const T& Input);

    /**
     * Append an object to the memory stream, and find out its index in the stream.
     * @tparam T                The type of object to append.
     * @param Input             The object to copy to the stream.
     * @param OutIndex          The index of the object.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    template <typename T>
    MemoryStream& Append(const T& Input, size& OutIndex);

    /**
     * Append empty <possibly-uninitialized> bytes to the memory stream.
     * @param Count             The number of bytes to ignore.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    MemoryStream& AppendEmptyBytes(size Count);

    /**
     * Append empty <possibly-uninitialized> bytes to the memory stream.
     * @param Count             The number of bytes to ignore.
     * @param OutIndex          The index of the first empty byte.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    MemoryStream& AppendEmptyBytes(size Count, size& OutIndex);

    /**
     * Acquire the handle of this memory stream.
     * @returns A pointer to the first object in the stream.
     */
    uint8* AcquireHandle();

private:

    /**
     * Append a number of bytes at the end of the memory stream.
     * @param Input             The index of the first byte to append.
     * @param InputSize         The number of bytes to append.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    MemoryStream& Append(const uint8* Input, size InputSize);

    /**
     * Append a number of bytes at the end of the memory stream.
     * @param Input             The index of the first byte to append.
     * @param InputSize         The number of bytes to append.
     * @param OutIndex          The index of the appended object.
     * @returns A reference to the current MemoryStream, for a builder-like interface.
     */
    MemoryStream& Append(const uint8* Input, size InputSize, size& OutIndex);
};

template <typename T>
MemoryStream& MemoryStream::Append(const T& Input)
{
    return Append(reinterpret_cast<const uint8*>(&Input), sizeof(T));
}

template <typename T>
MemoryStream& MemoryStream::Append(const T& Input, size& OutIndex)
{
    return Append(reinterpret_cast<const uint8*>(&Input), sizeof(T), OutIndex);
}
