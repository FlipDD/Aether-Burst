using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[AddComponentMenu("Camera Follow")]
public class CameraBehaviour : MonoBehaviour
{
    [Space(5)]
    [Tooltip("Create an empty gameobject and parent it to your players hip/pelvis bone.")]
    public GameObject CameraFollowObj;
    public GameObject CameraFollowEnemy;

    [Header("Camera Rotation Values")]
    [Space(5)]
    public float CameraMoveSpeed = 120.0f;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public float lastypos;
    Vector3 FollowPOS;
    GameObject CameraObj;
    GameObject PlayerObj;
    float camDistanceXToPlayer;
    float camDistanceYToPlayer;
    float camDistanceZToPlayer;
    float mouseX;
    float mouseY;
    float finalInputX;
    float finalInputZ;
    float smoothX;
    float smoothY;
    float rotY = 0.0f;
    float rotX = 0.0f;
    float inputX;
    float inputZ;
    private GameObject player;
    public Transform target2;
    private Vector3 relativePos;
    Vector3 zoomCameraOffset =  new Vector3(0f, 0f, 0f);
    private float smoother = 0.2f;
    public float smoothingtime = 0.2f;
    private bool resettimer;
    Quaternion localRotation;
    Quaternion lastRotation;
    private bool resetRotation;
    Vector3 savedAngles;
    private GameObject boulder;
    private GameObject wall;
    private float bouldertimer;
    private bool turn;
    private bool stop;
    private float stoptimer;
    private float anglestoRecord1;
    private float anglestoRecord2;
    private bool stoprecording;
    private bool recordtimer;
    internal bool playingCutscene;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wall = GameObject.FindGameObjectWithTag("Front");

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        boulder = GameObject.FindGameObjectWithTag("Boulder");
        bouldertimer = 2f;
        stoptimer = 1f;
    }

    void Update()
    {
        // We setup the rotation of the sticks here
        //inputX = Input.GetAxis("RightStickHorizontal");
        //inputZ = Input.GetAxis("RightStickVertical");
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseX = -Input.GetAxis("Mouse X");
            mouseY = -Input.GetAxis("Mouse Y");
        } else
        {
            mouseX = - Input.GetAxis("Horizontal2");
            mouseY = Input.GetAxis("Vertical2");
        }
        if (resetRotation == false)
        {
            finalInputX = inputX + mouseX;
            finalInputZ = inputZ + mouseY;

            rotY -= finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        }
        if (resetRotation == true)
        {

            //finalInputX = savedAngles.x;
            //finalInputZ = savedAngles.z;
            if (savedAngles.x > 290f)
                rotX = 0;
            else
                rotX = savedAngles.x;
            rotY = savedAngles.y;
            //rotX = lastRotation.x;
            //transform.eulerAngles = savedAngles;
            transform.rotation = lastRotation;
            localRotation = lastRotation;


        }

        if (player.GetComponent<PlayerInputController>().targetlock == false)
        {
        
            resettimer = true;
            
            //transform.rotation = Quaternion.Lerp(transform.rotation, localRotation, Time.smoothDeltaTime * 1f);
            transform.rotation = localRotation;
            resetRotation = false;
        }
        //var relativePos = target2.position - transform.position;
        //Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        //Quaternion targetRotation = Quaternion.LookRotation(target2.transform.position - transform.position);

        // Smoothly rotate towards the target point.
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
        //transform.LookAt(target2);


        if (player.GetComponent<PlayerInputController>().targetlock == true)
        {
            
        
                resetRotation = true;
                relativePos = zoomCameraOffset + target2.position - player.transform.position;
            relativePos.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(relativePos);
                float distance = Vector3.Distance(target2.position, player.transform.position);


                
        
                targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
                recordtimer = false;
                targetRotation.Normalize();


                if (recordtimer == true && stoprecording == false)
                {
                    anglestoRecord1 = targetRotation.eulerAngles.y;
                    anglestoRecord2 = targetRotation.eulerAngles.z;
                    stoprecording = true;
                    recordtimer = false;
                }
            //if (smoother > 0)
            //{

                transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.smoothDeltaTime * 60f);
            //transform.rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetRotation.y, Time.smoothDeltaTime * 60f);
                savedAngles = transform.eulerAngles;
                lastRotation = transform.rotation;
                //smoother = smoother - Time.deltaTime;
                //}
                //else
                //{
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.smoothDeltaTime * 300f);
                //}

            
        }
        if (boulder != null)
        {
            if (boulder.GetComponent<BoulderKnock>().boulderscene == true && bouldertimer > 0)
            {
                resetRotation = true;
                relativePos = (boulder.transform.position + zoomCameraOffset) - player.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(relativePos);


                //if (smoother > 0) 
                
                //{

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.smoothDeltaTime * 10f);
                savedAngles = transform.eulerAngles;
                lastRotation = transform.rotation;
                bouldertimer -= Time.deltaTime;

            }



            if (bouldertimer < 0 && turn == false)
            {
                boulder.GetComponent<BoulderKnock>().boulderscene = false;
                turn = true;
            
            }

            if(turn == true && stop==false)
            {

                

                resetRotation = true;
                relativePos = (wall.transform.position + zoomCameraOffset) - player.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(relativePos);


                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.smoothDeltaTime * 10f);
                savedAngles = transform.eulerAngles;
                lastRotation = transform.rotation;
                stoptimer -= Time.deltaTime;

            }

            if(stoptimer < 0)
            {
                stop = true;
            }
        }
        //if(resettimer == true)
        //{
        //smoother = 0.2f;
        //resettimer = false;
        //}
    }

    void LateUpdate()
    {
        
        CameraUpdater();
        // if (timer < duration)
        // {
        //     transform.localPosition += Random.insideUnitSphere * 200;
        // }
    }

    void CameraUpdater()
    {
        // set the target object to follow
        Transform target = CameraFollowObj.transform;
        
        //move towards the game object that is the target
        float step = CameraMoveSpeed * Time.deltaTime;
 

        lastypos = target.position.y;

        transform.localPosition = Vector3.MoveTowards(transform.position, target.position, step);
       
    }

    internal IEnumerator CameraShaker(float duration = 2)
	{
		Vector3 originalPos = transform.localPosition;
		float timer = 0;
		while (timer < duration)
		{
			transform.localPosition = originalPos + Random.insideUnitSphere * 200;
			timer += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}
}