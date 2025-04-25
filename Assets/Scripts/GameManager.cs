using UnityEngine;
using TMPro;
using UnityEngine.UI;   
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    public QuestionData[] categories;
    public QuestionData selectedCategory;
    private Question[] shuffledQuestions;
    private int currentQuestionIndex = 0;
    public TMP_Text questionText;
    public Image questionImage;
    public Button[] replyButtons;
    
    [Header("Score")]
    public ScoreManager score;
    public int correctReplyScore = 10;
    public int incorrectReplyScore = 5;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI correctReplyText;

    [Header("CorrectReplyIndex")]
    public int correctReplyIndex;
    int correctReplyCount;

    [Header("GameFinishPanel")]
    public GameObject gameFinishPanel;

    private bool isDragging = false;
    private Button draggedButton = null;
    private Vector3 touchOffset;

    void Start() {
        int selectedCategoryIndex = PlayerPrefs.GetInt("SelectedCategory", 0);
        gameFinishPanel.SetActive(false);
        SelectCategory(selectedCategoryIndex);
    }

    public void SelectCategory(int categoryIndex) {
        selectedCategory = categories[categoryIndex];
        currentQuestionIndex = 0;
        ShuffleQuestions();
        DisplayQuestion();
    }
    
    void ShuffleQuestions() {
        if (selectedCategory == null || selectedCategory.questions == null) return;
        
        // Create a copy of questions array to shuffle
        shuffledQuestions = new Question[selectedCategory.questions.Length];
        System.Array.Copy(selectedCategory.questions, shuffledQuestions, selectedCategory.questions.Length);
        
        // Shuffle the questions array
        for (int i = shuffledQuestions.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            Question temp = shuffledQuestions[i];
            shuffledQuestions[i] = shuffledQuestions[randomIndex];
            shuffledQuestions[randomIndex] = temp;
        }
    }
    
    public void DisplayQuestion() {
        if(selectedCategory == null || shuffledQuestions == null) return;
        ResetReplyColors();
        var question = shuffledQuestions[currentQuestionIndex];
        questionText.text = question.questionText;
        questionImage.sprite = question.questionImage;

        for(int i = 0; i < replyButtons.Length; i++) {
            TMP_Text buttonText = replyButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = question.replies[i];
        }
    }

    public void OnReplyButtonClick(int replyIndex) {
        if(replyIndex == shuffledQuestions[currentQuestionIndex].correctReplyIndex) {
            score.AddScore(correctReplyScore);
            correctReplyCount++;
        } else {
            score.SubtractScore(incorrectReplyScore);
        }

        currentQuestionIndex++;
        if(currentQuestionIndex < shuffledQuestions.Length) {
            DisplayQuestion();
        } else {
            ShowFinalResults();
        }
    }

    public void ShowCorrectReply() {
        correctReplyIndex = shuffledQuestions[currentQuestionIndex].correctReplyIndex;
        for(int i = 0; i < replyButtons.Length; i++) {
            if(i+1 == correctReplyIndex) {
                replyButtons[i].GetComponent<Image>().color = Color.green;
            } else {
                replyButtons[i].GetComponent<Image>().color = Color.red;
            }
        }
    }
    
    public void ResetReplyColors() {
        foreach(Button button in replyButtons) {
            button.GetComponent<Image>().color = Color.white;
        }
    }

    public void ShowFinalResults() {
        gameFinishPanel.SetActive(true);
        scoreText.text = "Score: " + score.score;
        correctReplyText.text = "Correct: " + correctReplyCount + " / " + shuffledQuestions.Length;
    }

    void Update() {
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
                int draggedIndex = System.Array.IndexOf(replyButtons, draggedButton);
                int targetIndex = System.Array.IndexOf(replyButtons, targetButton);
                if (draggedIndex != -1 && targetIndex != -1) {
                    Button temp = replyButtons[draggedIndex];
                    replyButtons[draggedIndex] = replyButtons[targetIndex];
                    replyButtons[targetIndex] = temp;
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
