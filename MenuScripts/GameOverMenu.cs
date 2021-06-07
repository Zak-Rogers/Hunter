using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{

    [SerializeField] GameObject gameOver, survey, mainMenu, roundOverTitle, gameOverTitle, thankYouTitle;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameObject nextRoundBTN, surveyBTN, mainMenuBTN, quitBTN;

    void Start()
    {
        score.text = "Score: " + PlayerPrefs.GetInt("score");
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("game") == 3)
        {
            roundOverTitle.SetActive(false);
            nextRoundBTN.SetActive(false);

            gameOverTitle.SetActive(true);
            mainMenuBTN.SetActive(true);
            quitBTN.SetActive(true);
            if(GameManager.instance.InStudy)
            {
                thankYouTitle.SetActive(true);
            }
        }
        
    }

    public void NextRound()
    {
        GameManager.instance.NextScene();
    }

    public void Survey()
    {
        gameOver.SetActive(false);
        survey.SetActive(true);
    }

    public void MainMenu()
    {
        gameOver.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
