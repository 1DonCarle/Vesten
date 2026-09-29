# Vesten

Vesten is a Unity gameplay prototype for a top-down, Vampire Survivors-style action game. The project focuses on ECS-based combat, enemy behavior, and Visual Effect Graph integration.

The main purpose of the project is to explore how gameplay requests, physics events and presentation can be connected through Unity Entities systems, while building the foundations for a game with large numbers of enemies, projectiles and effects.

This repository is a work in progress. Some gameplay paths are prototypes or scaffolding, so it should be read as an evolving technical project rather than a finished game.

## Prototype Highlights

- ECS authoring and baking for player, enemy, character, and projectile data
- Player movement, aiming, and projectile attack flow
- Physics-event-driven projectile hits and buffered damage requests
- Basic enemy targeting, behavior states, movement, and collision damage
- Batched Visual Effect Graph requests using graphics buffers

## Open the Project

1. Install Unity `6000.4.10f1` using Unity Hub.
2. Clone this repository and add the repository root as a project in Unity Hub.
3. Open the project with the version above and allow Unity to import packages and assets.
4. Open `Assets/#Main/01_Scenes/Main_scn.unity` to inspect the main scene. `Assets/#Main/01_Scenes/VFXZoo_scn.unity` is an additional VFX scene.

The project uses Unity Entities `6.4.0`, Entities Graphics `6.4.0`, URP `17.4.0`, the Input System `1.19.0`, and Visual Effect Graph `17.4.0`; package dependencies are declared in `Packages/manifest.json`.

## Code Tour

- `Assets/#Main/01_Scenes/` — main and VFX scenes
- `Assets/#Main/02_Scripts/` — gameplay ECS systems, authoring components, and utilities
- `Assets/#Main/03_Models/` — model assets
- `Assets/#Main/04_Materials/` — materials
- `Assets/#Main/05_Textures/` — textures
- `Assets/#Main/06_Shaders/` — shaders
- `Assets/#Main/07_VFX/` — Visual Effect Graph assets
- `Assets/#Main/08_Prefabs/` — prefabs

For a guided tour of the gameplay code and its current status, see the [Gameplay Systems Prototype README](Assets/%23Main/02_Scripts/README.md).

## Current Scope

The project is actively being developed. Experience spawning and parts of enemy combat and death presentation are incomplete. Scene behavior also depends on the configured authoring components, prefabs, input actions, and VFX assets. The detailed systems README calls out the main areas to explore and their prototype status.
