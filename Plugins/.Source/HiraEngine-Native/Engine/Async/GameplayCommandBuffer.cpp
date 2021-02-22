#include "GameplayCommandBuffer.h"

EXPORT_NATIVEOBJECT_CONSTRUCTOR(GameplayCommandBuffer, CreateGameplayCommandBuffer, const uint16, StartingBufferSize)
    : Super(EUpdateType::Update),
      ActiveTimers((StartingBufferSize / SActiveTimer::BufferSize) + 1),
      Hash(((StartingBufferSize / SActiveTimer::BufferSize) + 1) * SActiveTimer::BufferSize),
      CurrentHash(0),
      UnusedCommandBufferIndices(((StartingBufferSize / SActiveTimer::BufferSize) + 1) * SActiveTimer::BufferSize)
{
}

EXPORT_NATIVEOBJECT_DESTRUCTOR(GameplayCommandBuffer)
{
}

void GameplayCommandBuffer::OnAwake()
{
    CurrentHash = 1;

    const int32 ActiveTimersBufferSize = ActiveTimers.GetBufferSize();
    const int32 TotalBufferSize = ActiveTimersBufferSize * SActiveTimer::BufferSize;

    uint16* CurrentIndex = UnusedCommandBufferIndices.GetContainer();
    for (uint16 I = 0; I < TotalBufferSize; ++I, ++CurrentIndex)
    {
        *CurrentIndex = I;
    }

    ActiveTimers.SetElementCount(ActiveTimersBufferSize);
    UnusedCommandBufferIndices.SetElementCount(TotalBufferSize);
    Hash.SetElementCount(TotalBufferSize);
}

void GameplayCommandBuffer::OnDestroy()
{
    CurrentHash = 0;

    ActiveTimers.SetBufferSize(0);
    UnusedCommandBufferIndices.SetBufferSize(0);
    Hash.SetBufferSize(0);
}

void GameplayCommandBuffer::OnUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{
    TList<uint16> TimersFinished(5);
    TimersFinished.Push(0);

    uint16 SuccessCount = 0;

    SActiveTimer* const ItStart = ActiveTimers.GetContainer();
    const int32 Count = ActiveTimers.GetBufferSize();
    for (uint16 I = 0; I < Count; ++I)
    {
        SActiveTimer* const It = ItStart + I;

        if (It->Active)
        {
            for (uint8 J = 0; J < SActiveTimer::BufferSize; ++J)
            {
                It->TimeRemaining[J] -= DeltaTime * ((It->Active >> J) & 1);
            }

            for (uint8 J = 0; J < SActiveTimer::BufferSize; ++J)
            {
                if (((It->Active >> J) & 1) && It->TimeRemaining[J] <= 0)
                {
                    const uint16 ActualIndex = (I * SActiveTimer::BufferSize) + J;
                    It->Active &= ~(1 << J);
                    UnusedCommandBufferIndices.Push(ActualIndex);
                    TimersFinished.Push(ActualIndex);
                    SuccessCount++;
                }
            }
        }
    }

    TimersFinished[0] = SuccessCount;
    ExecuteBufferedCommands(TimersFinished.GetContainer());
}

STimerHandle GameplayCommandBuffer::SetTimer(const float Time)
{
    if (!UnusedCommandBufferIndices.GetElementCount())
    {
        const int32 NewBufferSize = ActiveTimers.ModifyBufferSize(1);
        UnusedCommandBufferIndices.ModifyBufferSize(SActiveTimer::BufferSize);
        const int32 NewTotalBufferSize = Hash.ModifyBufferSize(SActiveTimer::BufferSize);

        uint16* const ItStart = UnusedCommandBufferIndices.GetContainer();
        for (uint16 I = 0; I < SActiveTimer::BufferSize; ++I)
        {
            *(ItStart + I) = NewBufferSize + I;
        }
        UnusedCommandBufferIndices.SetElementCount(SActiveTimer::BufferSize);
        Hash.SetElementCount(NewTotalBufferSize);
    }

    const uint16 Index = UnusedCommandBufferIndices.Pop();
    const uint64 NewHash = CurrentHash++;

    const uint16 MainIndex = Index / SActiveTimer::BufferSize;    
    const uint8 InternalIndex = Index % SActiveTimer::BufferSize;

    SActiveTimer* AssignedTimer = ActiveTimers.GetContainer() + MainIndex;
    AssignedTimer->Active |= ~(1 << InternalIndex);
    AssignedTimer->TimeRemaining[InternalIndex] = Time;
    *(Hash.GetContainer()+Index) = NewHash;

    return STimerHandle(this, Index, NewHash);
}

IMPLEMENT_IMPORTED_LIBRARY_FUNCTION(void, GameplayCommandBuffer, ExecuteBufferedCommands, uint16*, Indices)
