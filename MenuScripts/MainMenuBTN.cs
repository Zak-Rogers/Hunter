using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBTN : MonoBehaviour
{

    [SerializeField] GameObject mainMenu, studyInfo, controls, credits, gameOver;

    private void Start()
    {
        if (GameManager.instance.RoundOver)
        {
            gameOver.SetActive(true);
            studyInfo.SetActive(false);
        }
    }

    public void MainMenu()
    {
        studyInfo.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(false);
        mainMenu.SetActive(true);
        GameManager.instance.RoundOver = false;
    }
}
