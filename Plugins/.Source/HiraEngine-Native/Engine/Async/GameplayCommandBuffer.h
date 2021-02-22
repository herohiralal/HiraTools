#pragma once

#include "CoreNativeObject.h"

class GameplayCommandBuffer;

struct SActiveTimer
{
    static constexpr uint8 BufferSize = 15;
    explicit constexpr SActiveTimer(const bool InActive = false, const float InTimeRemaining = 0)
        : Active(InActive),
          TimeRemaining(InTimeRemaining)
    {
    }

    uint16 Active;
    float TimeRemaining[BufferSize];
};

struct STimerHandle
{
    STimerHandle(GameplayCommandBuffer* const InOwner, const uint16 InBufferIndex, const uint64 InHash)
        : Owner(InOwner),
          BufferIndex(InBufferIndex),
          Hash(InHash)
    {
    }

    GameplayCommandBuffer* const Owner;
    const uint16 BufferIndex;
    const uint64 Hash;
};

class GameplayCommandBuffer final : public NativeObject
{
    typedef NativeObject Super;

PROPERTY(TList<SActiveTimer>, ActiveTimers, NONE, NONE)
PROPERTY(TList<uint64>, Hash, NONE, NONE)
PROPERTY(uint64, CurrentHash, NONE, NONE)
PROPERTY(TList<uint16>, UnusedCommandBufferIndices, NONE, NONE)

public:
    explicit GameplayCommandBuffer(uint16 StartingBufferSize);
    ~GameplayCommandBuffer();

    virtual void OnAwake() override;
    virtual void OnDestroy() override;
    virtual void OnUpdate(float UnscaledDeltaTime, float DeltaTime) override;

    STimerHandle SetTimer(float Time);

DECLARE_IMPORTED_LIBRARY_FUNCTION(void, ExecuteBufferedCommands, uint16*, Indices)
};
