using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTime : MonoBehaviour
{
    public static float PassedTime;
    public TextMeshProUGUI TimeText;

    void Start()
    {
        TimeText = GetComponent<TextMeshProUGUI>();
        string EndString = "";
        EndString += ((int)PassedTime).ToString() + "S" + ":"; 
        EndString += ((int)((PassedTime%1)*1000)).ToString() + "MS";

        TimeText.text = EndString;
    }
}
