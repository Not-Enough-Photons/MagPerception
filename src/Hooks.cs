using UnityEngine;

using SLZ.Combat;
using SLZ.Interaction;
using SLZ.Props.Weapons;

namespace NEP.MagPerception
{
    public static class Hooks
    {
        public static System.Action<Magazine, Hand> OnHandGrabbedMagazine;
         
        [HarmonyLib.HarmonyPatch(typeof(Magazine))]
        [HarmonyLib.HarmonyPatch(nameof(Magazine.OnGrab))]
        public static class AttachObject
        {
            public static void Postfix(Magazine __instance, Hand hand)
            {
                if(hand.manager.name != "[RigManager (Blank)]")
                {
                    return;
                }

                MagPerceptionManager.instance.OnMagazineAttached(__instance, hand);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnFire))]
        public static class OnGunFire
        {
            public static void Postfix(Gun __instance)
            {
                if(__instance.host.)
                MagPerceptionManager.instance.OnMagazineUpdated(__instance.MagazineState);
            }
        }
    }
}
