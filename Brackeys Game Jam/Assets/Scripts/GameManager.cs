using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transition;

    public GameObject cam;
    public GameObject winPanel;

    public float speed;
    public Transform target;
    private Transform defaultTarget;
    public GameObject barExplosionEffect;

    public GameObject[] enemies;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Boss")
        {
            GetComponent<AudioSource>().Play();
        }

        transition.gameObject.SetActive(true);

        defaultTarget = target;
    }

    void TransitionClose()
    {
        transition.SetTrigger("Close");
    }

    public void Death()
    {
        TransitionClose();
        Invoke("ResetScene", 2f);
    }

    public IEnumerator MoveCamera()
    {

        foreach (GameObject e in enemies)
        {
            if (e.gameObject != null)
            {
                e.GetComponent<Enemy>().speed = 0;
            }
        }

        cam.GetComponent<CamerFollow>().enabled = false;

        while (cam.GetComponent<CamerFollow>().enabled == false)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(target.position.x, target.position.y, cam.transform.position.z), speed * Time.deltaTime);

            if(Vector2.Distance(cam.transform.position, target.position) < .2f)
            {
                target.gameObject.SetActive(false);
                FindObjectOfType<AudioManager>().Play("Explosion");
                Instantiate(barExplosionEffect, target.position, target.rotation);

                yield return new WaitForSeconds(2);

                cam.GetComponent<CamerFollow>().enabled = true;

                foreach (GameObject e in enemies)
                {
                    if (e.gameObject != null)
                    {
                        e.GetComponent<Enemy>().speed = 8;
                    }
                }
            }

            yield return null;
        }

        target = defaultTarget;
    }

    public IEnumerator Win()
    {

        GetComponent<AudioSource>().volume = 0;

        FindObjectOfType<AudioManager>().Play("LevelComplete");

        foreach  (GameObject e in enemies)
        {
            if (e.gameObject != null)
            {
                e.GetComponent<Enemy>().Death();
            }
        }

        winPanel.SetActive(true);

        yield return new WaitForSeconds(3);

        TransitionClose();

        yield return new WaitForSeconds(2);

        if (SceneManager.GetActiveScene().name == "Boss")
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
