using System;
using System.Collections;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using UnityEngine;

namespace HealthDisplay
{
    public class HealthUICreator
    {
        private static HealthUICreator instance;
        public static HealthUICreator Instance => instance;
        public static object checkingScreenChanges;
        private bool initialized = false;
        private static bool isScreenSizeChanged = false;
        private GUIStyle healthBarBorderStyle;
        private GUIStyle healthBarBackgroundStyle;
        private GUIStyle healthBarMeterStyle;
        private GUIStyle healthTextStyle;
        private int healthBarBorderPadding = 6;
        
        public Texture2D healthBarBorderTexture;
        private Texture2D healthBarBackgroundTexture;
        private Texture2D healthBarMeterTexture;
        public static float currentPlayerHealth;
        private int PositionX;
        private int _positionY;
        private int _sizeX;
        private int _sizeY;
        private int postScreenWidth;
        private int preScreenWidth;
        private int postScreenHeight;
        private int preScreenHeight;
        private bool isDragging = false;
        private Vector2 dragOffset;
        private Color defaultBorderColor = new Color(0.133f, 0.133f, 0.133f);
        private Color defaultBackgroundColor = new Color(0.2196F, 0.5176f, 0.0313f, 0.125F);
        private Color defaultHealthColor = new Color(0.2196F, 0.5176f, 0.0313f, 1.0F);
        public string BorderColor { get; set; }
        public string HealthBarBackgroundColor { get; set; }
        public string HealthBarColor { get; set; }
        public bool IsDraggingEnabled { get; set; }
        public float HealthBarPositionX { get; set; }
        public float HealthBarPositionY { get; set; } 
        public float HealthBarWidth { get; set; } 
        public float HealthBarHeight { get; set; } 

        public HealthUICreator()
        {
            instance = this;
            InitializeStyles();
            InitializeScreenSizeUpdated(true);
            StartCoroutines();
            ApplyConfigurationSettings();
            initialized = true;
        }

        public void StartCoroutines()
        {
            checkingScreenChanges = MelonCoroutines.Start(CheckForScreenChanges());
        }

        public void StopCoroutines()
        {
            try
            {
                MelonCoroutines.Stop(checkingScreenChanges);
            }
            catch (Exception e)
            {
                MelonLogger.Warning($"Tried stopping IMGUI coroutines but found none!\n{e.StackTrace}");
            }
        }

        public void ApplyConfigurationSettings()
        {
            IsDraggingEnabled = ConfigData.allowUIDragging.Value;
            HealthBarPositionX = ConfigData.healthBarPositionX.Value;
            HealthBarPositionY = ConfigData.healthBarPositionY.Value;
            HealthBarWidth = ConfigData.healthBarSizeX.Value;
            HealthBarHeight = ConfigData.healthBarSizeY.Value;
            BorderColor = ConfigData.borderColor.Value;
            HealthBarBackgroundColor = ConfigData.borderColor.Value;
            HealthBarColor = ConfigData.healthColor.Value;
            
            InitializeScreenSizeUpdated(true);
            ForceUpdateSettings();
        }

        private void InitializeScreenSizeUpdated(bool forceUpdate = false)
        {
            if (forceUpdate || isScreenSizeChanged)
            {
                PositionX = CalculateScreenRatio(HealthBarPositionX, Screen.width) - (CalculateScreenRatio(HealthBarWidth, Screen.width) / 2);
                _positionY = CalculateScreenRatio(HealthBarPositionY, Screen.height) - (CalculateScreenRatio(HealthBarHeight, Screen.height) / 2);
                _sizeX = CalculateScreenRatio(HealthBarWidth, Screen.width);
                _sizeY = CalculateScreenRatio(HealthBarHeight, Screen.height);
                isScreenSizeChanged = false;
            }
        }

        private void ForceUpdateSettings()
        {
            InitializeStyles();
        }

        private void InitializeStyles()
        {
            healthBarBorderStyle = new GUIStyle();
            if (ColorUtility.TryParseHtmlString($"#{ConfigData.borderColor.Value.Replace("#", "").Trim()}", out Color borderColor))
            {
                healthBarBorderTexture = MakeTexture(2, 2, borderColor);
            }
            else
            {
                MelonLogger.Warning($"Config file for {Core._name} -> preference ({nameof(ConfigData.borderColor)}) could not be parsed, invalid hexadecimal for color. Applying default.");
                healthBarBorderTexture = MakeTexture(2, 2, defaultBorderColor);
            }
            healthBarBorderStyle.normal.background = healthBarBorderTexture;

            healthBarBackgroundStyle = new GUIStyle();
            if (ColorUtility.TryParseHtmlString($"#{ConfigData.backgroundHealthColor.Value.Replace("#", "").Trim()}", out Color backgroundHealthColor))
            {
                backgroundHealthColor.a = 0.125F;
                healthBarBackgroundTexture = MakeTexture(2, 2, backgroundHealthColor);
            }
            else
            {
                MelonLogger.Warning($"Config file for {Core._name} -> preference ({nameof(ConfigData.backgroundHealthColor)}) could not be parsed, invalid hexadecimal for color. Applying default.");
                healthBarBackgroundTexture = MakeTexture(2, 2, defaultBackgroundColor);
            }
            healthBarBackgroundStyle.normal.background = healthBarBackgroundTexture;

            healthBarMeterStyle = new GUIStyle();
            if (ColorUtility.TryParseHtmlString($"#{ConfigData.healthColor.Value.Replace("#", "").Trim()}", out Color healthColor))
            {
                healthBarMeterTexture = MakeTexture(2, 2, healthColor);
            }
            else
            {
                MelonLogger.Warning($"Config file for {Core._name} -> preference ({nameof(ConfigData.healthColor)}) could not be parsed, invalid hexadecimal for color. Applying default.");
                healthBarMeterTexture = MakeTexture(2, 2, defaultHealthColor);
            }
            healthBarMeterStyle.normal.background = healthBarMeterTexture;

            healthTextStyle = new GUIStyle();
            healthTextStyle.normal.textColor = Color.white;
            healthTextStyle.alignment = TextAnchor.MiddleCenter;
            healthTextStyle.fontSize = 12;
            healthTextStyle.fontStyle = FontStyle.Bold;
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            
            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        public void OnGUI()
        {
                
            if (!initialized || Player.Local == null || !Core.isMainSceneLoaded) 
            {
                return;
            }
                    
            if (healthBarBorderStyle == null || healthBarBorderStyle.normal.background == null)
            {
                MelonLogger.Msg("Reinitializing styles...");
                InitializeStyles();
            }
            
            InitializeScreenSizeUpdated();

            if (IsDraggingEnabled)
            {
                Rect dragRect = new Rect(PositionX, _positionY, _sizeX, _sizeY);
                Event currentEvent = Event.current;
                
                if (currentEvent.type == EventType.MouseDown && dragRect.Contains(currentEvent.mousePosition))
                {
                    isDragging = true;
                    dragOffset = new Vector2(currentEvent.mousePosition.x - PositionX, currentEvent.mousePosition.y - _positionY);
                }
                else if (currentEvent.type == EventType.MouseUp && dragRect.Contains(currentEvent.mousePosition))
                {
                    MelonLogger.Msg($"Saved position coordinates to {ConfigData.folderPath}::{ConfigData.positioning.DisplayName}");
                    HealthBarController.SaveInGamePositionToConfig();
                    isDragging = false;
                }
                
                if (isDragging && currentEvent.type == EventType.MouseDrag)
                {
                    PositionX = Mathf.Clamp((int)(currentEvent.mousePosition.x - dragOffset.x), 0, Screen.width - _sizeX);
                    _positionY = Mathf.Clamp((int)(currentEvent.mousePosition.y - dragOffset.y), 0, Screen.height - _sizeY);
                    
                    HealthBarPositionX = (PositionX + (_sizeX / 2)) * 100f / Screen.width;
                    HealthBarPositionY = (_positionY + (_sizeY / 2)) * 100f / Screen.height;
                }
            }

            GUI.Box(new Rect(PositionX, _positionY, _sizeX, _sizeY), "", healthBarBorderStyle);

            int healthPositionX = PositionX + (healthBarBorderPadding/2);
            int healthPositionY = _positionY + (healthBarBorderPadding/2);
            int healthSizeX = _sizeX - healthBarBorderPadding;
            int healthSizeY = _sizeY - healthBarBorderPadding;
            GUI.Box(new Rect(healthPositionX, healthPositionY, healthSizeX, healthSizeY), "", healthBarBackgroundStyle);

            int healthFillWidth = Mathf.RoundToInt(healthSizeX * (currentPlayerHealth / 100F));
            GUI.Box(new Rect(healthPositionX, healthPositionY, healthFillWidth, healthSizeY), "", healthBarMeterStyle);

            string healthText = Mathf.RoundToInt(currentPlayerHealth) + "%";
            GUI.Label(new Rect(PositionX, _positionY, _sizeX, _sizeY), healthText, healthTextStyle);

        }   

        public static int CalculateScreenRatio(float percentage, int screenSize)
        {
            return Mathf.RoundToInt(Mathf.Ceil((screenSize * percentage) / 100F)); 
        }

        public bool IsInitialized()
        {
            return initialized;
        }

        public IEnumerator CheckForScreenChanges()
        {
            while (true)
            {
                preScreenWidth = Screen.width;
                preScreenHeight = Screen.height;

                if (preScreenWidth != postScreenWidth || preScreenHeight != postScreenHeight)
                {
                    postScreenHeight = preScreenHeight;
                    postScreenWidth = preScreenWidth;
                    isScreenSizeChanged = true;
                }
                else
                {
                    isScreenSizeChanged = false;
                }
                yield return new WaitForSeconds(0.25F);
            }
        }
    }
}