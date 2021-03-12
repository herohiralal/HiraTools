#include "Memory.h"

void SMemory::Copy(uint8* Destination, const uint8* Source, const size Size)
{
    for (size It = 0; It < Size; ++It)
    {
        Destination[It] = Source[It];
    }
}
