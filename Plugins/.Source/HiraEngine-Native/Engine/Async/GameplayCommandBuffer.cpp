#include "GameplayCommandBuffer.h"

EXPORT_NATIVEOBJECT_CONSTRUCTOR(GameplayCommandBuffer, CreateGameplayCommandBuffer, const uint16, StartingBufferSize)
    : Super(EUpdateType::Update),
      ActiveTimers((StartingBufferSize / SActiveTimer::BufferSize) + 1),
      Hash(((StartingBufferSize / SActiveTimer::BufferSize) + 1) * SActiveTimer::BufferSize),
      CurrentHash(0)
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

    ActiveTimers.SetElementCount(ActiveTimersBufferSize);
    Hash.SetElementCount(TotalBufferSize);

    UNITY_EDITOR_LOG(Log, "Gameplay command buffer has awoken.!")
}

void GameplayCommandBuffer::OnDestroy()
{
    UNITY_EDITOR_LOG(Log, "Gameplay command buffer is dying...")

    CurrentHash = 0;

    ActiveTimers.SetBufferSize(0);
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
        SActiveTimer& It = *(ItStart + I);

        if (It.Active)
        {
            for (uint8 J = 0; J < SActiveTimer::BufferSize; ++J)
            {
                It.TimeRemaining[J] -= DeltaTime * (1 & (It.Active >> J));
            }

            for (uint8 J = 0; J < SActiveTimer::BufferSize; ++J)
            {
                const uint8 CurrentActive = ((It.Active >> J) & 1);
                const uint8 TimerOver = It.TimeRemaining[J] <= 0;
                if (CurrentActive && TimerOver)
                {
                    const uint16 ActualIndex = (I * SActiveTimer::BufferSize) + J;
                    It.Active &= ~(1 << J);
                    Hash[ActualIndex] = 0;
                    TimersFinished.Push(ActualIndex);
                    SuccessCount++;
                }
            }
        }
    }

    TimersFinished[0] = SuccessCount;
    ExecuteBufferedCommands(this, TimersFinished.GetContainer());
}

static_assert(0 % 5 == 0, "0 % <any non-zero number> must be 0.");

IMPLEMENT_EXPORTED_FUNCTION(STimerHandle, GameplayCommandBuffer, SetTimer, const float, Time)
{
    uint16 Index;
    if (!TryGetHash(Index))
    {
        const int32 NewBufferSize = ActiveTimers.ModifyBufferSize(1);
        const int32 NewTotalBufferSize = Hash.ModifyBufferSize(SActiveTimer::BufferSize);

        ActiveTimers.SetElementCount(NewBufferSize);
        Hash.SetElementCount(NewTotalBufferSize);

        TryGetHash(Index);
    }

    const uint64 NewHash = CurrentHash++;

    const uint16 MainIndex = Index / SActiveTimer::BufferSize;
    const uint8 InternalIndex = Index % SActiveTimer::BufferSize;

    SActiveTimer* AssignedTimer = ActiveTimers.GetContainer() + MainIndex;
    AssignedTimer->Active |= (1 << InternalIndex);
    AssignedTimer->TimeRemaining[InternalIndex] = Time;
    *(Hash.GetContainer() + Index) = NewHash;

    return STimerHandle{Index, reinterpret_cast<intptr>(this), NewHash};
}

IMPLEMENT_EXPORTED_FUNCTION(uint8, GameplayCommandBuffer, IsHandleValid, const STimerHandle&, InHandle)
{
    return Hash[InHandle.BufferIndex] == InHandle.Hash;
}

#define ASSERT_HANDLE_VALID(expr) \
    if (!IsHandleValid(InHandle)) \
    { \
        UNITY_LOG(Warning, "Invalid gameplay handle passed."); \
        expr \
    }

IMPLEMENT_EXPORTED_FUNCTION(float, GameplayCommandBuffer, GetTimeRemaining, const STimerHandle&, InHandle)
{
    ASSERT_HANDLE_VALID(return 0.0f;)

    return ActiveTimers[InHandle.BufferIndex / SActiveTimer::BufferSize].TimeRemaining[InHandle.BufferIndex % SActiveTimer::BufferSize];
}

IMPLEMENT_EXPORTED_FUNCTION(void, GameplayCommandBuffer, PauseTimer, const STimerHandle&, InHandle)
{
    ASSERT_HANDLE_VALID(return;)

    ActiveTimers[InHandle.BufferIndex / SActiveTimer::BufferSize].Active &= ~(1 << (InHandle.BufferIndex % SActiveTimer::BufferSize));
}

IMPLEMENT_EXPORTED_FUNCTION(void, GameplayCommandBuffer, ResumeTimer, const STimerHandle&, InHandle)
{
    ASSERT_HANDLE_VALID(return;)

    ActiveTimers[InHandle.BufferIndex / SActiveTimer::BufferSize].Active |= (1 << (InHandle.BufferIndex % SActiveTimer::BufferSize));
}

IMPLEMENT_EXPORTED_FUNCTION(void, GameplayCommandBuffer, CancelTimer, const STimerHandle&, InHandle)
{
    ASSERT_HANDLE_VALID(return;)

    ActiveTimers[InHandle.BufferIndex / SActiveTimer::BufferSize].Active &= ~(1 << (InHandle.BufferIndex % SActiveTimer::BufferSize));
    Hash[InHandle.BufferIndex] = 0;
}

#undef ASSERT_HANDLE_VALID

IMPLEMENT_IMPORTED_LIBRARY_FUNCTION(void, GameplayCommandBuffer, ExecuteBufferedCommands, GameplayCommandBuffer*, CommandBuffer, uint16*, Indices)

uint8 GameplayCommandBuffer::TryGetHash(uint16& OutHash) const
{
    const int32 BufferSize = Hash.GetBufferSize();
    uint64* It = Hash.GetContainer();

    for (uint16 I = 0; I < BufferSize; ++I)
    {
        if (!(*It))
        {
            OutHash = I;
            return true;
        }
    }

    return false;
}