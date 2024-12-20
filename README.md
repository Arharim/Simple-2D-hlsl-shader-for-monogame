
# 2D Lighting System with Normal Mapping

This project demonstrates a 2D lighting system implemented in C# using the **MonoGame** framework. 
It showcases the use of **normal mapping** and custom shaders to create dynamic and realistic lighting effects in a 2D game environment. 

## Features

- **Dynamic Lighting**: Multiple light sources with adjustable position, radius, color, and opacity.
- **Normal Mapping**: Adds realistic depth and lighting effects to flat 2D textures using normal maps.
- **Custom Shaders**: Utilizes HLSL shaders for lighting calculations.
- **Interactive Controls**: 
  - Use the mouse and keyboard to move lights, adjust their properties, or switch between active light sources.
  - Fine-tune light properties (radius, opacity, etc.) in real time.
- **Modular Design**: 
  - Light management, rendering, and input handling are encapsulated in separate classes, promoting clean and maintainable code.

## Controls

- **TAB**: Cycle through the available light sources.
- **Mouse Left Click**: Move the selected light source to the mouse position.
- **W / S**: Move the selected light source along the Z-axis (depth).
- **Arrow Keys**: Adjust the radius and opacity of the selected light source.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Arharim/Simple-2D-hlsl-shader-for-monogame.git
   cd Simple-2D-hlsl-shader-for-monogame
   ```
2. Open the solution file (`.sln`) in Visual Studio or your preferred IDE.
3. Add texture files and normal maps and build them using the monogame pipeline tool, substituting their names in the Main.cs file
4. Build and run the project.

## License

This project is licensed under the MIT License. Feel free to use and modify it for your own purposes.
