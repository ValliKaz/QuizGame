using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void onLevelSelect(int categoryIndex) {
        PlayerPrefs.SetInt("SelectedCategory", categoryIndex);
        if(categoryIndex == 0){
            SceneManager.LoadScene("Quiz1");
        }else if(categoryIndex == 1){
            SceneManager.LoadScene("Quiz2");
        }else if(categoryIndex == 2){
            SceneManager.LoadScene("Quiz3");
        }
    }
}
