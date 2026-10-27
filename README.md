# Operation: Lost Trio

A Unity 3D tactical survival/rescue prototype with a gritty urban district, weapons, loot, enemies, and rescue objectives.

## What's included
- First-person movement and mouse look
- Hitscan weapon prototype with magazine, reserve ammo, and reload
- Health, damage, and healing
- E-key interaction for loot and rescue objectives
- Enemy pursuit and close-range attacks using Unity NavMesh
- Basic HUD and rescue counter
- **Procedural urban map builder** with generated road markings, building shells, window panels, rooftops, rubble, cover, checkpoint props, grunge, brick, and ground textures
- **Adaptive difficulty** that increases enemy damage and detection range as a run continues
- **Main menu / pause flow** with Start, Resume, Return to Title, and Quit hooks for Unity UI buttons
- **Enemy wave spawner** with increasing wave sizes and configurable spawn points
- **Ammo pickup props** and optional magazine refill
- **Mission objective tracker** and a polished high-contrast tactical HUD with health bar, ammo, rescue count, wave number, and objective text

## Requirements
- Unity 2022.3 LTS or newer recommended
- A 3D Unity project
- AI Navigation / NavMesh support for enemy agents

## Quick start
1. Create a Unity 3D project.
2. Copy `Assets/OperationLostTrio` into the project's `Assets` folder.
3. Create an empty GameObject named `World Builder` and add `VisualWorldBuilder`. Press Play to generate the blockout district. To rebuild in the Editor, use the component context menu **Build Textured Survival Map**.
4. Create a player capsule with a `CharacterController`, `FirstPersonController`, `Health`, `WeaponRaycast`, and `InteractRaycaster`. Add a child camera and assign it to the controller, weapon, and interaction component.
5. Create enemies with a `NavMeshAgent`, `EnemyPatrol`, and `Health`; assign the player transform as their target. Bake a NavMesh over the walkable level.
6. Add `OperationGameManager` to an empty GameObject. Place three friend NPCs with colliders and `RescueObjective` components, then assign the manager.
7. Add `LootCrate` to crates with colliders. Add `SurvivalHUD` and assign the player's health, weapon, and game manager.
8. Add `SurvivalMainMenu` and wire Unity UI buttons to `StartMission`, `ResumeGame`, `ReturnToTitle`, and `QuitGame`.
9. For waves, add `EnemyWaveSpawner`, assign an enemy prefab with `Health` and `EnemyPatrol`, set spawn point transforms, and call `StartWaves` from a UI button or another script.
10. Add `WeaponPickup` to ammo pickup props. Add `MissionObjective` and assign it to `SurvivalHUD` to show the active objective.
11. Optionally add `AdaptiveDifficulty` to a manager object, assign the player and enemies, and tune the ramp interval and maximum damage increase.

## Controls
- WASD: move
- Mouse: look / aim
- Shift: sprint
- Space: jump
- Left mouse: shoot
- R: reload
- E: interact, loot, or rescue
- Escape: unlock cursor

## Visual upgrade notes
The map builder creates a stylized, gritty blockout using Unity primitives and generated textures so it works without downloading assets. For a genuinely high-end look, replace placeholder blocks with licensed PBR building/weapon/character assets, use baked lighting or a configured URP/HDRP pipeline, add decals, particles, sound, animations, and post-processing. The generated district is a starting level, not a finished open world.

## Important
This is a prototype scaffold, not a finished game. Scene setup, references, NavMesh baking, testing, and balancing are still required. The scripts use Unity's legacy Input API; enable **Input Manager (Old)** or **Both** in Player Settings if your project is configured for the new Input System only. Never commit passwords, API keys, or private tokens.