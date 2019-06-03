using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateRandomPhrase : MonoBehaviour
{
    [TextArea]
    public string[] phrases;

    void Start()
    {
        TextMeshProUGUI txt = GetComponent<TextMeshProUGUI>();
        txt.text = "" + phrases[Random.Range(0, phrases.Length-2)];
        txt.fontSize = 45;
    }
}
