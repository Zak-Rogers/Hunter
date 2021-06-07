using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Level1, Level2, Level3, StudyFeedback}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    bool inStudy = false;
    bool doingSurvey = false;
    private float time;
    bool gameComplete = false;
    bool roundOver = false; 
    int id;

    int trackNumber = 0;
    AudioSource[] backgroundMusic;
    AudioSource currentTrack;

    [SerializeField] float startTime = 180.0f;
    private int sceneIndex = 0;


    public int Id
    {
        get { return id; }
    }

    public bool InStudy
    {
        get { return inStudy; }
        set { inStudy = value; }
    }
    public bool DoingSurvey
    {
        get { return doingSurvey; }
        set { doingSurvey = value; }
    }

    public bool RoundOver
    {
        get { return roundOver; }
        set { roundOver = value; }
    }

    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Pre_Load")
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            backgroundMusic = GetComponents<AudioSource>();
            currentTrack = backgroundMusic[trackNumber];
            currentTrack.Play();
            id = Random.Range(1, 100000);
            SceneManager.LoadSceneAsync("Menu");
        }
        
    }

    void Start()
    {
        PlayerPrefs.SetFloat("Time", startTime);
        sceneIndex = 0;
    }

    
    void Update()
    {
        if(sceneIndex > 0 && !roundOver) 
        {
            time = PlayerPrefs.GetFloat("Time");
            time -= Time.deltaTime;
            PlayerPrefs.SetFloat("Time", time);
        
            if(time <=0 && !doingSurvey)
            {
                time = startTime;
                PlayerPrefs.SetFloat("Time", time);
                NextScene();
            }
        }

        if(!currentTrack.isPlaying)
        {
            trackNumber++;
            if (trackNumber >= backgroundMusic.Length)
            {
                trackNumber = 0;
            }
            currentTrack = backgroundMusic[trackNumber];
            currentTrack.Play();
        }

    }


    public void NextScene()
    {
        if (inStudy)
        {
            switch (sceneIndex)
            {
                case 0:
                    sceneIndex++;
                    SetGameState(GameState.Level1);
                    break;
                case 1:
                    sceneIndex++;
                    doingSurvey = true;
                    SetGameState(GameState.StudyFeedback);
                    break;

                case 2:
                    sceneIndex++;
                    SetGameState(GameState.MainMenu);
                    roundOver = true;
                    break;
                case 3:
                    sceneIndex++;
                    doingSurvey = false;
                    roundOver = false;
                    time = startTime;
                    PlayerPrefs.SetFloat("Time", time);
                    SetGameState(GameState.Level2);
                    break;
                case 4:
                    sceneIndex++;
                    doingSurvey = true;
                    SetGameState(GameState.StudyFeedback);
                    break;

                case 5:
                    sceneIndex++;
                    SetGameState(GameState.MainMenu);
                    roundOver = true;
                    break;
                case 6:
                    sceneIndex++;
                    doingSurvey = false;
                    roundOver = false;
                    time = startTime;
                    PlayerPrefs.SetFloat("Time", time);
                    SetGameState(GameState.Level3);
                    break;
                case 7:
                    sceneIndex++;
                    doingSurvey = true;
                    SetGameState(GameState.StudyFeedback);
                    break;
                case 8:
                    SetGameState(GameState.MainMenu);
                    sceneIndex = 0;
                    roundOver = true;
                    break;
            }
        }
        else
        {
            doingSurvey = false;
            
            switch (sceneIndex)
            {
                case 0:
                    sceneIndex++;
                    SetGameState(GameState.Level1);
                    break;
                case 1:
                    sceneIndex++;
                    SetGameState(GameState.MainMenu);
                    roundOver = true;
                    break;
                case 2:
                    sceneIndex++;
                    roundOver = false;
                    SetGameState(GameState.Level2);
                    break;
                case 3:
                    sceneIndex++;
                    SetGameState(GameState.MainMenu);
                    roundOver = true;
                    break;
                case 4:
                    sceneIndex++;
                    roundOver = false;
                    SetGameState(GameState.Level3);
                    break;
                case 5:
                    SetGameState(GameState.MainMenu);
                    sceneIndex = 0;
                    roundOver = true;
                    break;
            }
        }
    }

    private void SetGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                SceneManager.LoadSceneAsync("Menu");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case GameState.Level1:
                SceneManager.LoadSceneAsync("SM");
                PlayerPrefs.SetInt("inGameGEQ", 1);
                PlayerPrefs.SetInt("game", 1);
                break;

            case GameState.Level2:
                SceneManager.LoadSceneAsync("BT");
                PlayerPrefs.SetInt("inGameGEQ", 2);
                PlayerPrefs.SetInt("game", 2);
                break;

            case GameState.Level3:
                SceneManager.LoadSceneAsync("ML");
                PlayerPrefs.SetInt("inGameGEQ", 3);
                
                break;
            case GameState.StudyFeedback:
                SceneManager.LoadSceneAsync("StudyFeedback"); 
                break;
        }
    }
}
