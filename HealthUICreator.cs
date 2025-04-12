using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

namespace Health
{
    public class HealthUICreator
    {
        public static HealthUICreator Instance;

        public static GameObject healthHolder;
        public static Slider healthSlider;

        public static GameObject border;
        public static GameObject fillBackground;
        public static GameObject fill;
        public static GameObject healthPercent;
        public static Text healthText;

        public static bool Initialized
        {
            get => (healthHolder != null);
        }

        public HealthUICreator()
        {
            Instance = this;

            healthHolder = new GameObject("Health Manager");
            healthHolder.transform.SetParent(HealthMod.HUD, false);

            RectTransform healthHolder_RectTransform = healthHolder.AddComponent<RectTransform>();

            if (ConfigData.Instance == null)
            {
                healthHolder_RectTransform.anchorMin = new Vector2(0.235F, 0F);
                healthHolder_RectTransform.anchorMax = new Vector2(0.42F, 0F);
                healthHolder_RectTransform.pivot = new Vector2(0.5F, 0);
                healthHolder_RectTransform.sizeDelta = new Vector2(0, 30);
                healthHolder_RectTransform.anchoredPosition = new Vector2(0, 110);
            }
            else
            {
                healthHolder_RectTransform.anchorMin = new Vector2(ConfigData.anchorMinX.Value, ConfigData.anchorMinY.Value);
                healthHolder_RectTransform.anchorMax = new Vector2(ConfigData.anchorMaxX.Value, ConfigData.anchorMaxY.Value);
                healthHolder_RectTransform.pivot = new Vector2(ConfigData.pivtotX.Value, ConfigData.pivtotY.Value);
                healthHolder_RectTransform.sizeDelta = new Vector2(ConfigData.sizeDeltaX.Value, ConfigData.sizeDeltaY.Value);
                healthHolder_RectTransform.anchoredPosition = new Vector2(ConfigData.anchoredPositionX.Value, ConfigData.anchoredPositionY.Value);
            }
            
            border = new GameObject("Border");
            border.transform.SetParent(healthHolder.transform, false);

            RectTransform border_RectTransform = border.AddComponent<RectTransform>();
            border_RectTransform.anchorMin = new Vector2(0, 0);
            border_RectTransform.anchorMax = new Vector2(1, 1);
            border_RectTransform.offsetMin = Vector2.zero;
            border_RectTransform.offsetMax = Vector2.zero;

            Image border_Image = border.AddComponent<Image>();
            border_Image.color = new Color(34F/255F, 34F/255F, 34F/255F);

            fillBackground = new GameObject("Fill Background");
            fillBackground.transform.SetParent(healthHolder.transform, false);

            RectTransform fillBackground_RectTransform = fillBackground.AddComponent<RectTransform>();
            fillBackground_RectTransform.anchorMin = new Vector2(0F, 0F);
            fillBackground_RectTransform.anchorMax = new Vector2(1F, 1F);
            fillBackground_RectTransform.offsetMin = new Vector2(4F, 4F);
            fillBackground_RectTransform.offsetMax = new Vector2(-4F, -4F);

            Image fillBackground_Image = fillBackground.AddComponent<Image>();
            fillBackground_Image.color = new Color(56f / 255f, 132f / 255f, 8f / 255f, 32F / 255F);

            fill = new GameObject("Fill");
            fill.transform.SetParent(healthHolder.transform, false);

            RectTransform fill_RectTransform = fill.AddComponent<RectTransform>();
            fill_RectTransform.anchorMin = new Vector2(0F, 0F);
            fill_RectTransform.anchorMax = new Vector2(1F, 1F);
            fill_RectTransform.offsetMin = new Vector2(4F, 4F);
            fill_RectTransform.offsetMax = new Vector2(-4F, -4F);

            Image fill_Image = fill.AddComponent<Image>();
            fill_Image.color = new Color(56f / 255f, 132f / 255f, 8f / 255f);

            healthPercent = new GameObject("Health Percent");
            healthPercent.transform.SetParent(healthHolder.transform, false);

            RectTransform healthPercent_rectTransform = healthPercent.AddComponent<RectTransform>();
            healthPercent_rectTransform.anchorMin = new Vector2(0F, 0F);
            healthPercent_rectTransform.anchorMax = new Vector2(1F, 1F);
            healthPercent_rectTransform.offsetMin = new Vector2(4F, 4F);
            healthPercent_rectTransform.offsetMax = new Vector2(-4F, -4F);

            healthText = healthPercent.AddComponent<Text>();
            healthText.text = "";
            healthText.fontSize = 14;
            healthText.alignment = TextAnchor.MiddleCenter;
            healthText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            healthText.color = Color.white;

            healthSlider = healthHolder.AddComponent<Slider>();
            healthSlider.interactable = false;
            healthSlider.transition = Selectable.Transition.None;
            healthSlider.minValue = 0;
            healthSlider.maxValue = 100;
            healthSlider.wholeNumbers = false;
            healthSlider.value = 75;
            healthSlider.direction = Slider.Direction.LeftToRight;
            healthSlider.fillRect = fill_RectTransform;

            healthHolder.transform.SetAsFirstSibling();

            MelonLogger.Msg("Finished creating UI");
        }

        public static void UpdatePositionAndAnchoring()
        {
            if (healthHolder != null && HealthMod.mainSceneLoaded)
            {
                RectTransform healthHolderRectTransform = healthHolder.GetComponent<RectTransform>();

                if (healthHolderRectTransform == null)
                    return;

                MelonLogger.Msg("Dynamically updating health UI position...");

                healthHolderRectTransform.anchorMin = new Vector2(ConfigData.anchorMinX.Value, ConfigData.anchorMinY.Value);
                healthHolderRectTransform.anchorMax = new Vector2(ConfigData.anchorMaxX.Value, ConfigData.anchorMaxY.Value);
                healthHolderRectTransform.pivot = new Vector2(ConfigData.pivtotX.Value, ConfigData.pivtotY.Value);
                healthHolderRectTransform.sizeDelta = new Vector2(ConfigData.sizeDeltaX.Value, ConfigData.sizeDeltaY.Value);
                healthHolderRectTransform.anchoredPosition = new Vector2(ConfigData.anchoredPositionX.Value, ConfigData.anchoredPositionY.Value);
            }
            else
            {
                MelonLogger.Msg("Cannot update health UI position - UI not initialized or scene not loaded");
            }
        }
    }
}
