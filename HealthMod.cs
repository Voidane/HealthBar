using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Il2CppScheduleOne.PlayerScripts.Health;
using MelonLoader;
using MelonLoader.Utils;
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
        public static HealthUICreator healthUI;
        private const string versionCurrent = "1.0.0";
        private const string versionMostUpToDateURL = "";
        private const string urldownload = "";
        private string versionUpdate = null;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg($"===========================================");
            MelonLogger.Msg("Initializing mod!");

            new HarmonyLib.Harmony("com.voidane.healthbar").PatchAll();
            new ConfigData();

            try
            {
                ConfigData.OnSettingChanged += HealthUICreator.UpdatePositionAndAnchoring;
                ModManagerPhoneApp.ModSettingsEvents.OnPreferencesSaved += ConfigData.HandleSettingsUpdate;
            }
            catch (Exception e)
            {
                MelonLogger.Warning($"Could not subscribe to Mod Manager event (Mod Manager may not be installed " +
                    $"(https://www.nexusmods.com/schedule1/mods/397)):\n{e.Message}");
            }
            CheckForUpdates();
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
                        if (!HealthUICreator.Initialized)
                        {
                            MelonLogger.Msg("Creating new Health bar");
                            new HealthUICreator();
                        }
                    }));
                }));
            }
            else
            {
                mainSceneLoaded = false;
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

        private async void CheckForUpdates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string content = await client.GetStringAsync(versionMostUpToDateURL);
                    versionUpdate = content.Trim();
                }
            }
            catch (Exception e)
            {
                MelonLogger.Msg($"Could not fetch most up to date version {e.Message}");
            }

            if (versionCurrent != versionUpdate)
            {
                MelonLogger.Msg($"New Update for health mod! {urldownload} Current: {versionCurrent}, Update: {versionUpdate}");
            }

            MelonLogger.Msg($"Has been initialized...");
            MelonLogger.Msg($"===========================================");
        }
    }
}