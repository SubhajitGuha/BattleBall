using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGameManager : MonoBehaviour
{
    [NonSerialized] public static MazeGameManager instance;

    [SerializeField] private UIGameOver m_gameOverScreen;
    [SerializeField] private UITimer m_timerScreen;

    enum GameState : byte
    {
        WIN,
        LOOSE,
    }

    [SerializeField] private float m_timeLimit = 140.0f; //in seconds

    private bool m_gamePaused = false;
    private float m_gameTimer;

    private GameState m_gameState;

    //can call from other script
    public void AttackerWins()
    {
        m_gameState = GameState.WIN;
        HandleGameState();
    }

    public bool IsGamePaused()
    {
        return m_gamePaused;
    }

    private void PauseGame()
    {
        m_gamePaused = true;
        Time.timeScale = 0.0f;
    }

    private void ResumeGame()
    {
        m_gamePaused = false;
        Time.timeScale = 1.0f;
    }

    void HandleGameState()
    {
        PauseGame();
        switch (m_gameState)
        {
            case GameState.WIN:
                m_gameOverScreen.Activate("ATTACKER");
                break;
            case GameState.LOOSE:
                m_gameOverScreen.Activate("DEFENDER");
                break;
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        m_gameOverScreen.Deactivate();
        m_gameTimer = 0.0f;
        m_gamePaused = false;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGamePaused())
        {
            return;
        }
        m_gameTimer += Time.deltaTime;

        m_timerScreen.Activate(m_timeLimit - m_gameTimer);
        if (m_gameTimer >= m_timeLimit)
        {
            //if time expires attacker loses
            m_gameState = GameState.LOOSE;
            HandleGameState();
        }
    }
}
