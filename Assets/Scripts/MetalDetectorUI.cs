using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MetalDetectorUI : MonoBehaviour
{
    [SerializeField] private MetalDetector detector;
    [SerializeField] private Slider signalSlider;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text distanceText;

    private void OnEnable()
    {
        if (detector == null)
            return;

        // detector.OnSignalChanged += HandleSignalChanged;
        // detector.OnTreasureDetected += HandleTreasureDetected;
        // detector.OnModeChanged += HandleModeChanged;
        MetalDetectorEvents.OnSignalChanged += HandleSignalChanged;
        MetalDetectorEvents.ModeChanged += HandleModeChanged;
        MetalDetectorEvents.DetectionChanged += HandleTreasureDetected;

    }

    private void OnDisable()
    {
        if (detector == null)
            return;
        
        MetalDetectorEvents.OnSignalChanged -= HandleSignalChanged;
        MetalDetectorEvents.ModeChanged -= HandleModeChanged;
        MetalDetectorEvents.DetectionChanged -= HandleTreasureDetected;
        // detector.OnSignalChanged -= HandleSignalChanged;
        // detector.OnTreasureDetected -= HandleTreasureDetected;
        // detector.OnModeChanged -= HandleModeChanged;
    }

    private void HandleSignalChanged(float signal)
    {
        signalSlider.value = signal;
    }

    private void HandleTreasureDetected(ScanResult? result)
    {
        if(result == null) return;
        if (!result.Value.HasTarget) return;
        statusText.text = $"Обнаружен: {result.Value.Target.name}";
    }

    private void HandleModeChanged(ScanMode mode)
    {
        switch (mode)
        {
            case ScanMode.Disabled:
                statusText.text = "Металлоискатель выключен";
                break;
            case ScanMode.Standard:
                statusText.text = "Режим: Стандарт (Всё)";
                break;
            case ScanMode.Gold:
                statusText.text = "Режим: Цветные металлы";
                break;
            case ScanMode.Precise:
                statusText.text = "Режим: Повышенная точность";
                break;
            default:
                statusText.text = "Ошибка режима";
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
        // statusText.text = active
        //     ? "Металлоискатель включён"
        //     : "Металлоискатель выключен";
    }
        
}