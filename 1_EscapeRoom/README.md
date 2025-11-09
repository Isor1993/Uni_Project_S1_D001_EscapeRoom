# 🧩 Escape Room (Console Edition)
**Author:** Eric Rosenberg  
**Module:** D001 – Game Programming Basics (K2 / S2), SAE Institute Stuttgart  
**Date:** November 2025  

---

## 🎯 Overview
**Escape Room** is a modular console-based C# (.NET 8.0) game developed as part of the *Game Programming Basics* module at the SAE Institute Stuttgart.  
The project focuses on **clean architecture**, **SRP (Single Responsibility Principle)**, **dependency injection**, and **structured diagnostics logging**.

The goal was to create a flexible, scalable console game that demonstrates professional object-oriented structure and can easily be extended with new content and systems.

---

## 🧠 Core Features

- **Dynamic TileType-Based Game Board**  
  Managed by the `GameBoardManager`, which handles the entire board as a 2D array.  
  Each cell represents a distinct `TileType` (Empty, Wall, Player, NPC, Key, Door, etc.).

- **NPC Quiz System (Dialog, Rewards, Scoring)**  
  NPC data is loaded from an external `.txt` file (`npc_questions.txt`) containing question sets, possible answers, and rewards.  
  This allows easy content expansion without changing the code.

- **Key and Door Mechanics**  
  Players collect key fragments to open doors and advance to higher levels.  
  The `LevelManager` scales difficulty, board size, and spawn logic automatically.

- **HUD Rendering System**  
  Managed by the `UIManager`, which builds and displays upper and lower console HUD sections.  
  Data is dynamically provided by the `InteractionManager`.

- **Diagnostics Logging**  
  The `DiagnosticsManager` records all events (Errors, Warnings, Checks) with timestamps.  
  Logs can be displayed during runtime (press `I`).

- **Level Progression & Difficulty Scaling**  
  Each level increases the number of required keys, board complexity, and NPC count.

- **Dependency Injection Architecture**  
  Every major system has its own `Dependencies` record for modular setup.  
  No circular references – all managers have isolated, well-defined responsibilities.

---

## 🏗️ Manager Overview (SRP / OOP)

| Manager | Responsibility |
|----------|----------------|
| **GameBoardManager** | Creates and manages the game board (2D array of `TileType`). |
| **GameObjectManager** | Registers, moves, and removes objects on the board. |
| **SpawnManager** | Handles spawning of NPCs, keys, doors, and the player. |
| **RulesManager** | Defines and enforces game rules and movement restrictions. |
| **InteractionManager** | Processes all interactions (NPCs, doors, keys). |
| **InventoryManager** | Stores collected keys and player score. |
| **LevelManager** | Controls level transitions, difficulty scaling, and key requirements. |
| **NpcManager** | Loads and manages NPCs, connected to the `NpcDataLoader` and `.txt` database. |
| **PlayerController** | Handles input, player movement, and interaction direction. |
| **PrintManager** | Handles board and HUD rendering. |
| **ScreenManager** | Displays start, tutorial, win, and game-over screens. |
| **RandomManager** | Provides seeded randomization and math logic. |
| **SymbolsManager** | Central repository for all game symbols (used by UI and board). |
| **UIManager** | Builds and prints the HUD sections. |
| **DiagnosticsManager** | Manages debug logging with timestamps. |
| **Program.cs** | Entry point and main game loop with a state machine. |

---

## ⚙️ Technical Details

- **Language / Framework:** C# (.NET 8.0 Console Application)  
- **IDE:** Visual Studio 2022  
- **Target Platform:** Windows Console  
- **Architecture:** Modular / Dependency Injection / SRP  
- **Documentation:** Full XML comments for every method  
- **Design Goal:** Clear separation of logic layers, easy scalability, and maintainability  
- **No Hardcoding:** Systems are data-driven and easily extendable through enums and records  

---

## 📁 Project Structure

```
EscapeRoom_Project/
│
├── src/              # Full source code
│   ├── Program.cs
│   ├── Managers/
│   ├── Dependencies/
│   ├── GameBoardObjects/
│   └── npc_questions.txt
│
├── release/          # Compiled executable (.exe + .dll + .json)
│
└── other/            # Screenshots, documentation, and gameplay video
    ├── Screenshot_01.png
    ├── Screenshot_02.png
    ├── Screenshot_03.png
    └── Gameplay.mp4
```

---

## 🎮 How to Run

1. Navigate to the `/release/` folder.  
2. Ensure `npc_questions.txt` is placed next to the `.exe`.  
3. Run `Escape_Room.exe` (or `Semester1_D001_Escape_Room_Rosenberg.exe`).  
4. Use keyboard input to move and interact with NPCs, doors, and keys.

---

## 🧾 Project Summary

**Escape Room (Console Edition)** demonstrates a fully modular, SRP-based architecture for a console game.  
Every system is separated into individual managers, supported by dependency injection, and documented with XML comments.

This project can serve as a foundation for larger frameworks or engine prototypes, featuring:
- Clean OOP structure  
- Modular expandability  
- Fully data-driven logic  

---

**© 2025 Eric Rosenberg – SAE Institute Stuttgart**  
*“Built from scratch with structure, logic, and a little obsession for clean code.”*
