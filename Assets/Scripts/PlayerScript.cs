using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour {

    public float force;
    public float forceV;
    public int health;
    public int maxHealth;
    private Rigidbody2D rigi;
    public float rotationSpeed;
    public Transform dir;
    private Animator anim;
    public float fuel = 100;
    public float fuelConsumption = 0.2f;
    public Slider slider;
    public Slider healthBar;
    private AudioSource[] soundManag;
    private AudioSource boostAudio;
    private AudioSource explosionAudio;

    [SerializeField]
    private AudioClip explosionClip;

    [SerializeField]
    private AudioClip boostClip;

    [SerializeField]
    private bool mobileJoy;

    [SerializeField]
    private GameObject gameMan;

    [SerializeField]
    private GameObject joystick;

    private GameManagerControl game;

    private float gravity;
    

	// Use this for initialization
	void Start () {
        this.rigi = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        soundManag = GetComponents<AudioSource>();

        boostAudio = soundManag[0];
        boostAudio.clip = boostClip;
        explosionAudio = soundManag[1];
        explosionAudio.clip = explosionClip;

        slider.maxValue = fuel;
        this.gravity = this.rigi.gravityScale;
        game = gameMan.GetComponent<GameManagerControl>();

#if UNITY_EDITOR
        mobileJoy = false;
#endif
        if (!mobileJoy)
        {
            joystick.SetActive(false);
        }
        
		
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = fuel;




    }

    private void FixedUpdate()
    {
        healthBar.value = (health * 100) / maxHealth;

        float moveHorizontal;
        float moveVertical;
        if (this.mobileJoy)
        {
            moveHorizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            moveVertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        


        rigi.angularVelocity = -moveHorizontal * rotationSpeed;



        if (mobileJoy)
        {
            JoystickMovement();
        }
        else
        {
            ManualMovement();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Respawn")
        {
            Debug.Log("gg");
            StartCoroutine(DeathAnim());
        }
        
    }

    private IEnumerator DeathAnim()
    {
        rigi.gravityScale = 0;
        rigi.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<PolygonCollider2D>().enabled = false;

        explosionAudio.Play();
        anim.SetBool("Exploded", true);
        //Reproducir sonido

        yield return new WaitForSeconds(2f);
        game.GameOver();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(rigi.velocity.magnitude);

        if (collision.tag == "Finish")
        {
            if (rigi.velocity.magnitude <= 6.5f)
            {
                this.game.Win();
            }
            else
            {
                StartCoroutine(DeathAnim());
            }
        }
        else if (collision.transform.tag == "Bullet")
        {
            if (health > 1)
            {
                health -= 1;
            }
            else
            {
                StartCoroutine(DeathAnim());
            }

        }

    }

    private void JoystickMovement()
    {
        if (CrossPlatformInputManager.GetButton("Boost"))
        {
            boostAudio.volume = 0.6f;
            boostAudio.Play();
        }
        if (CrossPlatformInputManager.GetButton("Boost") && fuel > 0)
        {

            rigi.velocity = transform.up * force*Time.deltaTime;
            anim.SetBool("PowerOn", true);
            fuel -= fuelConsumption;
        }
        else
        {
            boostAudio.volume = 0.6f;
            boostAudio.Stop();
            anim.SetBool("PowerOn", false);
            rigi.gravityScale = gravity;
        }
    }

    private void ManualMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boostAudio.volume = 0.6f;
            boostAudio.Play();
        }
        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            
            rigi.velocity = transform.up * force*Time.deltaTime ;
            anim.SetBool("PowerOn", true);
            fuel -= fuelConsumption;
        }
        else
        {
            boostAudio.volume = 0.6f;
            boostAudio.Stop();
            anim.SetBool("PowerOn", false);
            rigi.gravityScale = gravity;
        }
    }
}
