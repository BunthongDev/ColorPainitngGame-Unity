# Color Painting Game

A Unity-based game where users can paint on sprites using a customizable brush. The game features a color picker and brush size slider for an intuitive and interactive painting experience.

---

## Features

- **Color Picker UI**: Allows users to select any color for painting.
- **Brush Size Slider**: Enables users to resize the brush dynamically.
- **Smooth Painting**: Paint sprites with smooth or pixelated brushes.
- **Layer Detection**: Automatically detects and paints the topmost sprite under the cursor.

---

## How to Use

1. **Select a Color**:
   - Use the color picker UI to choose a color for the brush.

2. **Adjust Brush Size**:
   - Use the brush size slider to resize the brush.

3. **Start Painting**:
   - Left-click on a sprite to start painting.
   - Drag the mouse to paint continuously.

---

## Installation

1. Clone this repository:
   ```bash
   https://github.com/BunthongDev/ColorPainitngGame-Unity


Scripts Overview
SpriteBrush.cs
Handles the painting logic, including:

Detecting sprites under the cursor.
Applying the selected color and brush size to the sprite.
ColorPickerUI.cs
Manages the color picker UI, allowing users to select a color.

BrushSizeUI.cs
Handles the brush size slider, enabling users to adjust the brush size dynamically.

How to Add New Features
Adding New Brush Shapes
Modify the ColorSpriteAtPosition method in SpriteBrush.cs.
Implement your custom brush logic (e.g., circular, square, or custom patterns).
Enhancing the UI
Add new UI elements in Unity's Canvas.
Link them to the appropriate scripts for functionality.
Screenshots
Color Picker UI Brush Size Slider

Contributing
Contributions are welcome! Feel free to submit a pull request or open an issue for suggestions and bug reports.

License
This project is licensed under the MIT License.

Contact
For questions or feedback, contact:

Name: Bunthong Rorn
Email: github.com/BunthongDev
