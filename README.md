# 🎯 RankingSystem by Seen

A **Counter-Strike 2 plugin for CounterStrikeSharp** that adds a ranking system to your server.  
Players gain or lose points based on kills, deaths, and round results. Rankings are persistent and can be displayed in chat via simple commands.

---

## ✨ Features

- Tracks **player score** across rounds and sessions
- Awards / removes points for:
  - Kill: **+6 points**
  - Death: **-2 points**
  - Round Win: **+3 points**
  - Round Loss: **-1 point**
  - Kill Streak: _Not yet implemented_ 🚧
  - Chat Messages: _Not yet implemented_ 🚧
- Persistent leaderboard (saved automatically on round end / plugin unload)
- **Chat commands** for players to check their rank or see the Top 15
- Colored chat messages with a **table-like Top 15 leaderboard**
- Highlights the current player in the leaderboard
- Special color highlights for **1st (gold)**, **2nd (silver)**, and **3rd (bronze)** place

---

## 📦 Installation

1. Make sure you are running a CS2 server with [CounterStrikeSharp](https://docs.cssharp.dev/) installed.
2. Download the compiled plugin `.dll` file (or build it yourself).
3. Place the `.dll` in your server’s:
   ```
   csgo/addons/counterstrikesharp/plugins/
   ```
4. Restart your server (or reload plugins).

On successful load, you should see:

```
[RankingSystem] Plugin loaded successfully!
```

---

## 🎮 Commands

Players can use the following commands in chat or console:

| Command  | Alias   | Description                                        |
| -------- | ------- | -------------------------------------------------- |
| `!rank`  | `rank`  | Shows your current score, rank, and total players. |
| `!top15` | `top15` | Displays the Top 15 leaderboard.                   |

---

## 🗂️ Data Persistence

- Rankings are stored automatically by the plugin.
- Leaderboard is saved:
  - At the end of each round
  - When the plugin is unloaded
- Ensures scores persist between server restarts.

---

## 📊 Example Output

**!rank**

```
[RankingSystem] Your current position is 5 / 20!
[RankingSystem] Your current score is 64!
```

**!top15**

```
[RankingSystem] TOP 15 - Leaderboard
 #   Name               Score
  1. Seen.              120
  2. Player123          98
  3. AnotherGuy         85
  ...
```

---

## 👤 Author

**Seen**  
Version: **1.0.0**

---
