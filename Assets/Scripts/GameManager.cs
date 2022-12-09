using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum GameState {GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text scoreText;
    private int score = 0;
    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public Canvas inGameCanvas;
    public Image[] keysTab;
    public int keysFound = 0;
    public TMP_Text timeText;
    private float timer = 0;

    // Start is called before the first frame update
    //
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(instance.currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else //if (instance.currentGameState == GameState.GS_PAUSEMENU)
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

    void Awake()
    {
        instance = this;
        score = 0;
        for(int i = 0; i < keysTab.Length; i++)
        {
            keysTab[i].color = Color.grey;
        }
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

    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        }
        else
        {
            inGameCanvas.enabled = false;
        }
        currentGameState = newGameState;
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
}
