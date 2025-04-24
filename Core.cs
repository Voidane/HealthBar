using System;
using System.Collections;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(HealthDisplay.Core), "HealthDisplay", "1.1.0", "Voidane", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace HealthDisplay
{
    public class Core : MelonMod
    {
        public static string _name = "Health Display";
        public static Transform UI;
        public static Transform HUD;
        public static GameObject healthHolder;
        public static HealthUICreator healthUI;
        public static bool isPlayerInitialized;
        public static bool isMainSceneLoaded;
        private const string versionCurrent = "1.1.0";
        public static string discord = "discord.gg/XB7ruKtJje";

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg($"Initializing Health Display Mod, Version: {versionCurrent}");
            MelonLogger.Msg($"Join our community! {discord}");
            new HarmonyLib.Harmony("com.voidane.healthbar").PatchAll();
            new ConfigData();
            TryLoadingDependencies();
        }

        public override void OnApplicationQuit()
        {
        }

        public override void OnGUI()
        {
            if (isPlayerInitialized && healthUI != null && healthUI.IsInitialized())
            {
                healthUI.OnGUI();
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "Main")
            {
                isMainSceneLoaded = true;
                MelonCoroutines.Start(IsPlayerInitialized());
            }
            else
            {
                isMainSceneLoaded = false;
                isPlayerInitialized = false;
                
                if (HealthUICreator.Instance != null)
                {
                    healthUI.StopCoroutines();
                }
            }
        }

        private IEnumerator IsPlayerInitialized()
        {
            while (Player.Local == null)
            {
                yield return new WaitForSeconds(0.5F);
            }

            if (HealthUICreator.Instance == null)
            {
                healthUI = new HealthUICreator();
            }
            else
            {
                healthUI.StartCoroutines();
            }   

            isPlayerInitialized = true;
        }

        private static void TryLoadingDependencies()
        {
            try
            {
                ConfigData.OnSettingChanged += HealthBarController.UpdateConfigSettings;
                ModManagerPhoneApp.ModSettingsEvents.OnPreferencesSaved += ConfigData.HandleSettingsUpdate;
            }
            catch (Exception e)
            {
                MelonLogger.Warning($"Dependency ModManagerPhoneApp (optional) could not be loaded. You can ignore this if you choose to not use it." +
                                   "\n1. Is ModManager&PhoneApp Installed?" + 
                                   "\n2. Is ModManager&PhoneApp Up to date?");
            }
        }
    }
}