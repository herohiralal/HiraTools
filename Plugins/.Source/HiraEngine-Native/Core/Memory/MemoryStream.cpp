#include "MemoryStream.h"
#include "Memory.h"
#include "Math/Math.h"

MemoryStream::MemoryStream(const size StartingSize)
{
    Handle = new uint8[StartingSize];
    BufferSize = StartingSize;
    FilledSize = 0;
}

MemoryStream::~MemoryStream()
{
    delete[] Handle;
    Handle = nullptr;
    BufferSize = 0;
    FilledSize = 0;
}

void MemoryStream::Resize(const size NewSize)
{
    const size NewFilledSize = SMath::Min(FilledSize, NewSize);

    uint8* NewHandle = new uint8[NewSize];
    SMemory::Copy(NewHandle, Handle, NewFilledSize);

    delete[] Handle;
    Handle = NewHandle;
    BufferSize = NewSize;
    FilledSize = NewFilledSize;
}

MemoryStream& MemoryStream::AppendEmptyBytes(const size Count)
{
    if (FilledSize + Count > BufferSize) Resize(FilledSize + Count);

    FilledSize += Count;

    return *this;
}

MemoryStream& MemoryStream::AppendEmptyBytes(const size Count, size& OutIndex)
{
    OutIndex = FilledSize;

    return AppendEmptyBytes(Count);
}

uint8* MemoryStream::AcquireHandle()
{
    uint8* OwnedHandle = Handle;

    Handle = nullptr;
    BufferSize = 0;
    FilledSize = 0;
    return OwnedHandle;
}

MemoryStream& MemoryStream::Append(const uint8* Input, const size InputSize)
{
    if (FilledSize + InputSize > BufferSize) Resize(FilledSize + InputSize);

    SMemory::Copy(Handle + FilledSize, Input, InputSize);
    FilledSize += InputSize;

    return *this;
}

MemoryStream& MemoryStream::Append(const uint8* Input, const size InputSize, size& OutIndex)
{
    OutIndex = FilledSize;

    return Append(Input, InputSize);
}
