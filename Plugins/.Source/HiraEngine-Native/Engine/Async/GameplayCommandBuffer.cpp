#include "GameplayCommandBuffer.h"

EXPORT_NATIVEOBJECT_CONSTRUCTOR(GameplayCommandBuffer, CreateGameplayCommandBuffer, const uint16, StartingBufferSize)
    : Super(EUpdateType::Update),
      ActiveTimers(StartingBufferSize),
      Hash(1),
      UnusedCommandBufferIndices(StartingBufferSize)
{
}

EXPORT_NATIVEOBJECT_DESTRUCTOR(GameplayCommandBuffer)
{
}

static constexpr SActiveTimer GEmptyTimer = SActiveTimer();

void GameplayCommandBuffer::OnAwake()
{
    Hash = 1;

    const int32 BufferSize = ActiveTimers.GetBufferSize();

    uint16* CurrentIndex = UnusedCommandBufferIndices.GetContainer();
    for (uint16 I = 0; I < BufferSize; I++)
    {
        *(CurrentIndex + I) = I;
    }

    SActiveTimer* Iterator = ActiveTimers.GetContainer();
    for (uint16 I = 0; I < BufferSize; I++)
    {
        *(Iterator + I) = GEmptyTimer;
    }
}

void GameplayCommandBuffer::OnDestroy()
{
    ActiveTimers.SetBufferSize(0);
    UnusedCommandBufferIndices.SetBufferSize(0);
}

void GameplayCommandBuffer::OnUpdate(const float UnscaledDeltaTime, const float DeltaTime)
{

    TList<uint16> TimersFinished(5);
    TimersFinished.Add(0);

    uint16 SuccessCount = 0;

    SActiveTimer* const ItStart = ActiveTimers.GetContainer();
    const int32 Count = ActiveTimers.GetBufferSize();
    for (uint16 I = 0; I < Count; I++)
    {
        SActiveTimer* const It = ItStart + I;
        
        It->TimeRemaining -= DeltaTime * It->Active;
        if(It->Active && It->TimeRemaining <= 0)
        {
            It->Active = false;
            It->Hash = 0;
            UnusedCommandBufferIndices.Add(I);
            TimersFinished.Add(I);
            SuccessCount++;
        }
    }

    TimersFinished[0] = SuccessCount;
    ExecuteBufferedCommands(TimersFinished.GetContainer());
}

IMPLEMENT_IMPORTED_LIBRARY_FUNCTION(void, GameplayCommandBuffer, ExecuteBufferedCommands, uint16*, Indices)
