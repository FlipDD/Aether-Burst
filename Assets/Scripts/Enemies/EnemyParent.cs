using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    internal Rigidbody rgbd;
    private ParticleSystem psWater;
    private Vector3 goToPosition;
    bool startedMoving;

    public State state;
    public enum State
    {
        Idle,
        Frozen
    }

    void Start ()
    {
        rgbd = GetComponent<Rigidbody>();
        state = State.Idle;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
            break;

            case State.Frozen:
                if (!startedMoving)
                    StartCoroutine(Freeze());
                else
                    transform.position = Vector3.Lerp(transform.position, goToPosition, .05f);
            break;
        }

    }

    IEnumerator Freeze ()
    {
        startedMoving = true;
        goToPosition = transform.position + (Vector3.up * 6);
        Transform ps = Instantiate(GameAssets.i.waterPrison, transform.position, Quaternion.identity);
        ps.transform.parent = transform;
        rgbd.isKinematic = true;
        yield return new WaitForSeconds(7);
        Destroy(transform.GetChild(0).gameObject);
        rgbd.isKinematic = false;
        startedMoving = false;
        state = State.Idle;
    }

    public void Froze() => state = State.Frozen;

}
