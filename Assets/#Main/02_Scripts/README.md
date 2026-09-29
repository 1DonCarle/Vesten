# Gameplay Systems Prototype

This folder contains a Unity ECS gameplay prototype built to explore data-oriented gameplay systems and their integration with Unity Physics, authoring/baking workflows, and Visual Effect Graphs. The code is a work in progress: it demonstrates architectural experiments and gameplay flows, not a finished or production-ready feature set.

## What It Explores

- **ECS authoring and runtime data:** MonoBehaviour authoring components bake scene objects into entities and ECS components.
- **Request-driven gameplay:** Systems communicate through entity buffers for projectile spawning, damage, and VFX events.
- **Physics-driven combat:** Physics event jobs identify relevant collisions and enqueue damage or destruction work for other systems.
- **Enemy behavior:** Target distance and behavior state drive basic idle, chase, aim, attack, and reload transitions.
- **Hybrid ECS/VFX integration:** Managed Visual Effect Graph resources are bridged to ECS through request buffers and GPU graphics buffers.

## Suggested Reading Order

1. [PlayerInputSystem.cs](ECS/Player/PlayerInputSystem.cs) and [PlayerAttackSystem.cs](ECS/Player/PlayerAttack/PlayerAttackSystem.cs) — translate player input into movement and attack intent.
2. [ProjectileSpawnSystem.cs](ECS/ProjectileSpawnSystem.cs) — consumes spawn requests, creates projectile entities, and submits matching VFX requests.
3. [BulletHitSystem.cs](ECS/Bullets/BulletHitSystem.cs) and [BulletHitJob.cs](ECS/Bullets/BulletHitJob.cs) — process physics trigger events and enqueue damage.
4. [DamageSystem.cs](ECS/Combat/DamageSystem.cs) and [DestroyEntitySystem.cs](ECS/Character/DestroyEntitySystem.cs) — apply queued damage and defer entity destruction.
5. [VFXManager.cs](ECS/Utility/VFXManager.cs) and [VFXSystem.cs](ECS/Utility/VFXSystem.cs) — manage batched VFX requests and graphics-buffer updates.
6. [TargetingSystem.cs](ECS/Enemy/TargetingSystem.cs), [DecisionSystem.cs](ECS/Enemy/DecisionSystem.cs), and [EnemyMoveToPlayerSystem.cs](ECS/Enemy/EnemyMoveToPlayerSystem.cs) — show the current enemy targeting and movement loop.

## Prototype Status

Several paths are scaffolding or deliberately incomplete while the gameplay design is being explored. In particular, experience spawning is not implemented yet, death presentation and follow-up behavior are placeholders, and enemy attacks do not yet represent a complete combat loop. Some systems also use managed Unity objects for integration points such as cameras and VFX.

Treat these scripts as a focused sample of the prototype's ECS structure and experiments. Runtime behavior depends on the scene, authoring setup, prefabs, input actions, and VFX assets configured elsewhere in the Unity project.

## Project Context

The project targets Unity `6000.4.10f1`. The scripts use Unity Entities, Unity Physics, and Unity Visual Effect Graph APIs; the complete project and its configured assets are needed to run the prototype.
