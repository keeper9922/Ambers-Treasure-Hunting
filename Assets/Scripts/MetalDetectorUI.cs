using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class MetalDetectorUI : MonoBehaviour
    {
        [SerializeField] private MetalDetector detector;
        [SerializeField] private Slider signalSlider;
        [SerializeField] private TMP_Text statusText;

        private void OnEnable()
        {
            if (detector == null)
                return;

            detector.OnSignalChanged += HandleSignalChanged;
            detector.OnTreasureDetected += HandleTreasureDetected;
            detector.OnModeChanged += HandleModeChanged;
        }

        private void OnDisable()
        {
            if (detector == null)
                return;

            detector.OnSignalChanged -= HandleSignalChanged;
            detector.OnTreasureDetected -= HandleTreasureDetected;
            detector.OnModeChanged -= HandleModeChanged;
        }

        private void HandleSignalChanged(float signal)
        {
            signalSlider.value = signal;
        }

        private void HandleTreasureDetected(GameObject treasure)
        {
            statusText.text = $"Обнаружен объект: {treasure.name}";
        }

        private void HandleModeChanged(bool active)
        {
            statusText.text = active
                ? "Металлоискатель включён"
                : "Металлоискатель выключен";
        }
        
    }
}