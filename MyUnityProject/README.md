# MyUnityProject

## Overview
This Unity project features an interactive drawing game where players can draw shapes by moving a player object. The game includes multiple figures, specifically a square and a circle, which the player can interact with.

## Project Structure
- **Assets**
  - **Scripts**
    - `PlayerNieve.cs`: Manages player interaction with figures and handles drawing mechanics.
    - `SquareFigure.cs`: Defines the square shape and its interaction logic.
    - `CircleFigure.cs`: Defines the circular shape and its interaction logic.
  - **Scenes**
    - `MainScene.unity`: The main scene containing the layout of the game, including the player and figures.
  - **Prefabs**
    - `Player.prefab`: Prefab for the player object, including necessary components for movement and interaction.
    - `SquareFigure.prefab`: Prefab for the square figure, visualizing and enabling interaction.
    - `CircleFigure.prefab`: Prefab for the circle figure, visualizing and enabling interaction.

## Setup Instructions
1. Open the project in Unity.
2. Navigate to the `Scenes` folder and open `MainScene.unity`.
3. Ensure all necessary prefabs are present in the scene.
4. Press Play to start the game and interact with the figures.

## Gameplay Mechanics
- Players control the player object to draw shapes by following the defined points of the figures.
- The game checks if the player is within a certain tolerance of the figure points to determine if the drawing is accurate.
- Players can complete figures and receive feedback on their performance.

## Additional Information
For any issues or contributions, please refer to the project's issue tracker or contact the project maintainer.