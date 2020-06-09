﻿namespace OSU
{
    public interface IElement
    { 
        int PosX { get;} 
        int PosY { get; } 
        int DelayTime { get; } 
        int DeathTime { get; }
        void Show();
        void Remove();
    }
}