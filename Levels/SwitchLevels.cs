using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SwitchLevels : MonoBehaviour
{
    public string[] LevelNames;
    public UnityEvent NextLevelActions;
    public void NextLevel() {
        int index = 0;
        string LevelName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < LevelNames.Length;i++) {
            if (LevelNames[i] == LevelName) {
                index = i;
            }
        }
        SceneManager.LoadScene(LevelNames[index]);
    }
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Player")) {
            NextLevelActions?.Invoke();
        }
    }
}
