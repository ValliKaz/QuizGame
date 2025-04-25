using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public const string scoreKey = "Score";
    
    void Start() {
        LoadScore();
        UpdateScoreText();
    }

    public void AddScore(int amount) {
        score += amount;
        UpdateScoreText();
        SaveScore();
    }
    public void SubtractScore(int amount) {
        score -= amount;
        UpdateScoreText();
        SaveScore();
    }
    
    public void UpdateScoreText() {
        scoreText.text = "Score: " + score.ToString();
    }

    public void SaveScore() {
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();
    }

    public void LoadScore() {
        if(PlayerPrefs.HasKey(scoreKey)) {
            score = PlayerPrefs.GetInt(scoreKey);
            
        }else {
            score = 0;
        }
    }

    public void ResetScore() {
        PlayerPrefs.DeleteAll();
        score = 0;
        UpdateScoreText();
        SaveScore();
    }
}
