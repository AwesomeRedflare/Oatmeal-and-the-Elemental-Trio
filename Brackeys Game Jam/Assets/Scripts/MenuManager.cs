using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Animator transition;

    public GameObject levelSelectPanel;
    public GameObject controlsPanel;

    public GameObject fireButton;
    public GameObject waterButton;
    public GameObject electricButton;
    public static bool isFireSaved = false;
    public static bool isWaterSaved = false;
    public static bool isElectricSaved = false;
    public static bool levelSelect;

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("MenuMusic");

        transition.gameObject.SetActive(true);

        if (isFireSaved && fireButton != null)
        {
            fireButton.GetComponent<Button>().enabled = false;
            fireButton.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (isWaterSaved && waterButton != null)
        {
            waterButton.GetComponent<Button>().enabled = false;
            waterButton.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (isElectricSaved && electricButton != null)
        {
            electricButton.GetComponent<Button>().enabled = false;
            electricButton.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }

        if(levelSelect == true && levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    void TransitionClose()
    {
        transition.SetTrigger("Close");
    }

    void TransitionOpen()
    {
        transition.SetTrigger("Open");
    }

    public void StartButton()
    {
        levelSelect = true;
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("StartButtonTransition");
    }

    IEnumerator StartButtonTransition()
    {
        TransitionClose();

        yield return new WaitForSeconds(.6f);

        levelSelectPanel.SetActive(true);

        yield return new WaitForSeconds(.4f);

        TransitionOpen();
    }

    public void ControlsButton()
    {
        levelSelect = true;
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("ControlsButtonTransition");
    }

    IEnumerator ControlsButtonTransition()
    {
        TransitionClose();

        yield return new WaitForSeconds(.6f);

        controlsPanel.SetActive(true);

        yield return new WaitForSeconds(.4f);

        TransitionOpen();
    }

    public void BackButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("BackButtonTransition");
    }

    IEnumerator BackButtonTransition()
    {
        TransitionClose();

        yield return new WaitForSeconds(.6f);

        levelSelectPanel.SetActive(false);
        controlsPanel.SetActive(false);

        yield return new WaitForSeconds(.4f);

        TransitionOpen();
    }

    public void FireStageButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("LoadToFireStage");
    }

    IEnumerator LoadToFireStage()
    {
        TransitionClose();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Fire");

    }

    public void WaterStageButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("LoadToWaterStage");
    }

    IEnumerator LoadToWaterStage()
    {
        TransitionClose();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Water");

    }

    public void BossStageButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("LoadToBossStage");
    }

    IEnumerator LoadToBossStage()
    {
        TransitionClose();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Boss");

    }

    public void ElectricStageButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("LoadToElectricStage");
    }

    IEnumerator LoadToElectricStage()
    {
        TransitionClose();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Electric");

    }

    public void MainMenuButton()
    {
        FindObjectOfType<AudioManager>().Play("Button");
        StartCoroutine("LoadToMainMenu");
    }

    IEnumerator LoadToMainMenu()
    {
        TransitionClose();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Main Menu");

    }
}
