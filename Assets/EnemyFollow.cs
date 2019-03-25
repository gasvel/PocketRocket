using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour {

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject bulletRef;

    private GameObject pj;

    private float timer =0;
    public float waitingTime = 3;

	// Use this for initialization
	void Start () {
        pj = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(this.transform.position, pj.transform.position);
        if (distance <= 13)
        {
            this.AttackPlayer();
        }

    }

    void AttackPlayer()
    {
        timer += Time.deltaTime;
        float angle = Mathf.Atan2(pj.transform.position.y - transform.position.y, pj.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        if (transform.rotation.z < 0.4f)
        {
            transform.rotation = (Quaternion.Euler(new Vector3(0, 0, angle + 180)));
            if (timer > waitingTime)
            {
                GameObject bulletInstance = Instantiate(bullet, bulletRef.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
                Rigidbody2D bulletR = bulletInstance.GetComponent<Rigidbody2D>();
                timer = 0;
                Debug.Log(angle);
                bulletInstance.transform.eulerAngles = new Vector3(0, 0, angle);
                //                bulletR.velocity = bulletInstance.transform.forward * 20;
                //                Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            }
        }
    }
}
