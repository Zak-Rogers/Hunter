using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScript : MonoBehaviour
{

    [SerializeField] Slider chargeSlider;
    [SerializeField] TextMeshProUGUI score, time, game;
    
    void Start()
    {
        if(PlayerPrefs.GetInt("inGameGEQ") == 3)
        {
            PlayerPrefs.SetInt("game", 3);
        }
    }

    
    void Update()
    {
        chargeSlider.value = PlayerPrefs.GetFloat("charge");
        score.text = "Score: " + PlayerPrefs.GetInt("score");
        time.text = "Time Remaining: " + (int)PlayerPrefs.GetFloat("Time");
        game.text = "Round: " + PlayerPrefs.GetInt("game");
    }
}
