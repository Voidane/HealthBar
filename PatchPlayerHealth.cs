using HarmonyLib;
using Il2CppScheduleOne.PlayerScripts.Health;

namespace HealthDisplay
{
    [HarmonyPatch(typeof(PlayerHealth), nameof(PlayerHealth.Update))]
    public static class PatchPlayerHealth
    {
        private static float preHealth;
        private static float postHealth;

        [HarmonyPostfix]
        public static void Postfix(PlayerHealth __instance)
        {
            postHealth = __instance.CurrentHealth;
            if (preHealth != postHealth)
            {
                preHealth = postHealth;
                HealthBarController.UpdateHealth(postHealth);
            }
        }
    }
}