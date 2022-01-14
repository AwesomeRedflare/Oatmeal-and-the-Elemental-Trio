using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;

    public int health = 3;

    public float distance;
    private bool inRange = false;

    public GameObject deathExplosion;
    public GameObject hitEffect;

    private Transform target;

    private Rigidbody2D rb;

    private void Start()
    {
        transform.Rotate(0, 0, Random.Range(0f, 360f));

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, target.position) < distance)
        {
            inRange = true;
        }
    }

    private void FixedUpdate()
    {
        if(inRange == true)
        {
            Vector2 direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.layer == 9)
        {
            FindObjectOfType<AudioManager>().Play("Bullet");

            if (gameObject.tag == "Fire" && col.gameObject.CompareTag("Water"))
            {
                health -= 3;
            }
            else if (gameObject.tag == "Water" && col.gameObject.CompareTag("Electric"))
            {
                health -= 3;
            }
            else if (gameObject.tag == "Electric" && col.gameObject.CompareTag("Fire"))
            {
                health -= 3;
            }
            else
            {
                Instantiate(hitEffect, transform.position, col.transform.rotation);
                health -= 1;
            }

            Destroy(col.gameObject);

            if (health <= 0)
            {
                if (FindObjectOfType<LevelOne>() != null)
                {
                    LevelOne lev = FindObjectOfType<LevelOne>();

                    lev.enemyCount -= 1;

                    if (lev.enemyCount == 0)
                    {
                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartCoroutine("MoveCamera");

                        Debug.Log("success");
                    }
                }

                FindObjectOfType<AudioManager>().Play("EnemyDestroy");
                FindObjectOfType<CamerFollow>().TriggerShake(.08f, .08f);

                Death();
            }

        }

    }

    public void Death()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
