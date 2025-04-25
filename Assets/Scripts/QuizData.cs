using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "WordGame/QuizData")]
public class QuizData : ScriptableObject
{
    [System.Serializable]
    public class Quiz
    {
        public Sprite Image;
        public string correctWord;
    }

    public Quiz[] quizzes;
}


