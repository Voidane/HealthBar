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
        public static GameObject border;
        public static GameObject fill;

        public HealthUICreator()
        {
            Instance = this;

            healthHolder = new GameObject("Health Manager");
            healthHolder.transform.SetParent(HealthMod.HUD, false);

            RectTransform healthHolder_RectTransform = healthHolder.AddComponent<RectTransform>();
            healthHolder_RectTransform.anchorMin = new Vector2(0.35F, 0.11F); // Y Higher value lifts bottom
            healthHolder_RectTransform.anchorMax = new Vector2(0.65F, 0.05F); // Y Lower value shortens bar vert
            healthHolder_RectTransform.pivot = new Vector2(0.5F, 0);

            border = new GameObject("Border");
            border.transform.SetParent(healthHolder.transform, false);

            RectTransform border_RectTransform = border.AddComponent<RectTransform>();
            border_RectTransform.anchorMin = new Vector2(0, 0);
            border_RectTransform.anchorMax = new Vector2(1, 1);
            border_RectTransform.offsetMin = Vector2.zero;
            border_RectTransform.offsetMax = Vector2.zero;

            Image border_Image = border.AddComponent<Image>();
            border_Image.color = new Color(34F/255F, 34F/255F, 34F/255F);

            fill = new GameObject("Fill");
            fill.transform.SetParent(healthHolder.transform, false);

            RectTransform fill_RectTransform = fill.AddComponent<RectTransform>();
            fill_RectTransform.anchorMin = new Vector2(0F, 0F);
            fill_RectTransform.anchorMax = new Vector2(1F, 1F);
            fill_RectTransform.offsetMin = new Vector2(2, 2);
            fill_RectTransform.offsetMax = new Vector2(-2, -2);

            Image fill_Image = fill.AddComponent<Image>();
            fill_Image.color = new Color(56f / 255f, 132f / 255f, 8f / 255f);

            MelonLogger.Msg("Finished creating UI");
        }
    }
}
