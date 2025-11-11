# 📘 README  
### SAE Institute Stuttgart  
**Modul:** D001 – Game Programming Basics (K2 / S2)  
**Student:** Eric Rosenberg  
**Projekt:** Escape Room (Console Edition)

---

## 1. Basis-Modul
Dies ist die Abgabe von **Eric Rosenberg** für das Modul  
**D001 – Game Programming Basics (K2 / S2)** am SAE Institute Stuttgart.

Das Projekt trägt den Titel **„Escape Room“** und wurde in **C# (.NET 8.0)** als **Konsolenanwendung** entwickelt.  
Ziel war die Entwicklung eines modular aufgebauten Game-Systems, das auf Prinzipien wie **SRP (Single Responsibility Principle)**, **Dependency Injection** und **strukturiertes Logging** basiert.

---

## 2. Abgabe nicht vorhanden
*(nicht zutreffend – alle geforderten Projektbestandteile vorhanden)*

---

## 3. Mehrere Abgaben in einem Ordner
*(nicht zutreffend – eigenständiges Einzelprojekt)*

---

## 4. Gruppenarbeit
*(nicht zutreffend – Einzelarbeit von Eric Rosenberg)*

---

## 5. Feature-Beschreibung  
### 🧩 Hauptfunktionen & Systemübersicht

Das Projekt **„Escape Room“** ist ein vollständig modular aufgebautes Konsolen-Spiel, bestehend aus mehreren spezialisierten Managern, klaren Datenstrukturen und sauber gekapselten Verantwortlichkeiten.

### 🎮 Kernsysteme

- **Dynamisches Board mit TileType-System**  
  → Steuerung über `GameBoardManager`, der das gesamte Spielfeld als 2D-Array verwaltet.  
  Jedes Feld besitzt einen eigenen `TileType` (Empty, Wall, Player, NPC, Key, Door usw.).

- **NPC-Quiz mit Dialogsystem (Fragenbank, Rewards, Score)**  
  → NPC-Daten werden aus einer `.txt`-Datei geladen und bestehen aus Name, Frage, Antworten, Rewards (Score & KeyFragments).  
  → Das System erlaubt die **einfache Erweiterung der Quiz-Datei**, ohne Codeänderung.

- **Key-/Door-Mechanik mit Level-Progression**  
  → Spieler sammelt Key-Fragmente, um Türen zu öffnen und das nächste Level freizuschalten.  
  → Nach Türöffnung: Levelmanager erhöht Schwierigkeitsgrad, Boardgröße und Spawnanzahl.

- **HUD-System (Top & Bottom HUD, Console Rendering)**  
  → Realisiert durch `UIManager`, der dynamisch obere und untere HUD-Zonen rendert.  
  → Befüllung durch `InteractionManager` (z. B. NPC-Dialoge, Systemnachrichten).

- **Diagnostics-Log mit Zeitstempeln**  
  → `DiagnosticsManager` speichert alle Logs (Errors, Warnings, Checks) mit Zeitstempeln.  
  → Ausgabe auf Knopfdruck (`I`) zur Laufzeit.

- **Level-Scaling + Difficulty-Progression**  
  → Jedes Level erfordert mehr Schlüssel, größere Maps und mehr NPCs.  
  → Dynamische Anpassung über `LevelManager` (Programmatisch durch `Program.cs`-Parameter).

- **Modularer Dependency-Injection-Aufbau**  
  → Jede Komponente besitzt eine eigene `Dependencies`-Record-Struktur.  
  → Keine zirkulären Abhängigkeiten, klare Zuständigkeiten.

---

### 🧠 Manager-Architektur (SRP / OOP)

- **GameBoardManager** – Erstellt und verwaltet das Spielfeld-Array mit TileTypes.  
- **GameObjectManager** – Registriert, bewegt und löscht alle Objekte auf dem Board.  
- **SpawnManager** – Steuert Spawn-Positionen für NPCs, Keys, Player und Doors.  
- **RulesManager** – Definiert alle Regeln für Bewegung und Spawns.  
- **InteractionManager** – Handhabt alle Interaktionen (NPC, Door, Key).  
- **InventoryManager** – Speichert Key-Fragmente und Score.  
- **LevelManager** – Verwaltet Levelwechsel, Difficulty und Key-Anforderungen.  
- **NpcManager** – Lädt NPCs aus `.txt` und erstellt Instanzen mit eigenem Leben & Daten.  
- **PlayerController** – Liest Input, steuert Bewegung und Interaktionen.  
- **PrintManager** – Kümmert sich um das visuelle Rendering (Board, HUD, Symbole).  
- **ScreenManager** – Verwaltet Start-, Tutorial-, Win- und GameOver-Screens.  
- **RandomManager** – Generiert deterministische Zufallsentscheidungen mit Seed.  
- **SymbolsManager** – Hält alle im Spiel verwendeten Symbole (z. B. Player, Door, Key).  
- **UIManager** – Baut und rendert das HUD (oben/unten).  
- **DiagnosticsManager** – Loggt Systemmeldungen mit Zeitstempel und Typ.  
- **Program.cs** – Zentrale Steuerung, Initialisierung und GameLoop mit State-Machine.

---

### ⚙️ Technische Eckdaten

- **Sprache / Framework:** C# (.NET 8.0 Console Application)  
- **Entwicklungsumgebung:** Visual Studio 2022  
- **Zielplattform:** Windows Console  
- **Architektur:** Modular / Dependency Injection / SRP  
- **Kommentierung:** Vollständige XML-Dokumentation jeder Methode  
- **Erweiterbarkeit:** Keine Hardcodings – neue Features können über Dependencies und Enums ergänzt werden.  
- **Designprinzip:** Einfache Erweiterbarkeit und vollständige Trennung der Logikschichten.

---

### 📂 Ordnerstruktur (nach SAE-Vorgabe)

```
EscapeRoom_Project/
│
├── src/              # Vollständiger Sourcecode
│   ├── Program.cs
│   ├── Managers/
│   ├── Dependencies/
│   ├── GameBoardObjects/
│   └── npc_questions.txt
│
├── release/          # Kompilierte Build-Dateien (.exe)
│
└── other/            # Screenshots, Videos, zusätzliche Infos
    ├── Screenshot_01.png
    ├── Screenshot_02.png
    ├── Screenshot_03.png
    └── Gameplay.mp4
```

---

### 🧾 Abgabebeschreibung (nach SAE-Vorgabe)

- **Art der Abgabe:** Einzelarbeit  
- **Medien:** Mindestens 1 Gameplay-Video (30–90 Sek.) + 3(+) Screenshots (≥ 1024×768) => Kleiner da es ein Konsolen Projekt ist.
- **Dateiname:** `README.md` (nicht verändert, Pflichtname laut SAE-Vorgabe)  
- **Inhalt:** Strukturierte Übersicht über Module, Features und Besonderheiten  

---

### 🧠 Zusammenfassung

Das Projekt **Escape Room** zeigt ein **vollständig eigenständig programmiertes Spielsystem**, das auf **sauberer Codearchitektur, Modularität, Datenkapselung und systematischem Logging** basiert.  
Alle Systeme wurden SRP-konform entworfen, mit klaren Schnittstellen und XML-Dokumentation.  

Das Spiel ist **leicht erweiterbar** (neue NPCs, neue Tiles, neue Regeln) und kann als **Grundlage für komplexere Game-Frameworks** verwendet werden.

---

**Stuttgart, 09. November 2025**  
_© 2025 Eric Rosenberg – SAE Institute Stuttgart_
