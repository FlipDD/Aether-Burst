using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	[SerializeField]
	protected float moveSpeed = 1f;
	[SerializeField]
	protected float jumpForce;
	private Rigidbody rgbd;
	private PlayerInputController playerInputController;
	public float acc;
	private UIBar uiBar;
	internal IEnumerator letGo;
	public LayerMask whatIsWall;
	internal bool isTouching;
	private bool isWaiting;
	ContactPoint cp;

	private Transform iceWall;

	private Transform hand;

    void Start()
	{
		rgbd = GetComponent<Rigidbody>();
		playerInputController = GetComponent<PlayerInputController>();
		uiBar = GetComponent<UIBar>();
		hand = GameObject.Find("mixamorig:LeftHand").transform;
	}
 
	internal void Move(Vector3 movement)
	{
		if (!playerInputController.isGrounded) 
		{
			rgbd.velocity = new Vector3(rgbd.velocity.x, rgbd.velocity.y-(acc), rgbd.velocity.z);
			acc += .75f * Time.deltaTime;
			// moveSpeed = 33;
		} 
		else
			acc = 0;
		
		rgbd.AddForce(moveSpeed * movement * Time.deltaTime, ForceMode.VelocityChange);
	}

	public void DoubleJump(Vector3 movement, float breathUsed = 10)
	{
		Transform tf2 = Instantiate(GameAssets.i.windEffect, transform.position, Quaternion.Euler(90, 0, 0));
		if (movement.x != 0 || movement.z != 0)
			tf2.rotation = Quaternion.LookRotation(new Vector3(-transform.forward.x, -1f, -transform.forward.z), Vector3.up);
		
		tf2.GetComponent<ParticleSystem>().Play();
		Destroy(tf2.gameObject, 2.3f);
		acc = 0;

		rgbd.AddForce((Vector3.up*1.4f+movement).normalized * jumpForce * 110, ForceMode.Impulse);
		CheckForAbility(playerInputController.airEffect, breathUsed, 4);
		AudioManager.i.Play("WindJump", transform.position);
	}

	internal void Jump(float breathUsed = 5)
	{
		Transform tf1 = Instantiate(GameAssets.i.windEffect, transform.position, Quaternion.Euler(90, 0, 0));
		tf1.GetComponent<ParticleSystem>().Play();
		Destroy(tf1.gameObject, 1.5f);
		rgbd.AddForce((Vector3.up*2+(rgbd.velocity.normalized * .75f)).normalized * jumpForce * 70, ForceMode.Impulse);
		CheckForAbility(playerInputController.airEffect, breathUsed, 2);
		AudioManager.i.Play("WindJump", transform.position);
	}

	void CheckForAbility(GameObject ability, float breath, float breathSaving)
	{
		if (playerInputController.currentAbility != null)
		{
			if (playerInputController.currentAbility.name == ability.name)
				uiBar.DecreaseBreath(breath - breathSaving);
			else
				uiBar.DecreaseBreath(breath);
		}
	}


	internal void Dash(Vector3 dir)
	{
		rgbd.velocity = Vector3.zero;
		acc = 0;
		rgbd.AddForce(transform.forward * 1200, ForceMode.Impulse);
		Transform fireDash = Instantiate(GameAssets.i.fireDash, transform.position + (transform.forward * .2f), Quaternion.identity);
		fireDash.parent = hand;
		fireDash.localPosition = Vector3.zero;
		fireDash.rotation = Quaternion.LookRotation(new Vector3(-transform.forward.x, 0, -transform.forward.z), transform.up);
		Destroy(fireDash.gameObject, .7f);
		CheckForAbility(playerInputController.fireEffect, 13, 6);
		AudioManager.i.Play("FireDash", transform.position);
	}

	void OnCollisionStay (Collision col)
	{
		if (col.gameObject.CompareTag("Wall"))
		{
			AudioManager.i.Play("Land", transform.position);
			isTouching = true;
			if (playerInputController.pressedJump && !isWaiting)
			{
				StartCoroutine(WallJumpFrequency());
				cp = col.GetContact(0);
				Vector3 dir = (cp.normal-transform.position).normalized;
					rgbd.AddForce(-cp.normal * 100);
				// if (dir.z > 0) playerInputController.animator.SetTrigger("LeftWall");
				// else playerInputController.animator.SetTrigger("RightWall");

				if (Physics.Raycast(transform.position, transform.right, 1))
					playerInputController.animator.SetTrigger("RightWall");
				else if (Physics.Raycast(transform.position, -transform.right, 1))
					playerInputController.animator.SetTrigger("LeftWall");
				else
					playerInputController.animator.SetTrigger("BackLWall");
				
			}
		}
	}
	
	internal void WallJump()
	{
		acc = 0;
		// rgbd.velocity = new Vector3(rgbd.velocity.x, 0f, rgbd.velocity.z);
		rgbd.velocity = Vector3.zero;
		Vector3 spawnPoint = cp.point+(cp.normal*1.02f);
		Vector3 dir;
		if (playerInputController.movement != Vector3.zero)
		{
			dir = ((playerInputController.movement*.2f)+(cp.normal*1.45f)+(transform.up*1.4f)).normalized;
			// float dist = spawnPoint.x + tra
			iceWall = ObjectPooler.Instance.SpawnFromPool("IceWallPs", new Vector3(spawnPoint.x + (transform.position.x - spawnPoint.x), transform.position.y-1.25f, spawnPoint.z + (transform.position.z - spawnPoint.z)), Quaternion.LookRotation(cp.normal)).transform; 
		}
		else 
		{
			dir = ((cp.normal*1.45f)+(transform.up*1.6f)).normalized;
			iceWall = ObjectPooler.Instance.SpawnFromPool("IceWallPs", new Vector3(spawnPoint.x, transform.position.y-1.25f, spawnPoint.z), Quaternion.LookRotation(cp.normal)).transform;
		}
		
		iceWall.GetChild(0).GetComponentInChildren<IceCollision>().SetRotation(cp.normal);
		transform.rotation = Quaternion.LookRotation(cp.normal);
		rgbd.AddForce(dir * 1400, ForceMode.Impulse);
		playerInputController.hasJumped = true;
		CheckForAbility(playerInputController.waterEffect, 8, 4);
		// AudioManager.i.Play("WallJump");
	}

	void PlayWalking(int n)
	{
		if (n == 0)
			AudioManager.i.Play("Walk1", transform.position);
		else if (n == 1)
			AudioManager.i.Play("Walk2", transform.position);
		else if (n == 2)
			AudioManager.i.Play("Walk11", transform.position);
		else if (n == 3)
			AudioManager.i.Play("Walk22", transform.position);
	}

    void OnCollisionExit() => isTouching = false;

    private IEnumerator WallJumpFrequency()
    {
		isWaiting = true;
        yield return new WaitForSeconds(.5f);
		isWaiting = false;
    }
}