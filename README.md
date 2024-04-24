# **Asteroids**

**Overview**

This project is a simplified version of the classic Asteroids game, written in C# using the Unity game engine. It leverages a custom-built Entity-Component-System (ECS) framework to showcase advanced programming techniques and optimization strategies. The ECS architecture ensures a clean separation of concerns by decoupling game logic from the interface and visual data, operating independently of Unity's game objects.

**Background**

This project was undertaken to demonstrate my programming skills and ability to meet specific requirements for a technical challenge.

**Features**

- **Custom ECS Framework**: The game uses a self-written ECS framework designed for optimal performance, ensuring there are no allocations during runtime, which helps maintain a consistent and smooth gameplay experience.
- **Optimized Performance**: Additional attention has been taken to avoid runtime allocations, preventing performance spikes and garbage collection during runtime.

**Installation and Setup**

To set up and run the project, follow these steps:

1. **Clone the Repository**:

- git clone https://github.com/funkypickledpickle/Asteroids

2. **Open the Project in Unity**:

- Open Unity Hub.
- Click on **Add** and select the **asteroids** folder.
- Open the project in Unity.

3. **Run the Game**:

- Press the **Play** button in the Unity Editor to start the game.

**Gameplay**

In this simplified version of Asteroids, you control a spaceship in a 2D environment. Your objective is to destroy the asteroids before they collide with your spaceship. The game features basic mechanics including:

- **Spaceship Movement**: Use the arrow keys to navigate the spaceship, which moves with acceleration and inertia.
- **Shooting**: Press the space bar to shoot bullets. Press shift to use laser.
- **Score Tracking**: Your score increases as you destroy more asteroids.
- **Weapon Types**:
    - **Bullets**: Break asteroids into smaller, faster-moving fragments.
    - **Laser**: Destroys all objects it intersects, with limited shots that recharge over time.
- **Collision Handling**: Collision with asteroids, fragments, or UFOs results in a game over message and a prompt to restart.
- **Periodic Spawns**: Asteroids and UFOs appear periodically, with UFOs actively pursuing the player.

**UI Features**

The game includes a comprehensive UI displaying the following spaceship metrics for debugging purposes:

- Coordinates
- Rotation Angle
- Instantaneous Speed
- Number of Laser Charges
- Laser Cooldown Time\

**Key Scripts**

- **Loader.cs**: The entry point of the application, initializing the DI container and game components.
- **GameController.cs**: Manages the ECS and is controlled by game states, which utilize public methods to alter the game state.

**Project**  **Structure**

Here's an overview of the project's structure:

- **Configuration/**: Contains gameplay and optimization settings (e.g., asteroid spawning frequency, player lives).
- **Tools/**: Utility extensions and independent tools.
- **UnsafeTools/**: Includes unsafe code utilities for performance.
- **Services/**: Common services (e.g., resource loading, camera info, frame timing).
- **ValueTypeECS/**: Custom ECS framework, dependent on Tools, Configuration, and Services.
- **GameplayECS/**: Game logic based on the ValueTypeECS.
- **Gameplay/**: Has the GameContorller that manages ECS and game states (Menu, Pause, Gameplay) that changes the state of the game.
- **UI/**: Handles the visual interface, user interaction, and displays visual data (e.g., ships, bullets).
- **Loader/**: Initializes the DI container and all game components.

**Contact**

For any questions or suggestions, feel free to reach out:

- Email:funkypickledpickle@gmail.com
