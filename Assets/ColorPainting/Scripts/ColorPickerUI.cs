// filepath: /Users/HaruMalik/Desktop/Game-Development/ColorPainting/Assets/ColorPainting/Scripts/ColorPickerUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [SerializeField] private Slider redSlider;
    [SerializeField] private Slider greenSlider;
    [SerializeField] private Slider blueSlider;
    [SerializeField] private Image colorPreview;

    public Color SelectedColor { get; private set; }

    private void Start()
    {
        // Initialize sliders and add listeners
        redSlider.onValueChanged.AddListener(UpdateColor);
        greenSlider.onValueChanged.AddListener(UpdateColor);
        blueSlider.onValueChanged.AddListener(UpdateColor);

        // Set default color
        UpdateColor(0);
    }

    private void UpdateColor(float value)
    {
        float r = redSlider.value;
        float g = greenSlider.value;
        float b = blueSlider.value;

        SelectedColor = new Color(r, g, b, 1f);
        colorPreview.color = SelectedColor;
    }
}