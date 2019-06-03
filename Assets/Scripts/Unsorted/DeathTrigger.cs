using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameObject loseState;

    private TMPro.TMP_Text loseText;
    private Image loseImg;

    void Start()
    {
        loseText = loseState.GetComponent<TMPro.TMP_Text>();
        loseImg = loseState.GetComponentInChildren<Image>();
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < col.transform.childCount ; i++)
            {
                if (col.transform.GetChild(i).name == "CamFollow")
                    col.transform.GetChild(i).transform.parent = null;
            }
            col.gameObject.GetComponent<Rigidbody>().mass = 1000000;
            StartCoroutine(LoseState());
            col.gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;        
        }
        else if (col.gameObject.CompareTag("Ground"))
            return;
        else
            Destroy(col.gameObject);
    }

    internal IEnumerator LoseState()
    {
        
        float t = 0;
        for (t = 0; t < 6; t += Time.deltaTime)
        {
            loseText.color = new Color(1, .2f, .2f, Mathf.Lerp(0, .8f, t/2.5f));
            loseImg.color = new Color(.7f, .4f, .4f, Mathf.Lerp(0, .3f, t/3.5f));
            yield return null;
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

}