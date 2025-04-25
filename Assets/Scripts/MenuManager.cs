using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("Select");
    }

    public void ResetQuiz() {
        PlayerPrefs.DeleteAll();
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void BackToMenu() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Menu");
    }

    public void RestartQuiz() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Select");
    }
}
