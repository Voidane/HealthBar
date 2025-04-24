using Il2CppScheduleOne.Persistence.Datas;
using MelonLoader;
using UnityEngine;

namespace HealthDisplay
{
    public class ConfigData
    {
        public static ConfigData Instance;
        public static MelonPreferences_Category positioning;
        public static MelonPreferences_Entry<bool> allowUIDragging;
        public static MelonPreferences_Entry<float> healthBarPositionX;
        public static MelonPreferences_Entry<float> healthBarPositionY;
        public static MelonPreferences_Entry<float> healthBarSizeX;
        public static MelonPreferences_Entry<float> healthBarSizeY;
        
        public static MelonPreferences_Category customization;
        public static MelonPreferences_Entry<string> borderColor;
        public static MelonPreferences_Entry<string> backgroundHealthColor;
        public static MelonPreferences_Entry<string> healthColor;


        public static string folderPath = "UserData/HealthDisplay.cfg";
        public static event Action OnSettingChanged;

        public ConfigData()
        {
            Instance = this;

            positioning = MelonPreferences.CreateCategory("HealthDisplay-Positioning & Size", "Positioning & Anchors");
            allowUIDragging = positioning.CreateEntry("Allow UI Dragging", true, "Allow UI Dragging", "The player can adjust the position of the health bar by clicking and draging the health bar GUI.");
            healthBarPositionX = positioning.CreateEntry("Health Bar Position X", 32.8125F, "Health Bar Position X", "Will position the healthbar by this amount % of the screen by the width");
            healthBarPositionY = positioning.CreateEntry("Health Bar Position Y", 87.2000F, "Health Bar Position Y", "Will position the healthbar by this amount % of the screen by the height");
            healthBarSizeX = positioning.CreateEntry("Health Bar Width", 18.7500F, "Health Bar Width", "The amount of % of the screen size width it should take up");
            healthBarSizeY = positioning.CreateEntry("Health Bar Height", 2.5925F, "Health Bar Height", "The amount of % of the screen size height should take up");

            customization = MelonPreferences.CreateCategory("HealthDisplay-Colors & Styles", "Colors & Styles");
            borderColor = customization.CreateEntry("Border Color", "222222", "Border Color");
            backgroundHealthColor = customization.CreateEntry("Background Health Color", "388408", "Background Health Color");
            healthColor = customization.CreateEntry("Health Color", "388408", "Health Color");

            positioning.SetFilePath(folderPath);
            positioning.SaveToFile();
            customization.SetFilePath(folderPath);
            customization.SaveToFile();
        }

        public static void HandleSettingsUpdate()
        {
            MelonLogger.Msg("Mod Manager saved preferences. Reloading settings...");
            positioning.LoadFromFile();
            customization.LoadFromFile();
            OnSettingChanged?.Invoke();
        }
    }
}