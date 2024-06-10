using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchLevelButton : MonoBehaviour
{
    public LevelArray LevelNames;
    public static string LevelName;

    public void RetryLevel() {
        int index = 0;
        for (int i = 0; i < LevelNames.Levels.Length;i++) {
            if (LevelNames.Levels[i] == LevelName) {
                index = i;
            }
        }
        SceneManager.LoadScene(LevelNames.Levels[index]);
    }
    public void NextLevel() {
        int index = 0;
        for (int i = 0; i < LevelNames.Levels.Length;i++) {
            if (LevelNames.Levels[i] == LevelName) {
                index = i;
            }
        }
        SceneManager.LoadScene(LevelNames.Levels[index+1]);
    }


    
}
