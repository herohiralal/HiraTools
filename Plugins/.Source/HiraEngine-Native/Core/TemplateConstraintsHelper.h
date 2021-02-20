#pragma once

template <class TTarget, class TBase>
struct AssignableTo
{
    AssignableTo()
    {
        void (*_)(TTarget*) = Constraints;
    }

    static void Constraints(TTarget* TargetPointer)
    {
        TBase* _ = TargetPointer;
    }
};
