using UnityEngine;

using SLZ.Rig;

using BoneLib;

namespace NEP.MagPerception.Utilities
{
    public static class Utilities
    {
        public static BaseController GetLeftController()
        {
            return Player.leftController;
        }

        public static BaseController GetRightController()
        {
            return Player.rightController;
        }
    }
}