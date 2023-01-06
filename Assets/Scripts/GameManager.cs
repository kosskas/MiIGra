using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;


public enum GameState {GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text scoreText;
    public TMP_Text endScoreText;
    public TMP_Text highScoreText;
    public TMP_Text heartsText;
    public TMP_Text QualityText;
    public TMP_Text enemyKills;
    public TMP_Text airText;
    public int currentAir = 100;
    private int score = 0;
    public int lives = 3;
    private int enemiesKilled = 0;
    public GameState currentGameState = GameState.GS_GAME;
    public Canvas inGameCanvas;
    public Image[] keysTab;
    public int keysFound = 0;
    public int keysToFound = 3;
    public TMP_Text timeText;
    private float timer = 0;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public const string keyHighScore = "HighScoreLelev1";
    public Slider slider;

    // Start is called before the first frame update
    //
    void Start()
    {
        heartsText.text = lives.ToString();
        QualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        InvokeRepeating("UpdateAir", 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        if (lives == 0)
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(instance.currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else if (instance.currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }

            
        }
        
    }
    private void UpdateTime()
    {
        timer += Time.deltaTime;
        timeText.text = string.Format("Time: {0:00}:{1:00}", ((int)timer / 60), ((int)timer % 60));
    }

    private void UpdateAir()
    {
        if(currentAir>100)
        {
            currentAir = 100;
        }
        currentAir -= 1;
        airText.text = currentAir.ToString();
    }

    void Awake()
    {
        
        instance = this;
        InGame();
        score = 0;
        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.grey;
        }
        if(!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
    }

    public void AddAir()
    {
        currentAir += 30;
    }
    public void AddKeys()
    {
        keysTab[keysFound++].color = Color.white;
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void AddHeart(int points)
    {
        lives += points;
        heartsText.text = lives.ToString();
    }
    public void AddKill()
    {
        enemiesKilled++;
        enemyKills.text = enemiesKilled.ToString();
    }

    void SetGameState(GameState newGameState)
    {
       if(newGameState == GameState.GS_LEVELCOMPLETED)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int highScore = 0;
            if (currentScene.name == "Level1")
            {
                highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore<score)
                {
                    PlayerPrefs.SetInt(keyHighScore, score);
                    highScore = score;

                }
            }
            endScoreText.text = string.Format("Your score: " + score);
            highScoreText.text = string.Format("The best score: " + highScore);
        }
        else if(newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
            Time.timeScale = 1;
        }
        else if (newGameState == GameState.GS_OPTIONS || newGameState == GameState.GS_PAUSEMENU)
        {
            Time.timeScale = 0;
            inGameCanvas.enabled = false;
        }
        else if (newGameState == GameState.GS_GAME_OVER)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            inGameCanvas.enabled = false;
        }
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);

    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnPlusButtonClicked()
    {
        QualitySettings.IncreaseLevel();
        QualityText.text= QualitySettings.names[QualitySettings.GetQualityLevel()];

    }
    public void OnMinusButtonClicked()
    {
        QualitySettings.DecreaseLevel();
        QualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void OnOptionsButtonClicked()
    {
        Options();
    }
    public void SetVolume(float vol)
    {
        AudioListener.volume = slider.value;
       // Debug.Log(vol);
    }
}
