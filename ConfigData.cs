using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
namespace Health
{
    public class ConfigData
    {
        public static ConfigData Instance;
        public static MelonPreferences_Category positioning;
        public static MelonPreferences_Entry<float> anchorMinX;
        public static MelonPreferences_Entry<float> anchorMinY;
        public static MelonPreferences_Entry<float> anchorMaxX;
        public static MelonPreferences_Entry<float> anchorMaxY;
        public static MelonPreferences_Entry<float> pivtotX;
        public static MelonPreferences_Entry<float> pivtotY;
        public static MelonPreferences_Entry<float> sizeDeltaX;
        public static MelonPreferences_Entry<float> sizeDeltaY;
        public static MelonPreferences_Entry<float> anchoredPositionX;
        public static MelonPreferences_Entry<float> anchoredPositionY;

        public static event Action OnSettingChanged;

        public ConfigData()
        {
            Instance = this;

            positioning = MelonPreferences.CreateCategory("HealthDisplay-Positioning & Anchors", "Positioning & Anchors");
            anchorMinX = positioning.CreateEntry("Anchor Min X", 0.235F, "Anchor Min X", 
                "Controls the left edge position of the health display as a percentage of screen width (0 = left edge, 1 = right edge)");
            anchorMinY = positioning.CreateEntry("Anchor Min Y", 0F, "Anchor Min Y", 
                "Controls the bottom edge position of the health display as a percentage of screen height (0 = bottom edge, 1 = top edge)");
            anchorMaxX = positioning.CreateEntry("Anchor Max X", 0.42F, "Anchor Max X", 
                "Controls the right edge position of the health display as a percentage of screen width (0 = left edge, 1 = right edge)");
            anchorMaxY = positioning.CreateEntry("Anchor Max Y", 0F, "Anchor Max Y", 
                "Controls the top edge position of the health display as a percentage of screen height (0 = bottom edge, 1 = top edge)");
            pivtotX = positioning.CreateEntry("Pivot X", 0.5F, "Pivot X", 
                "Sets the horizontal pivot point for rotation and scaling (0 = left, 0.5 = center, 1 = right)");
            pivtotY = positioning.CreateEntry("Pivot Y", 0F, "Pivot Y", 
                "Sets the vertical pivot point for rotation and scaling (0 = bottom, 0.5 = center, 1 = top)");
            sizeDeltaX = positioning.CreateEntry("Delta Size X", 0F, "Delta Size X", 
                "Adjusts the width of the health display in pixels relative to the anchored width");
            sizeDeltaY = positioning.CreateEntry("Delta Size Y", 30F, "Delta Size Y", 
                "Adjusts the height of the health display in pixels (default: 30)");
            anchoredPositionX = positioning.CreateEntry("Anchored Position X", 0F, 
                "Anchored Position X", "Fine-tunes the horizontal position in pixels relative to the anchored position");
            anchoredPositionY = positioning.CreateEntry("Anchored Position Y", 110F, 
                "Anchored Position Y", "Fine-tunes the vertical position in pixels relative to the anchored position (default: 110)");

            positioning.SetFilePath("UserData/HealthDisplay.cfg");
            positioning.SaveToFile();
        }

        public static void HandleSettingsUpdate()
        {
            MelonLogger.Msg("Mod Manager saved preferences. Reloading settings...");
            positioning.LoadFromFile();
            OnSettingChanged?.Invoke();
        }
    }
}