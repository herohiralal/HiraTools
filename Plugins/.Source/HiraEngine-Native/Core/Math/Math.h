#pragma once

struct SMath
{
    template <typename T>
    static T Min(T A, T B);

    template <typename T>
    static T Max(T A, T B);
};

template <typename T>
T SMath::Min(T A, T B)
{
    return A < B ? A : B;
}

template <typename T>
T SMath::Max(T A, T B)
{
    return A > B ? A : B;
}
