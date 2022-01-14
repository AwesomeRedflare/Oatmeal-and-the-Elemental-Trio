using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public float health;
    private float targetHealth;
    public float healthSpeed;
    public float maxHealth;
    public GameObject healthObject;

    public GameObject waterHitEffect;
    public GameObject ElectricHitEffect;
    public GameObject FireHitEffect;
    public GameObject bossDefeatedExplosion;

    public GameObject waterEnemy;
    public GameObject electricEnemy;
    public GameObject fireEnemy;
    private GameObject currentEnemy;
    public Transform spawnPointOne;
    public Transform spawnPointTwo;
    public float timeBtwSpawn;

    public float rotateSpeed;
    public float rotateAngle = 180;
    public GameObject body;
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;

    public Transform startPos;
    public Transform pointOne;
    public Transform pointTwo;
    public float speed;
    private Rigidbody2D rb;

    private bool destinationOne = false;
    private bool startMoving = false;

    public Image healthBar;

    public GameObject gameManager;

    private void Start()
    {
        currentEnemy = electricEnemy;
        rb = GetComponent<Rigidbody2D>();
        targetHealth = health;
    }

    private void Update()
    {
        if(startMoving == true)
        {
            if (destinationOne == true)
            {
                rb.transform.position = Vector2.MoveTowards(transform.position, pointOne.position, speed * Time.deltaTime);
            }
            else
            {
                rb.transform.position = Vector2.MoveTowards(transform.position, pointTwo.position, speed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, pointOne.position) < .5f)
            {
                destinationOne = false;
            }

            if (Vector2.Distance(transform.position, pointTwo.position) < .5f)
            {
                destinationOne = true;
            }
        }

        body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, Quaternion.Euler(0, 0, rotateAngle), rotateSpeed * Time.deltaTime);

        healthBar.fillAmount = health / maxHealth;
        health = Mathf.Lerp(health, targetHealth, healthSpeed * Time.deltaTime);
    }

    public IEnumerator Intro()
    {
        Debug.Log("check");

        while (Vector2.Distance(transform.position, startPos.position) > .1f && startMoving == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, startPos.position) < .1f)
            {
                yield return new WaitForSeconds(2);

                gameManager.GetComponent<AudioSource>().Play();
                healthObject.SetActive(true);
                GetComponent<CircleCollider2D>().enabled = true;
                startMoving = true;
                StartCoroutine("SpawnEnemies");
            }

            yield return null;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while(targetHealth > 0)
        {
            yield return new WaitForSeconds(timeBtwSpawn - Random.Range(-.5f, .5f));

            Instantiate(currentEnemy, spawnPointOne.position, Quaternion.Euler(0, 0, 180), gameManager.transform);

            yield return new WaitForSeconds(timeBtwSpawn - Random.Range(-.5f, .5f));

            Instantiate(currentEnemy, spawnPointTwo.position, Quaternion.Euler(0, 0, 180), gameManager.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {

            if(targetHealth > 33 && targetHealth < 66)
            {
                rotateAngle = 90;
                currentEnemy = waterEnemy;

                if(yellow.activeSelf == true)
                {
                    Instantiate(bossDefeatedExplosion, col.transform.position, transform.rotation);
                    yellow.SetActive(false);
                }

            } else if (targetHealth > 0 && targetHealth < 33)
            {
                rotateAngle = -90;
                currentEnemy = fireEnemy;

                if (blue.activeSelf == true)
                {
                    Instantiate(bossDefeatedExplosion, col.transform.position, transform.rotation);
                    blue.SetActive(false);
                }
            }

            if (targetHealth >= 66 && col.gameObject.CompareTag("Fire"))
            {
                targetHealth -= 3;
                Instantiate(ElectricHitEffect, col.transform.position, col.transform.rotation);
            }
            else if(targetHealth >= 66)
            {
                targetHealth -= 1;
                Instantiate(ElectricHitEffect, col.transform.position, col.transform.rotation);
            }

            if (targetHealth >= 33 && targetHealth < 66 && col.gameObject.CompareTag("Electric"))
            {
                targetHealth -= 3;
                Instantiate(waterHitEffect, col.transform.position, col.transform.rotation);
            }
            else if (targetHealth >= 33 && targetHealth < 66)
            {
                targetHealth -= 1;
                Instantiate(waterHitEffect, col.transform.position, col.transform.rotation);
            }

            if (targetHealth > 0 && targetHealth < 40 && col.gameObject.CompareTag("Water"))
            {
                targetHealth -= 3;
                Instantiate(FireHitEffect, col.transform.position, col.transform.rotation);
            }
            else if (targetHealth > 0 && targetHealth < 33)
            {
                Instantiate(FireHitEffect, col.transform.position, col.transform.rotation);
                targetHealth -= 1;
            }

            Destroy(col.gameObject);

            if (targetHealth <= 0)
            {
                gameManager.GetComponent<AudioSource>().volume = 0;
                FindObjectOfType<AudioManager>().Play("Explosion");
                FindObjectOfType<AudioManager>().Play("EnemyDestroy");

                FindObjectOfType<ShakeBehavior>().enabled = true;
                FindObjectOfType<ShakeBehavior>().TriggerShake(.3f, .3f);

                gameManager.GetComponent<GameManager>().StartCoroutine("Win");

                foreach (Transform c in gameManager.transform)
                {
                    if (c.GetComponent<Enemy>() != null)
                    {
                        c.GetComponent<Enemy>().Death();
                    }
                }
                Instantiate(bossDefeatedExplosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

                gameObject.SetActive(false);
                healthObject.SetActive(false);
            }
        }
    }
}
