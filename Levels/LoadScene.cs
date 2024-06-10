using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string SceneName;
    public LevelArray SceneArray;
    public void Load() {
        SceneManager.LoadScene(SceneName);
    }
    public void LoadOffArray(int num) {
        SceneManager.LoadScene(SceneArray.Levels[num]);
    }
}
