using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    public float waitingTime;
    private float timer = 0;
    private Animator animator;
    private EnemyState state;


    [SerializeField]
    private GameObject laser;

	// Use this for initialization
	void Start () {
        this.state = EnemyState.WAITING;
        this.animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (laser.transform.eulerAngles.z > 70 && laser.transform.eulerAngles.z < 85)
        {
            this.state = EnemyState.SHOOTING;

        }
        switch (this.state)
        {
            case EnemyState.SHOOTING:
                this.Shoot();
                break;
            case EnemyState.MOVING:
                this.Move();
                break;
            case EnemyState.WAITING:
                this.Wait();
                break;
        }


    }

    void Shoot()
    {
        Debug.Log(transform.eulerAngles.z);
        laser.SetActive(false);
        laser.transform.eulerAngles = new Vector3(0, 0, 270f);
        animator.SetTrigger("Shoot");
        this.state = EnemyState.WAITING;
    }

    void Wait()
    {
        timer += Time.deltaTime;
        if(timer > (waitingTime - 0.5f))
        {
            animator.SetTrigger("Shoot");

        }
        if (timer > waitingTime)
        {
            laser.SetActive(true);
            timer = 0;
            this.state = EnemyState.MOVING;

        }
    }

    void Move()
    {
        laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, Quaternion.Euler(0, 0, 450), 1.5f * Time.deltaTime);

    }
}



public enum EnemyState
{
    SHOOTING,MOVING,WAITING
}
