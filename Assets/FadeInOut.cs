using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        FadeOut();    
    }

    internal void FadeIn(bool goToNextScene = true)
    {
        img.CrossFadeAlpha(1f, 1f, true);
        if (goToNextScene)
            StartCoroutine(GoNextScene());
    }

    internal void FadeOut()
    {
        img.CrossFadeAlpha(0f, 1f, true);
    }

    IEnumerator GoNextScene()
    {
        yield return new WaitForSeconds(1);
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index+1);
    }

    internal IEnumerator WinScreen()
    {
        Image logo = GameObject.Find("Logo").GetComponent<Image>();
        Debug.Log(logo.name);
        FadeIn(false);
        yield return new WaitForSeconds(2);
        float t = 3f;
        while (t > 0)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, Mathf.Lerp(logo.color.a, 1, Time.deltaTime));
            yield return null;
        }
        yield return new WaitForSeconds(3);
        float tt = 3f;
        while (tt > 0)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, Mathf.Lerp(logo.color.a, 0, Time.deltaTime ));
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("EntranceSection");
        Destroy(gameObject);

    }

    void StartWinScreen()
    {
        StartCoroutine(WinScreen());
    }
}
