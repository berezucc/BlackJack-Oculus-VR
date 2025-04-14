# 🃏 VR Blackjack – Toronto Metropolitan University

> **Course:** Virtual Reality  
> **Author:** Nikita Berezyuk

## 🎯 Project Overview

**VR Blackjack** is a solo-developed immersive virtual reality casino game designed to simulate the experience of gambling — without the financial risks. Created for the Virtual Reality course, the project allows users to play blackjack in a fully interactive 3D casino using intuitive VR controller gestures and spatial navigation.

This project aims to be both entertaining and educational, offering a safe environment to understand gambling mechanics while integrating core VR development techniques such as gesture recognition, teleportation movement, and interaction with 3D objects.

---

## 🕹️ Gameplay Instructions

- You start with **$1000** in virtual currency.
- Use your **left controller** to teleport to the blackjack table.
- Use your **right controller trigger** to grab chips (each chip = $50).
- Place chips on the table to place your bet.
- Press the **A button** to start the round.
- Gesture-based controls:
  - **Hit**: Move controller **up and down twice** like a knock.
  - **Stand**: Wave controller **left to right twice**.
- Win animations include **confetti**.
- You can leave and return to the game at any time.

---

## 🛠️ Features

- 🧠 Blackjack game logic
- ✋ Controller-based item interaction
- 🎮 VR gesture recognition (hit/stand)
- 🌌 Cubemap skybox for realistic environments
- 🎉 Particle system (confetti) on win
- 🧭 Teleportation movement system
- 📺 On-screen UI showing balance
- 🎨 Custom shader to highlight interactables
- 🔘 Button interaction logic

---

## 📽️ Project Demo Video

[Watch the project video](#https://www.youtube.com/watch?v=PVt41_nmirQ&ab_channel=NikitaBerezyuk) *(Link goes here once uploaded)*

---

## 🧾 Installation / Requirements

To run the project:

- Unity 2022.x (or compatible version with Oculus XR Plugin)
- Oculus Quest 2 headset (tested)
- Enable **XR Plugin Management** and install **Oculus Integration**
- Open the project in Unity and load the main Blackjack scene

---

## 📁 Project Structure

```text
Assets/
├── Blackjack/           # Core game logic and prefabs
├── Sounds/              # Audio files (ambient casino sounds)
├── Skybox/              # 8K HDR cubemap environment
├── UI/                  # UI prefabs (balance screen, buttons)
└── Scripts/             # Player interaction, chip logic, gesture detection
```

---

## 📜 License

For academic use only. All assets and code are developed for CPS643 coursework.

---

## 🙏 Acknowledgments

- Inspired by real-world casino mechanics and the psychology of gambling.
- Thanks to TMU’s VR Lab and course instructor for support and feedback.
