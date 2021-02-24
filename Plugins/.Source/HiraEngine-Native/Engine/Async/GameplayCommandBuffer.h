#pragma once

#include "CoreNativeObject.h"

class GameplayCommandBuffer;

struct SActiveTimer
{
    static constexpr uint8 BufferSize = 15;

    explicit SActiveTimer(const bool InActive = false)
        : Active(InActive)
    {
        for (uint8 I = 0; I < BufferSize; ++I)
        {
            TimeRemaining[I] = 0.0f;
        }
    }

    uint16 Active;
    float TimeRemaining[BufferSize];
};

struct STimerHandle
{
    const uint16 BufferIndex;
    const intptr Owner;
    const uint64 Hash;
};

class GameplayCommandBuffer final : public NativeObject
{
    typedef NativeObject Super;

PROPERTY(TList<SActiveTimer>, ActiveTimers, NONE, NONE)
PROPERTY(TList<uint64>, Hash, NONE, NONE)
PROPERTY(uint64, CurrentHash, NONE, NONE)

public:
    explicit GameplayCommandBuffer(uint16 StartingBufferSize);
    ~GameplayCommandBuffer();

    virtual void OnAwake() override;
    virtual void OnDestroy() override;
    virtual void OnUpdate(float UnscaledDeltaTime, float DeltaTime) override;

    STimerHandle SetTimer(float Time);
    uint8 IsHandleValid(const STimerHandle& InHandle);
    float GetTimeRemaining(const STimerHandle& InHandle);
    void PauseTimer(const STimerHandle& InHandle);
    void ResumeTimer(const STimerHandle& InHandle);
    void CancelTimer(const STimerHandle& InHandle);

DECLARE_IMPORTED_LIBRARY_FUNCTION(void, ExecuteBufferedCommands, GameplayCommandBuffer*, CommandBuffer, uint16*, Indices)

private:
    uint8 TryGetBufferIndex(uint16& OutBufferIndex) const;
};