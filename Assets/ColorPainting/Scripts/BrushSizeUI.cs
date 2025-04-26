// filepath: /Users/HaruMalik/Desktop/Game-Development/ColorPainting/Assets/ColorPainting/Scripts/BrushSizeUI.cs
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeUI : MonoBehaviour
{
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private Text brushSizeText;

    public int BrushSize { get; private set; }

    private void Start()
    {
        // Initialize the slider and add a listener
        brushSizeSlider.onValueChanged.AddListener(UpdateBrushSize);

        // Set default brush size
        UpdateBrushSize(brushSizeSlider.value);
    }

    private void UpdateBrushSize(float value)
    {
        BrushSize = Mathf.RoundToInt(value);
        brushSizeText.text = $"Brush Size: {BrushSize}";
    }
}