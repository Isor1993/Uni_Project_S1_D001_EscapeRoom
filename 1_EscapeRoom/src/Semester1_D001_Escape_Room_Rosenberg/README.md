# 🧩 Escape Room – Console Edition  
*A modular C# console game built with clean architecture, dependency injection, and a obsession for structured design.*

![Gameplay Screenshot](1_EscapeRoom/other/Screenshot_01.png)

---

## 🎮 Overview
**Escape Room** is a fully modular **C# (.NET 8.0)** console game created as part of my Game Programming studies at the **SAE Institute Stuttgart**.  
It’s a blend of classic text-based gameplay and system architecture practice — every part of the game (NPCs, player, UI, spawns, levels) is handled by its own manager.  
The project demonstrates how far you can push structure and readability, even inside a simple console window.

---

## ✨ Core Features

- 🧱 **Modular Manager Architecture** – Each system (UI, Interaction, Rules, Level, etc.) is its own module with dependency injection.  
- 🧩 **Clean SRP Implementation** – No spaghetti code. Every class does one thing and does it well.  
- 🗝️ **Key & Door Progression System** – Collect key fragments to unlock new levels.  
- 🧠 **NPC Quiz System** – NPCs pull their dialogue and questions from a `.txt` file.  
- 🖥️ **Dynamic HUD Rendering** – A top and bottom HUD drawn entirely in the console.  
- 🪄 **Diagnostics Logger** – Internal console logger with timestamped categories (Errors, Warnings, Checks).  
- 📈 **Level Scaling** – Automatic difficulty and map growth across multiple levels.  

---

## 🕹️ How to Play

1. Clone the repository:  
   ```bash
   git clone https://github.com/<yourusername>/EscapeRoom-Console.git
   ```
2. Open the project in **Visual Studio 2022**.  
3. Run without debugging (`Ctrl + F5`).  
4. Controls:  
   - `WASD` → Move  
   - `E` → Interact (talk, collect, open)  
   - `I` → Show logs  
   - `ESC` → Quit the game  

> Make sure the file `npc_questions.txt` is next to the executable when running the release version.

---

## 🧱 Architecture Breakdown

| System | Responsibility |
|--------|----------------|
| **GameBoardManager** | Creates and updates the board grid (2D TileType array). |
| **InteractionManager** | Handles all object/player interactions (NPC, Door, Key). |
| **UIManager** | Builds and renders top/bottom HUD. |
| **RulesManager** | Defines valid player movement and object placement. |
| **LevelManager** | Manages level flow, scaling, and progression logic. |
| **NpcManager** | Loads NPC data from `.txt` (dialogues, rewards, questions). |
| **InventoryManager** | Stores key fragments and score. |
| **PrintManager** | Renders game visuals (board, UI, messages). |
| **DiagnosticsManager** | Logs all runtime messages. |
| **Program.cs** | Core loop, initialization, and state control. |

---

## 🧠 Design Principles

- **SRP (Single Responsibility Principle)** – Each class handles one defined task.  
- **Dependency Injection** – Clean and testable inter-manager connections.  
- **Data-Driven Design** – External `.txt` files for flexible content.  
- **Minimal Hardcoding** – Logic is abstracted and reusable.  
- **Readable Console Rendering** – Structured ASCII-style UI.

---

## ⚙️ Tech Stack

| Category | Tools |
|-----------|--------|
| Language | C# |
| Framework | .NET 8.0 |
| IDE | Visual Studio 2022 |
| Architecture | SRP / Modular / Dependency Injection |
| Platform | Windows Console |

---

## 📂 Repository Structure

```
1_EscapeRoom/
│
├── src/								    # Full C# source code
│   ├── Program.cs
│   ├── Managers/
│   ├── Dependencies/
│   ├── GameBoardObjects/
│   └── npc_questions.txt
│
├── other/									# Screenshots and gameplay media
│   ├── Screenshot_01.png
│   ├── Screenshot_02.png
│   ├── Screenshot_03.png
│   └── Gameplay.mp4
│
├── README.md								# You’re reading it right now
│
└── Semester1_D001_Escape_Room_Rosenberg	# Visual Studio Repo Project
```

---

## 💬 Behind the Project
> “Escape Room” isn’t just a programming exercise — it’s part of my long-term goal to master modular game system design.  
> Each subsystem was built, tested, and documented to fit within a real game-engine-like workflow.  
> The project reflects my approach to **clean code, architecture discipline, and technical creativity**.

---

## 🧾 License
This project is released under the **MIT License** – you’re free to use, modify, and learn from the code.  
(See `LICENSE` for details.)

---

## 📫 Contact

**Eric Rosenberg**  
🎓 Game Programming Student – SAE Institute Stuttgart  
💼 [LinkedIn](https://https://www.linkedin.com/in/eric-rosenberg-441649288/)  
🎮 [Instagram Devlog – @IsorTowerDev](https://www.instagram.com/isor_gamedev)  
📧 Contact: *[IsorDev@email.de]*  

---

**© 2025 Eric Rosenberg – Built with structure, logic, and a bit of chaos magic.**
