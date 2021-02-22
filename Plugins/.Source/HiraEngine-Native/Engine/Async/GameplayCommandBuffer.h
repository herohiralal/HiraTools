#pragma once

#include "CoreNativeObject.h"

class GameplayCommandBuffer;

struct SActiveTimer
{
    explicit constexpr SActiveTimer(const bool InActive = false, const float InTimeRemaining = 0, const uint64 InHash = 0)
        : Active(InActive),
          TimeRemaining(InTimeRemaining),
          Hash(InHash)
    {
    }

    uint8 Active;
    float TimeRemaining;
    uint64 Hash;
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
PROPERTY(uint64, Hash, NONE, NONE)
PROPERTY(TList<uint16>, UnusedCommandBufferIndices, NONE, NONE)

public:
    explicit GameplayCommandBuffer(uint16 StartingBufferSize);
    ~GameplayCommandBuffer();

    void OnAwake() override;
    void OnDestroy() override;
    void OnUpdate(float UnscaledDeltaTime, float DeltaTime) override;

DECLARE_IMPORTED_LIBRARY_FUNCTION(void, ExecuteBufferedCommands, uint16*, Indices)
};
