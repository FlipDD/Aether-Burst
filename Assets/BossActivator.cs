using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject wall;
    [SerializeField] private PlayerInputController pic;
    [SerializeField] private GameObject crystal1;
    [SerializeField] private GameObject crystal2;

    private bool shaking;

//     void Start(){
// boss.SetActive(false);
//     } 

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !shaking)
        {
            shaking = true;
            wall.SetActive(true);
            slider.SetActive(true);
            crystal1.SetActive(false);
            crystal2.SetActive(false);
            // StartCoroutine(pic.CameraShaker2(2, .7f, .7f));
            // StartCoroutine(ActivateBoss());
            Destroy(gameObject, 4);
            boss.GetComponent<JorgeController>().SetState();
        }
    }

    // IEnumerator ActivateBoss()
    // {
    //     yield return new WaitForSeconds(2.5f);
    //     boss.SetActive(true);
    // }
}
