using UnityEngine;

using SLZ.Combat;
using SLZ.Interaction;
using SLZ.Props.Weapons;
using MelonLoader;

namespace NEP.MagPerception
{
    public static class Hooks
    {
        [HarmonyLib.HarmonyPatch(typeof(Magazine), nameof(Magazine.OnGrab))]
        public static class OnMagAttached
        {
            public static void Postfix(Hand hand, Magazine __instance)
            {
                MelonLogger.Msg("Mag Attached");
                MagPerceptionManager.instance.OnMagazineAttached(__instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnTriggerGripAttached))]
        public static class OnGunAttached
        {
            public static void Postfix(Gun __instance)
            {
                MelonLogger.Msg("Gun Attached");
                MagPerceptionManager.instance.OnGunAttached(__instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnTriggerGripDetached))]
        public static class OnGunDetached
        {
            public static void Postfix(Gun __instance)
            {
                if (__instance == null)
                {
                    return;
                }
                
                MelonLogger.Msg("Gun Detached");
                MagPerceptionManager.instance.OnGunDetached(__instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.EjectCartridge))]
        public static class OnGunEjectRound
        {
            public static void Postfix()
            {
                MelonLogger.Msg("Eject Round");
                MagPerceptionManager.instance.OnGunEjectRound();
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnMagazineInserted))]
        public static class OnMagazineInserted
        {
            public static void Postfix(Gun __instance)
            {
                MelonLogger.Msg("Mag Inserted");
                MagPerceptionManager.instance.OnMagazineInserted(__instance.MagazineState, __instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnMagazineRemoved))]
        public static class OnMagazineRemoved
        {
            public static void Postfix(Gun __instance)
            {
                MelonLogger.Msg("Mag Removed");
                MagPerceptionManager.instance.OnMagazineInserted(__instance.MagazineState, __instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(AmmoPlug), nameof(AmmoPlug.OnPlugInsertComplete))]
        public static class OnRoundInserted
        {
            public static void Postfix(AmmoPlug __instance)
            {
                MelonLogger.Msg($"Round {__instance.name} inserted");
            }
        }
    }
}
