using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;

namespace Tools
{
    public class TimerLabel : Timer
    {
        [Header("Label")]
        public TextMeshProUGUI label;

        protected override void Update()
        {
            base.Update();

            label.text = $"{delay:#0.##}";
        }
    }
}