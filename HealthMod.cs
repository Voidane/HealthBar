using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppScheduleOne.PlayerScripts.Health;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthMod : MelonMod
    {
        public static Transform UI;
        public static Transform HUD;
        public static bool mainSceneLoaded;

        public static GameObject healthHolder;
        public HealthBarController healthController;
        public static HealthUICreator healthUI;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initializing mod!");
            HarmonyPatches();
        }

        private void HarmonyPatches()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.voidane.healthmod");

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                mainSceneLoaded = true;
                MelonCoroutines.Start(WaitOnSceneLoad(null, "UI", 20.0F, (_UI) =>
                {
                    UI = _UI;
                    MelonCoroutines.Start(WaitOnSceneLoad(UI, "HUD", 5.0F, (_HUD) =>
                    {
                        HUD = _HUD;
                        if (HealthUICreator.Instance == null)
                        {
                            MelonLogger.Msg("Creating new Health bar");
                            new HealthUICreator();
                        }
                    }));
                }));
            }
        }

        private IEnumerator WaitOnSceneLoad(Transform parent, string name, float timeoutLimit, Action<Transform> onComplete)
        {
            Transform target = null;
            float timeOutCounter = 0F;
            int attempt = 0;

            while (target == null && timeOutCounter < timeoutLimit)
            {
                target = (parent == null) ? GameObject.Find(name).transform : parent.Find(name);
                if (target == null)
                {
                    timeOutCounter += 0.5F;
                    yield return new WaitForSeconds(0.5F);
                }
            }

            if (target != null)
            {
                onComplete?.Invoke(target);
            }
            else
            {
                MelonLogger.Error("Failed to find target object within timeout period!");
                onComplete?.Invoke(null);
            }

            yield return target;
        }
    }
}