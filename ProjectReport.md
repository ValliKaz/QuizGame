# CS:GO Quiz Game - Project Report

## Project Setup

### Unity Configuration
- **Unity Version**: Latest version with Universal Render Pipeline (URP)
- **Project Type**: 2D Game
- **Render Pipeline**: Universal Render Pipeline (URP) for optimized graphics
- **Input System**: New Input System package for modern input handling
- **Text Rendering**: TextMesh Pro for high-quality text display

### Project Structure
- **Scenes**:
  - Menu.unity: Main menu and navigation hub
  - Select.unity: Quiz type selection interface
  - Quiz1.unity, Quiz2.unity, Quiz3.unity: Different difficulty levels
- **Folders**:
  - Assets/: Main project directory
  - Scripts/: C# scripts for game logic
  - Scenes/: Unity scene files
  - Data/: Quiz data and prefabs
  - Images/: CS:GO themed visual assets
  - Settings/: Game configuration files

### Build Settings
- **Platform**: Windows
- **Resolution**: 1920x1080 (Full HD)
- **Quality Settings**: Medium to High
- **Graphics API**: DirectX 11/12

## Assets

### Visual Assets
- **CS:GO Weapons**: AK-47, AWP, Deagle, Glock, etc.
- **Game Items**: Grenades, Molotov, Defuse Kit, Scope
- **Maps**: Mirage and other CS:GO map images
- **UI Elements**: Buttons, panels, backgrounds
- **Icons**: Game mechanics icons (Boost, Defuse)

### Audio Assets
- **Sound Effects**:
  - Button clicks
  - Correct/incorrect answer feedback
  - Score updates
  - Level completion
- **Background Music**: CS:GO themed ambient music

### Prefabs
- **UI Prefabs**:
  - Button.prefab: Standardized button with hover effects
  - Text (TMP).prefab: TextMesh Pro text component
- **Game Prefabs**:
  - Question panels
  - Answer buttons
  - Score display

### Data Assets
- **Quiz Data**:
  - MCQ.asset: Multiple choice questions
  - TrueFalse.asset: True/False questions
  - WordQuiz.asset: Word-based questions

## Scripting

### Core Scripts
1. **GameManager.cs**
   - Central game controller
   - Manages game state transitions
   - Handles quiz initialization
   - Controls game flow

2. **MenuManager.cs**
   - Main menu navigation
   - Scene transitions
   - UI element management

3. **LevelManager.cs**
   - Difficulty level management
   - Level progression logic
   - Score thresholds

4. **ScoreManager.cs**
   - Score calculation
   - High score tracking
   - Score display updates

### Quiz Logic Scripts
1. **QuizData.cs**
   - Question data structure
   - Answer validation
   - Quiz type management

2. **QuestionData.cs**
   - Individual question format
   - Answer options handling
   - Correct answer validation

3. **WordGame.cs**
   - Word quiz specific logic
   - Word validation
   - Special scoring rules

### Script Architecture
- **Design Pattern**: Singleton for managers
- **Event System**: Unity Events for UI interactions
- **Data Flow**: ScriptableObjects for quiz data
- **State Management**: Enum-based state machine

## Linking Objects

### Scene Hierarchy
```
- Canvas (Main UI)
  |- MainMenu
  |- QuizSelection
  |- QuizInterface
     |- QuestionPanel
     |- AnswerButtons
     |- ScoreDisplay
  |- SettingsPanel
```

### Component References
- **GameManager**:
  - References to all quiz scenes
  - ScoreManager instance
  - LevelManager instance

- **MenuManager**:
  - UI button references
  - Scene transition triggers
  - Settings panel reference

- **Quiz Interface**:
  - Question text component
  - Answer button components
  - Score text component
  - Timer component

### ScriptableObject Connections
- **Quiz Data**:
  - Connected to GameManager
  - Referenced by QuestionData
  - Used by WordGame

### Event System
- **UI Events**:
  - Button click events
  - Answer selection events
  - Score update events
- **Game Events**:
  - Level completion
  - Quiz type selection
  - Difficulty change

### Inspector Settings
- **GameManager**:
  - Quiz data references
  - Scene references
  - UI element references
- **UI Elements**:
  - Button listeners
  - Text components
  - Image components 