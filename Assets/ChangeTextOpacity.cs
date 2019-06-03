using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChangeTextOpacity : MonoBehaviour
{
    private char[] chars;
    [SerializeField]
    private TMP_FontAsset dungeonFont;
    [SerializeField]
    private TMP_FontAsset symbolFont;

    [SerializeField]
    private string welcomeMessage;
    private char[] messageChars;

    void Start()
    {
        messageChars = welcomeMessage.ToCharArray();

        foreach (char c in messageChars)
        {
            GameObject obj = new GameObject("Char");
            obj.transform.SetParent(transform);
            obj.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI objText = obj.GetComponent<TextMeshProUGUI>();
            objText.text = "" + c;
            objText.font = symbolFont;
            objText.fontSize = 80;
            objText.fontStyle = FontStyles.Bold;
            objText.alignment = TextAlignmentOptions.Center;
            StartCoroutine(ChangeText(obj.transform, objText));
        }
    }

    //increase opacity, rotate chars 90, change font, rotate chars 90 back, decrease opacity
    private IEnumerator ChangeText(Transform objToRotate, TextMeshProUGUI txt)
    {
        float t = 0;
        while (t < 2)
        {
            txt.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t/2));
            t += Time.deltaTime;
            yield return null;
        }
        txt.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1.5f));
        Quaternion initialRotation = transform.rotation;
        Quaternion firstDesiredRotation = Quaternion.Euler(359, transform.eulerAngles.y, transform.eulerAngles.z);
        objToRotate.rotation = firstDesiredRotation;
        Quaternion desiredRotation = Quaternion.Euler(270, transform.eulerAngles.y, transform.eulerAngles.z);
        while (objToRotate.eulerAngles.x > 271)
        {
            objToRotate.rotation = Quaternion.Slerp(objToRotate.rotation, desiredRotation, Time.deltaTime * 4);
            yield return null;
        }
        objToRotate.rotation = desiredRotation;
        txt.font = dungeonFont;
        while (objToRotate.eulerAngles.x < 359)
        {
            objToRotate.rotation = Quaternion.Slerp(objToRotate.rotation, initialRotation, Time.deltaTime * 4);
            yield return null;
        }
        objToRotate.rotation = initialRotation;
        yield return new WaitForSeconds(1);
        float tt = 0;
        while (tt < 2)
        {
            txt.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, tt/2));
            tt += Time.deltaTime;
            yield return null;
        }
        txt.color = new Color(1, 1, 1, 0);
    }

    private IEnumerator IncreaseTextOpacity(TextMeshPro txt)
    {
        float t = 0;
        // txt.font = anotherFont;
        while (t < 2)
        {
            txt.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t/2));
            t += Time.deltaTime;
            yield return null;
        }
        txt.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1.5f);
        // StartCoroutine(DecreaseTextOpacity());
    }

    private IEnumerator DecreaseTextOpacity(TextMeshPro txt)
    {
        float t = 0;
        while (t < 2)
        {
            txt.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t/2));
            t += Time.deltaTime;
            yield return null;
        }
        txt.color = new Color(1, 1, 1, 0);
    }
}
