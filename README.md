# CS:GO Quiz Game

A Unity-based quiz game focused on Counter-Strike: Global Offensive (CS:GO) knowledge. Test your knowledge about weapons, maps, and game mechanics through various quiz formats.

## Description

This interactive quiz game challenges players' knowledge of CS:GO through multiple quiz types and difficulty levels. The game features a user-friendly interface, score tracking, and different quiz formats to keep players engaged.

## Features

- **Multiple Quiz Types**:
  - Multiple Choice Questions (MCQ)
  - True/False Questions
  - Word Quiz
  - Three different difficulty levels

- **Game Mechanics**:
  - Score tracking system
  - Level progression
  - Interactive UI elements
  - Immediate feedback on answers
  - CS:GO themed visuals and assets

- **User Interface**:
  - Main menu with game options
  - Quiz selection screen
  - Score display
  - Progress tracking
  - Responsive button controls

## Tools and Technologies Used

- **Unity Engine** (Latest Version)
- **C#** for scripting
- **TextMesh Pro** for text rendering
- **Universal Render Pipeline (URP)** for graphics
- **Unity Input System** for controls
- **Unity UI** for interface elements

## Project Structure

### Scripts
- `GameManager.cs`: Main game controller handling game state and flow
- `MenuManager.cs`: Manages menu navigation and UI interactions
- `LevelManager.cs`: Handles level progression and difficulty settings
- `ScoreManager.cs`: Tracks and manages player scores
- `QuizData.cs`: Data structure for quiz questions and answers
- `QuestionData.cs`: Question format and validation
- `WordGame.cs`: Specialized handler for word-based quiz questions

### Scenes
- `Menu.unity`: Main menu scene
- `Select.unity`: Quiz type selection scene
- `Quiz1.unity`: First quiz level
- `Quiz2.unity`: Second quiz level
- `Quiz3.unity`: Third quiz level

### Assets
- **Images**: CS:GO themed assets (weapons, maps, items)
- **Data**: Quiz question assets and prefabs
- **Settings**: Game configuration and UI templates

## Getting Started

1. Clone the repository
2. Open the project in Unity
3. Open the `Menu` scene
4. Press Play to start the game

## How to Play

1. Start from the main menu
2. Select your preferred quiz type
3. Choose a difficulty level
4. Answer questions to earn points
5. Track your progress through different levels
6. Try to achieve the highest score possible

Built with ❤️ by ValliKaz
