using UnityEngine;

using SLZ.Combat;
using SLZ.Interaction;
using SLZ.Props.Weapons;
using MelonLoader;

namespace NEP.MagPerception
{
    public static class Hooks
    {
        public static System.Action<Magazine, Hand> OnHandGrabbedMagazine;
         
        [HarmonyLib.HarmonyPatch(typeof(Magazine))]
        [HarmonyLib.HarmonyPatch(nameof(Magazine.OnGrab))]
        public static class AttachMagObject
        {
            public static void Postfix(Magazine __instance, Hand hand)
            {
                if(hand.manager.name != "[RigManager (Blank)]")
                {
                    return;
                }

                MagPerceptionManager.instance.OnMagazineAttached(__instance, hand.handedness);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Magazine))]
        [HarmonyLib.HarmonyPatch(nameof(Magazine.OnEject))]
        public static class DetachMagObject
        {
            public static void Postfix()
            {
                MagPerceptionManager.instance.OnMagazineDetached();
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun))]
        [HarmonyLib.HarmonyPatch(nameof(Gun.OnTriggerGripAttached))]
        public static class AttachGunObject
        {
            public static void Postfix(Hand hand, Gun __instance)
            {
                if(hand.manager.name != "[RigManager (Blank)]")
                {
                    return;
                }

                MagPerceptionManager.instance.OnGunAttached(__instance, hand);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun))]
        [HarmonyLib.HarmonyPatch(nameof(Gun.OnTriggerGripDetached))]
        public static class DetachGunObject
        {
            public static void Postfix(Hand hand, Gun __instance)
            {
                if(hand.manager.name != "[RigManager (Blank)]")
                {
                    return;
                }

                MagPerceptionManager.instance.OnGunDetached(__instance, hand);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.EjectCartridge))]
        public static class OnEjectCartridge
        {
            public static void Postfix(Gun __instance)
            {
                MagPerceptionManager.instance.OnEjectCartridge(__instance);
            }
        }
    }
}
