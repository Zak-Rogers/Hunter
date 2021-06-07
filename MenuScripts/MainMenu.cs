using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject mainMenu, studyInfo, controls, credits, gameOver;
    [SerializeField] Button comfirmButton;

    private void Awake()
    {
        if(GameManager.instance.RoundOver)
        {
            gameOver.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void StartGame()
    {
        GameManager.instance.NextScene();
        GameManager.instance.RoundOver = false;
        GameManager.instance.DoingSurvey = false;
    }

    public void StudyInfo()
    {
        mainMenu.SetActive(false);
        studyInfo.SetActive(true);
    }

    public void ComfirmParticipitation()
    {
        GameManager.instance.InStudy = true;
        comfirmButton.image.color = Color.green;
    }

    public void Controls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
