using Il2CppSLZ.Marrow;

namespace NEP.MagPerception
{
    public static class Hooks
    {
        [HarmonyLib.HarmonyPatch(typeof(Magazine), nameof(Magazine.OnGrab))]
        public static class OnMagAttached
        {
            public static void Postfix(Hand hand, Magazine __instance)
            {
                MagPerceptionManager.instance.OnMagazineAttached(__instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnTriggerGripAttached))]
        public static class OnGunAttached
        {
            public static void Postfix(Gun __instance)
            {
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
                
                MagPerceptionManager.instance.OnGunDetached(__instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.EjectCartridge))]
        public static class OnGunEjectRound
        {
            public static void Postfix()
            {
                MagPerceptionManager.instance.OnGunEjectRound();
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnMagazineInserted))]
        public static class OnMagazineInserted
        {
            public static void Postfix(Gun __instance)
            {
                MagPerceptionManager.instance.OnMagazineInserted(__instance.MagazineState, __instance);
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Gun), nameof(Gun.OnMagazineRemoved))]
        public static class OnMagazineRemoved
        {
            public static void Postfix(Gun __instance)
            {
                MagPerceptionManager.instance.OnMagazineInserted(__instance.MagazineState, __instance);
            }
        }
    }
}
