namespace HealthDisplay
{
    public static class HealthBarController
    {
        public static void UpdateHealth(float health)
        {
            HealthUICreator.currentPlayerHealth = health;
        }

        public static void UpdateConfigSettings()
        {
            HealthUICreator health = HealthUICreator.Instance;
            if (health != null && Core.isMainSceneLoaded)
            {
                health.ApplyConfigurationSettings();
            }
        }

        public static void SaveInGamePositionToConfig()
        {
            HealthUICreator health = HealthUICreator.Instance;
            ConfigData.healthBarPositionX.Value = health.HealthBarPositionX;
            ConfigData.healthBarPositionY.Value = health.HealthBarPositionY;
            ConfigData.customization.SaveToFile();
            ConfigData.positioning.SaveToFile();
        }
    }
}