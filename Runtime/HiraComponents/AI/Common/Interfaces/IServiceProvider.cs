﻿namespace HiraEngine.Components.AI
{
    public interface IServiceProvider
    {
        IService GetService(UnityEngine.GameObject target);
    }
}