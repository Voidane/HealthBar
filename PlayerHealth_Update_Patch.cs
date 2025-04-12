using HarmonyLib;
using Il2CppScheduleOne.PlayerScripts.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace Health
{
    [HarmonyPatch(typeof(PlayerHealth), "Update")]
    public static class PlayerHealth_Update_Patch
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
