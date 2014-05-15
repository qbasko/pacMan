using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    public enum RotationEnum
    {
        North = 270,
        East = 0,
        West=180,
        South=90
    }

    public enum GameStateEnum
    {
        End,
        Game,
        Intro
    }

    public enum MapTileType
    {
        MapEmpty,
        MapBarrier,
        Dot,
        SuperDot,
        TunnelLeft,
        TunnelRight,
        MapStart,
        MapExit
    }

}
