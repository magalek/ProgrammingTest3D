using System;

namespace Entities {
    [Flags]
    public enum PlayerState {
        Idle = 0,
        Moving = 1 << 0,
        InAir = 1 << 1
    }
}