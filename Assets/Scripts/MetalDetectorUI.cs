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
        statusText.text = $"Обнаружен: {treasure.name}";
    }

    private void HandleModeChanged(bool active)
    {
        statusText.text = active
            ? "Металлоискатель включён"
            : "Металлоискатель выключен";
    }
        
}