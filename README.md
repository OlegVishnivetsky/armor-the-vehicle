# Armor the Vehicle - main machanic

A player controls a turret mounted on a car that drives forward through a level.
Enemies wait in idle until the car gets close, then chase and damage it. The player
aims the turret to shoot enemies and tries to reach the finish without losing all HP.

- **Win** — the car reaches the finish trigger → "You win".
- **Lose** — the car's HP reaches zero → "You lose".
- After either, a tap restarts the level.

---

## Tech

- **Unity 6** (6000.x)
- **VContainer** — dependency injection
- **UniTask** — async flow (scene load, timed sequences)
- **R3** — reactive `Health` (drives the health bar)
- **DOTween** — UI / panel / health-bar animation
- **Cinemachine** — camera blending
- **ZLinq** — zero acoocation linq
- **Feel (MMF)** — hit / death / damage feedback

---

## Boot → Gameplay flow

The game starts in a **Boot** scene and loads the **Gameplay** scene from there.

```
Boot scene
  └─ RootLifetimeScope          (persistent DI container)
       ├─ registers services    (Input, StaticData, Vfx, SceneLoader, StateMachine)
       └─ Bootstrapper           → switches the state machine to BootState

BootState
  ├─ StaticDataService.LoadAll()        load configs + prefabs from Resources
  └─ SceneLoaderService.LoadAsync()     show transition cover, async-load "Gameplay"

Gameplay scene
  └─ GameplayLifetimeScope      (child of root container)
       ├─ registers gameplay deps + all gameplay states
       └─ on Start() → switches the state machine to InitializeGameplayState
```

The single `GameStateMachine` lives in the **root** scope, so it persists across the
scene load. Root-level states (`BootState`) are created lazily by `StateFactory`;
gameplay states are registered eagerly from `GameplayLifetimeScope`, because the
factory resolves from the root container and can't reach child-scope registrations.

---

## State order

```
BootState
   │  load static data, async-load Gameplay scene
   ▼
InitializeGameplayState
   │  cover screen → clean up old level → spawn level, car, enemies → bind camera
   ▼
WaitForStartState
   │  set start camera, wait for camera to settle, reveal screen
   │  wait for tap
   ▼
GameLoopState  ◄──────────────────────────────┐
   │  car moves + fires, enemies chase,        │
   │  player aims turret                       │
   │                                           │
   ├─ car HP = 0 ──► LoseState                 │
   │                   │ death FX, delay,      │
   │                   │ show "You lose"       │
   │                   ▼                        │
   └─ finish reached ─► VictoryState           │
                         │ final camera, FX,   │
                         │ delay, show "You win"│
                         ▼                       │
                    WaitForRestartState          │
                         │ wait for tap          │
                         └──► InitializeGameplayState  (restart) ─┘
```

### What each state does

| State | Responsibility |
|---|---|
| **BootState** | Load static data, async-load the Gameplay scene behind the transition cover. |
| **InitializeGameplayState** | Show the cover, tear down any previous level, spawn the level / car / enemies, bind the camera, hand off to `WaitForStartState`. |
| **WaitForStartState** | Activate the starting camera, wait for it to settle, hide the cover, then wait for the first tap to start moving. |
| **GameLoopState** | Active play: car moves and fires, turret aims, enemies chase. Listens for car death and finish reached. |
| **LoseState** | Car death feedback + explosion VFX, short delay, show the lose panel, go to `WaitForRestartState`. |
| **VictoryState** | Final camera + confetti VFX, short delay, show the victory panel, go to `WaitForRestartState`. |
| **WaitForRestartState** | Wait for a tap, hide the result panels, restart via `InitializeGameplayState`. |

---

## Gameplay loop (inside GameLoopState)

1. The car drives forward automatically;
2. The player drags to aim the turret within its angle limits; firing happens while
   pressing, capped by the fire rate.
3. Enemies in range start chasing the car and deal contact damage.
4. Bullets and enemies report damage through a shared `IDamageable` interface.
5. `Health` is reactive — the car's health bar reveals on damage, shakes, and hides.
6. The loop ends when the car dies (**lose**) or reaches the finish (**win**).
