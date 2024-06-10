using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private float PassedTime = 0f;
    public UnityEvent NextLevelActions;
    public string LevelCompleteName = "VaporWaveLevelComplete";
    public void NextLevel() {
        
        LevelTime.PassedTime = PassedTime;
        SwitchLevelButton.LevelName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(LevelCompleteName);
    }
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("Player")) {
            NextLevelActions?.Invoke();
        }
    }
    void Update() {
        PassedTime += Time.deltaTime;
    }
}
