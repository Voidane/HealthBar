using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public static class HealthBarController
    {
        public static void UpdateHealth(float health)
        {
            HealthUICreator.healthSlider.value = health;
            HealthUICreator.healthText.text = $"{health}%";
        }
    }
}
