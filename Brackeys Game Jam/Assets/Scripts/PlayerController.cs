using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 moveVelocity;
    public float rotateSpeed;
    public float rotateAngle = 0;

    private Rigidbody2D rb;
    private Animator anim;
    public GameObject savedCharacter;

    public float health;
    private float targetHealth;
    public float healthSpeed;
    public float maxHealth;
    public Image healthBar;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public int enemyCount;

    private bool isHurt = false;
    public float invulnerableDuration;
    private float invulnerableCountDown;

    private GameManager gameManager;

    public static bool acquiredFire = false;
    public GameObject fire;
    public static bool acquiredWater = false;
    public GameObject water;
    public static bool acquiredElectric = false;
    public GameObject electric;

    private void Start()
    {
        targetHealth = health;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if(acquiredFire == true)
        {
            fire.SetActive(true);
        }

        if (acquiredWater == true)
        {
            water.SetActive(true);
        }

        if (acquiredElectric == true)
        {
            electric.SetActive(true);
        }
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveVelocity = moveInput.normalized * speed;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), rotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            rotateAngle += 90;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            rotateAngle -= 90;   
        }

        if (timeBtwShots <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                FindObjectOfType<AudioManager>().Play("Shoot");
                foreach (Transform c in transform)
                {
                    if (c.GetComponent<Character>() != null && c.gameObject.activeSelf == true)
                    {
                        c.GetComponent<Character>().Shoot();
                    }
                }
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        if (isHurt == true && invulnerableCountDown > 0)
        {
            invulnerableCountDown -= Time.deltaTime;
            anim.SetBool("Hurt", isHurt);
        }
        if (invulnerableCountDown <= 0)
        {
            isHurt = false;
            anim.SetBool("Hurt", isHurt);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.Death();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameManager.StartCoroutine("Win");
        }


        healthBar.fillAmount = health / maxHealth;
        health = Mathf.Lerp(health, targetHealth, healthSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Goal"))
        {
            gameManager.StartCoroutine("Win");

            if (SceneManager.GetActiveScene().name == "Fire")
            {
                MenuManager.isElectricSaved = true;
                acquiredElectric = true;
            }

            if (SceneManager.GetActiveScene().name == "Water")
            {
                MenuManager.isFireSaved = true;
                acquiredFire = true;
            }

            if (SceneManager.GetActiveScene().name == "Electric")
            {
                MenuManager.isWaterSaved = true;
                acquiredWater = true;
            }

            Destroy(col.gameObject);

            savedCharacter.SetActive(true);

            speed = 0;
        }

        if (col.CompareTag("Key"))
        {
            FindObjectOfType<AudioManager>().Play("Key");
            gameManager.StartCoroutine("MoveCamera");
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if((col.gameObject.layer == 8 || col.gameObject.layer == 10)&& isHurt == false)
        {
            FindObjectOfType<AudioManager>().Play("Hurt");

            FindObjectOfType<CamerFollow>().TriggerShake(.2f, .2f);

            isHurt = true;
            invulnerableCountDown = invulnerableDuration;
            targetHealth -= 5;
            Debug.Log(health);

            if (targetHealth <= 0)
            {
                foreach (Transform c in transform)
                {
                    if (c.GetComponent<Character>() != null && c.gameObject.activeSelf == true)
                    {
                        c.GetComponent<Character>().Death();
                    }
                }

                FindObjectOfType<AudioManager>().Play("Death");
                gameObject.SetActive(false);
                gameManager.Death();
            }
        }
    }
}
