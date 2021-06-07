using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;

public class StudyInfoScript : MonoBehaviour
{

    [SerializeField] Button backOutBTN;
    int questionNumber = 0;
    int numOfQuestions;
    [SerializeField] GameObject inGameGEQ, socialPresenceGEQ;
    List<GameObject> inGameQuestions = new List<GameObject>();
    [SerializeField] GameObject finalQuestion;
    List<GameObject> socialPresenceQuestions = new List<GameObject>();
    IDictionary<string, object> results = new Dictionary<string, object>();
    IDictionary<string, object> resultsPart1 = new Dictionary<string, object>();
    IDictionary<string, object> resultsPart2 = new Dictionary<string, object>();
    IDictionary<string, object> resultsPart3 = new Dictionary<string, object>();
    int numOfEntreies = 0;
    [SerializeField] Button[] buttons;

    private void Awake()
    {
        for (int i = 0; i < inGameGEQ.transform.childCount; i++)
        {
            inGameQuestions.Add(inGameGEQ.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < socialPresenceGEQ.transform.childCount; i++)
        {
            socialPresenceQuestions.Add(socialPresenceGEQ.transform.GetChild(i).gameObject);
        }

        Cursor.lockState = CursorLockMode.None;
       
    }

    private void NextQuestion()
    {
        if(inGameGEQ.activeSelf)
        {
            inGameQuestions[questionNumber].SetActive(false);
            questionNumber++;
            if (questionNumber < inGameQuestions.Count)
            {
                inGameQuestions[questionNumber].SetActive(true);
                StartCoroutine(DisableButtons());
            }
            else if (questionNumber == inGameQuestions.Count && PlayerPrefs.GetInt("game") == 3)
            {
                finalQuestion.SetActive(true);
                StartCoroutine(DisableButtons());
            }
            else
            {
                finalQuestion.SetActive(false);

                if (PlayerPrefs.GetInt("inGameGEQ") <= 3)
                {
                     GameManager.instance.NextScene();
                }
            }

        }

        if (socialPresenceGEQ.activeSelf)
        {
            socialPresenceQuestions[questionNumber].SetActive(false);
            questionNumber++;
            socialPresenceQuestions[questionNumber].SetActive(true);
        }
    }

    public void LastQuestionBTN(int result)
    {
         string finalQString = "id: " + GameManager.instance.Id + ", Prefured round: " + result;
         AnalyticsResult analyticsResult = Analytics.CustomEvent(finalQString);
         GameManager.instance.NextScene();
    }

    public void ButtonPressed(int result) 
    {
        string eventString = "id: " + GameManager.instance.Id + ", Part" + PlayerPrefs.GetInt("game") + "- inGameGEQ_Q" + questionNumber + " = " + result;
        
        AnalyticsResult analyticsResult = Analytics.CustomEvent(eventString);
        if (inGameGEQ.activeSelf)
        {
            if(questionNumber <=5)
            {
                resultsPart1["inGameGEQ_Q" + questionNumber] = result;
            }

            if (questionNumber > 5 && questionNumber <= 10)
            {
                resultsPart2["inGameGEQ_Q" + questionNumber] = result;
            }

            if (questionNumber > 10)
            {
                resultsPart3["inGameGEQ_Q" + questionNumber] = result;
            }
            
            NextQuestion();
        }

    }

    //temp disable buttons after player has answered a question.
    IEnumerator DisableButtons()
    {
        foreach(Button btn in buttons)
        {
            btn.interactable = false;
        }
        yield return new WaitForSeconds(2);

        foreach (Button btn in buttons)
        {
            btn.interactable = true;
        }
    }
}
