using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class WordGame : MonoBehaviour
{
    public QuizData quizData;
    public GameObject letterFieldPrefab;
    public GameObject letterButtonPrefab;
    
    public Transform letterFieldParent;
    public Transform letterButtonParent;
    
    [Header("UI Elements")]
    public GameObject gameFinishedPanel;
    public TMP_Text finalScoreText;
    public TMP_Text correctAnswersText;
    public TMP_Text currentScoreText;
    
    [Header("Score Settings")]
    public int correctWordScore = 10;
    public int wrongWordPenalty = 5;
    
    private TMP_Text[] letterFields;
    private Button[] letterButtons;
    private int currentFieldIndex = 0;
    private int currentQuizIndex = 0;
    private bool isGameActive = true;
    private int currentScore = 0;
    private int correctAnswers = 0;
    
    private QuizData.Quiz[] shuffledQuizzes;
    
    private bool isDragging = false;
    private Button draggedButton = null;
    private Vector3 touchOffset;
    
    void Start() {
        gameFinishedPanel.SetActive(false);
        currentScore = 0;
        UpdateScoreDisplay();
        ShuffleQuestions();
        LoadQuiz(currentQuizIndex);
    }
    
    void ShuffleQuestions() {
        if (quizData == null || quizData.quizzes == null) return;
        
        // Create a copy of quizzes array to shuffle
        shuffledQuizzes = new QuizData.Quiz[quizData.quizzes.Length];
        System.Array.Copy(quizData.quizzes, shuffledQuizzes, quizData.quizzes.Length);
        
        // Shuffle the quizzes array
        for (int i = shuffledQuizzes.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            QuizData.Quiz temp = shuffledQuizzes[i];
            shuffledQuizzes[i] = shuffledQuizzes[randomIndex];
            shuffledQuizzes[randomIndex] = temp;
        }
    }
    
    void LoadQuiz(int quizIndex) {
        ResetGame();
        if(shuffledQuizzes == null || shuffledQuizzes.Length == 0) {
            return;
        }
        if(quizIndex < 0 || quizIndex >= shuffledQuizzes.Length) {
            ShowGameFinished();
            return;
        }
        
        QuizData.Quiz quiz = shuffledQuizzes[quizIndex];
        Image image = GameObject.Find("QuizImage").GetComponent<Image>();
        if(image == null) {
            return;
        }
        image.sprite = quiz.Image;
        CreateLetterFields(quiz.correctWord.Length);
        CreateLetterButtons(quiz.correctWord);
        isGameActive = true;
    }
    
    void CreateLetterFields(int fieldCount) {
        foreach(Transform child in letterFieldParent) {
            Destroy(child.gameObject);
        }
        letterFields = new TMP_Text[fieldCount];
        for(int i = 0; i < fieldCount; i++){
            GameObject field = Instantiate(letterFieldPrefab, letterFieldParent);
            letterFields[i] = field.GetComponentInChildren<TMP_Text>();
            if(letterFields[i] == null) {
                return;
            }
        }
    }
    
    void CreateLetterButtons(string correctWord) {
        foreach(Transform child in letterButtonParent) {
            Destroy(child.gameObject);
        }
        letterButtons = new Button[8];
        char[] correctLetters = correctWord.ToCharArray();
        char[] wrongLetters = GenerateRandomLetters(8-correctLetters.Length, correctLetters);
        char[] allLetters = new char[8];
        correctLetters.CopyTo(allLetters, 0);
        wrongLetters.CopyTo(allLetters, correctLetters.Length);
        ShuffleLetters(allLetters);
        for(int i = 0; i < 8; i++){
            GameObject buttonObject = Instantiate(letterButtonPrefab, letterButtonParent);
            buttonObject.GetComponentInChildren<TMP_Text>().text = allLetters[i].ToString();
            int index = i;
            buttonObject.GetComponent<Button>().onClick.AddListener(() => OnLetterButtonClick(index));
            letterButtons[i] = buttonObject.GetComponent<Button>();
            if(letterButtons[i] == null) {
                return;
            }
        }
    }
    
    void OnLetterButtonClick(int buttonIndex) {
        if(currentFieldIndex < letterFields.Length) {
            string letter = buttonIndex < 8 ? letterButtons[buttonIndex].GetComponentInChildren<TMP_Text>().text : "";
            letterFields[currentFieldIndex].text = letter;
            letterButtons[buttonIndex].interactable = false;
            currentFieldIndex++;
        }
    }
    
    char[] GenerateRandomLetters(int count, char[] excludeLetters) {
        char[] randomLetters = new char[count];
        for(int i = 0; i < count; i++){
            char randomLetter;
            do{
                randomLetter = (char)('A' + Random.Range(0, 26));
            } while(System.Array.Exists(excludeLetters, c => c == randomLetter));
            randomLetters[i] = randomLetter;
        }
        return randomLetters;
    }
    
    void ShuffleLetters(char[] letters) {
        for(int i = letters.Length - 1; i > 0; i--){
            int randomIndex = Random.Range(0, i + 1);
            char temp = letters[i];
            letters[i] = letters[randomIndex];
            letters[randomIndex] = temp;
        }
    }
    
    public void CheckWord() {
        string playerWord = "";
        foreach(TMP_Text field in letterFields) {
            if(field.text == null) {
                return;
            }
            playerWord += field.text;
        }
        if(playerWord == shuffledQuizzes[currentQuizIndex].correctWord) {
            AddScore(correctWordScore);
            correctAnswers++;
            foreach(TMP_Text field in letterFields) {
                field.color = Color.green;
            }
            isGameActive = false;
            Invoke("LoadNextQuiz", 2f);
        } else {
            SubtractScore(wrongWordPenalty);
            foreach(TMP_Text field in letterFields) {
                field.color = Color.red;
            }
        }
    }
    
    void LoadNextQuiz() {
        currentQuizIndex++;
        if(currentQuizIndex < shuffledQuizzes.Length) {
            LoadQuiz(currentQuizIndex);
        } else {
            ShowGameFinished();
        }
    }
    
    public void DeleteLastLetter() {
        if (!isGameActive || currentFieldIndex <= 0 || letterFields == null) return;
        
        currentFieldIndex--;
        if (currentFieldIndex < 0 || currentFieldIndex >= letterFields.Length) return;
        
        TMP_Text currentField = letterFields[currentFieldIndex];
        if (currentField == null) return;
        
        string deletedLetter = currentField.text;
        if (string.IsNullOrEmpty(deletedLetter) || deletedLetter == "_") return;
        
        // Find and reactivate the corresponding button
        if (letterButtons != null) {
            foreach (Button button in letterButtons) {
                if (button != null && !button.interactable) {
                    TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                    if (buttonText != null && buttonText.text == deletedLetter) {
                        button.interactable = true;
                        break;
                    }
                }
            }
        }
        
        currentField.text = "_";
    }
    
    void ResetGame() {
        if(letterFields != null) {
            foreach(TMP_Text field in letterFields) {
                if(field != null) {
                    field.text = "";
                    field.color = Color.white;
                }
            }
        }
        if(letterButtons != null) {
            foreach(Button button in letterButtons) {
                if(button != null) {
                    button.interactable = true;
                }
            }
        }
        currentFieldIndex = 0;
    }
    
    public void UseHint() {
        if (!isGameActive) return;
        
        string correctWord = shuffledQuizzes[currentQuizIndex].correctWord;
        char[] correctLetters = correctWord.ToCharArray();
        
        for (int i = 0; i < letterButtons.Length; i++) {
            if (letterButtons[i] != null && letterButtons[i].interactable) {
                string buttonLetter = letterButtons[i].GetComponentInChildren<TMP_Text>().text;
                bool isCorrectLetter = false;
                
                foreach (char correctLetter in correctLetters) {
                    if (buttonLetter == correctLetter.ToString()) {
                        isCorrectLetter = true;
                        break;
                    }
                }
                
                if (!isCorrectLetter) {
                    Destroy(letterButtons[i].gameObject);
                    letterButtons[i] = null;
                }
            }
        }
    }
    
    void AddScore(int points) {
        currentScore += points;
        UpdateScoreDisplay();
    }
    
    void SubtractScore(int points) {
        currentScore = Mathf.Max(0, currentScore - points);
        UpdateScoreDisplay();
    }
    
    void UpdateScoreDisplay() {
        if (currentScoreText != null) {
            currentScoreText.text = "Score: " + currentScore;
        }
    }
    
    void ShowGameFinished() {
        isGameActive = false;
        gameFinishedPanel.SetActive(true);
        finalScoreText.text = "Score: " + currentScore;
        correctAnswersText.text = "Correct: " + correctAnswers + " / " + shuffledQuizzes.Length;
    }
    
    void Update() {
        if (!isGameActive) return;
        
        // Handle touch/mouse input
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            HandleTouchInput(touch);
        } else if (Input.GetMouseButton(0)) {
            // Simulate touch with mouse
            Touch simulatedTouch = new Touch();
            simulatedTouch.position = Input.mousePosition;
            simulatedTouch.phase = Input.GetMouseButtonDown(0) ? TouchPhase.Began : 
                                 Input.GetMouseButtonUp(0) ? TouchPhase.Ended : 
                                 TouchPhase.Moved;
            HandleTouchInput(simulatedTouch);
        }
    }
    
    void HandleTouchInput(Touch touch) {
        switch (touch.phase) {
            case TouchPhase.Began:
                HandleTouchBegan(touch.position);
                break;
            case TouchPhase.Moved:
                HandleTouchMoved(touch.position);
                break;
            case TouchPhase.Ended:
                HandleTouchEnded(touch.position);
                break;
        }
    }
    
    void HandleTouchBegan(Vector2 position) {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;
        
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        
        foreach (var result in results) {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null && button.interactable) {
                isDragging = true;
                draggedButton = button;
                touchOffset = button.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
                break;
            }
        }
    }
    
    void HandleTouchMoved(Vector2 position) {
        if (isDragging && draggedButton != null) {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f)) + touchOffset;
            draggedButton.transform.position = newPosition;
        }
    }
    
    void HandleTouchEnded(Vector2 position) {
        if (isDragging && draggedButton != null) {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = position;
            
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            
            Button targetButton = null;
            foreach (var result in results) {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null && button != draggedButton) {
                    targetButton = button;
                    break;
                }
            }
            
            if (targetButton != null) {
                // Swap positions
                Vector3 targetPos = targetButton.transform.position;
                targetButton.transform.position = draggedButton.transform.position;
                draggedButton.transform.position = targetPos;
                
                // Swap in array
                int draggedIndex = System.Array.IndexOf(letterButtons, draggedButton);
                int targetIndex = System.Array.IndexOf(letterButtons, targetButton);
                if (draggedIndex != -1 && targetIndex != -1) {
                    Button temp = letterButtons[draggedIndex];
                    letterButtons[draggedIndex] = letterButtons[targetIndex];
                    letterButtons[targetIndex] = temp;
                }
            } else {
                // Return to original position
                draggedButton.transform.position = draggedButton.GetComponent<RectTransform>().anchoredPosition;
            }
        }
        
        isDragging = false;
        draggedButton = null;
    }
}
