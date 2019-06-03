using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text start, settings, credits, quit; 
    [SerializeField]
    private GameObject settingsMenu;    

    Vector3 mouse_pos;
    Transform target;
    Vector3 object_pos;
    float angle;
    bool choosing;

    void Start()
    {
        target = transform;
        AudioManager.i.Play("Explore1", transform.position, true);
    }

    void Update()
    {
        if (!choosing)
        {
            mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f;
            object_pos = Camera.main.WorldToScreenPoint(target.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (angle >= 0 && angle < 90)
            {
                Debug.Log("Settings");
                settings.fontStyle = FontStyles.Bold;
                if (Input.GetMouseButtonDown(0))
                {
                    settingsMenu.SetActive(true);
                    choosing = true;
                }
            }
            else
                settings.fontStyle = FontStyles.Normal;

            if (angle >= 90 && angle < 180)
            {
                Debug.Log("Play");
                start.fontStyle = FontStyles.Bold;
                if (Input.GetMouseButtonUp(0))
                    SceneManager.LoadScene("EntranceSection");
            }
            else
                start.fontStyle = FontStyles.Normal;

            if (angle > -180 && angle <= -90)
            {
                Debug.Log("Credits");
                credits.fontStyle = FontStyles.Bold;
            }
            else
                credits.fontStyle = FontStyles.Normal;            

            if (angle > -90 && angle < 0)
            {
                Debug.Log("Exit");
                quit.fontStyle = FontStyles.Bold;   
                if (Input.GetMouseButtonUp(0))
                    Application.Quit();

            }
            else
                quit.fontStyle = FontStyles.Normal;
        }
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        choosing = false;
    }
}
