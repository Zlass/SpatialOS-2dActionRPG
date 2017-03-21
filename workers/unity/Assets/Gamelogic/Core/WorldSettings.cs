using Improbable;
using Improbable.Math;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
    public static class WorldSettings
    {
        //  Entity Prefab Names
        public static string PlayerManagerEntityName = "PlayerManagerEntity";

        //  Entity Prefab Paths

        // Worker Connection
        public const int TargetFramerateFSim = 60;
        public static int TargetFramerate = 120;
        public static int FixedFramerate = 10;
    }
}