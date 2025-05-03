using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [NonSerialized]public static GameManager instance;


    [SerializeField] private UIMatchOver m_matchOverScreen;
    [SerializeField] private UIGameOver m_gameOverScreen;
    [SerializeField] private UITimer m_timerScreen;
    [SerializeField] private UIEnergyBar m_attackerEnergyBar;
    [SerializeField] private UIEnergyBar m_defenderEnergyBar;

    enum GameState : byte
    {
        WIN,
        LOOSE,
        DRAW,
    }

    public enum MatchState : byte
    {
        NONE,
        WIN,
        LOOSE,
        DRAW,
    }

    [SerializeField] private int m_numberOfMatches = 5;
    [SerializeField] private float m_timeLimit = 140.0f; //in seconds
    [SerializeField] static private int m_energyPoints = 6;
    private float m_attackerEnergy = 0.0f;
    private float m_defenderEnergy = 0.0f;

    private bool m_gamePaused = false;
    private float m_gameTimer;

    private MatchState m_currentMatchState;
    private GameState m_gameState;

    //need to save this in between matches
    private int m_attackerWinCount = 0;
    private int m_defenderWinCount = 0;
    private int m_currentMatch = 0;

    [NonSerialized]public bool isBallOccupied; //defines whether any of the attacker occupies the ball

    //private void initilize()
    //{
    //    m_gameTimer = 0.0f;
    //    m_currentMatchState = MatchState.NONE;
    //}

    //can call from other script
    public void AttackerWins()
    {
        m_currentMatchState = MatchState.WIN;
        HandleMatchState();
    }
    //can call from other script
    public void DefenderWins()
    {
        m_currentMatchState = MatchState.LOOSE;
        HandleMatchState();
    }

    //this function checks whether I Can spawn the attacker or not
    public bool CanAttackerSpawn()
    {
        if(m_attackerEnergy >= AttackerVariables.EnergyCost)
        {
            //I can spawn the attacker now deduce energy points
            m_attackerEnergy -= AttackerVariables.EnergyCost;
            return true;
        }
        return false;
    }

    //this function checks whether I Can spawn the defender or not
    public bool CanDefenderSpawn()
    {
        if(m_defenderEnergy >= DefenderVariables.EnergyCost)
        {
            m_defenderEnergy -= DefenderVariables.EnergyCost;
            return true;
        }
        return false;
    }

    public static int GetNumEnergyPoints()
    {
        return m_energyPoints;
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

    void HandleMatchState()
    {
        PauseGame();
        switch (m_currentMatchState)
        {
            case MatchState.WIN:
                m_attackerWinCount += 1;
                m_matchOverScreen.Activate("ATTACKER", m_attackerWinCount, m_defenderWinCount);
                break;
            case MatchState.LOOSE:
                m_defenderWinCount += 1;
                m_matchOverScreen.Activate("DEFENDER", m_attackerWinCount, m_defenderWinCount);
                break;
            case MatchState.DRAW:
                m_attackerWinCount+=1;
                m_defenderWinCount+=1;
                m_matchOverScreen.Activate(m_attackerWinCount, m_defenderWinCount);
                break;
        }
        PlayerPrefs.SetInt(MyUtils.ATTACKER_WIN_COUNT, m_attackerWinCount);
        PlayerPrefs.SetInt(MyUtils.DEFENDER_WIN_COUNT, m_defenderWinCount);
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
            case GameState.DRAW:
                m_gameOverScreen.Activate();
                break;
        }
    }

    public void NextMatchButton()
    {
        m_currentMatch += 1;
        PlayerPrefs.SetInt(MyUtils.MATCH_COUNT, m_currentMatch);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void TieBreakerMatchButton()
    {
        SceneManager.LoadScene("Maze", LoadSceneMode.Single);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        m_matchOverScreen.Deactivate();
        m_gameOverScreen.Deactivate();
        m_gameTimer = 0.0f;
        m_currentMatchState = MatchState.NONE;
        isBallOccupied = false;
        m_gamePaused = false;
        Time.timeScale = 1.0f;

        m_currentMatch = PlayerPrefs.GetInt(MyUtils.MATCH_COUNT, 0);
        m_attackerWinCount = PlayerPrefs.GetInt(MyUtils.ATTACKER_WIN_COUNT, 0);
        m_defenderWinCount = PlayerPrefs.GetInt(MyUtils.DEFENDER_WIN_COUNT, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGamePaused())
        {
            return;
        }
        m_gameTimer += Time.deltaTime;
        m_timerScreen.Activate(m_timeLimit - m_gameTimer); //render remaining time in ui

        m_attackerEnergy += AttackerVariables.EnergyRegeneration * Time.deltaTime;
        m_attackerEnergy = Mathf.Clamp(m_attackerEnergy, 0.0f, m_energyPoints);

        m_attackerEnergyBar.Activate(m_attackerEnergy/m_energyPoints); //render attacker energy progress bar

        m_defenderEnergy += AttackerVariables.EnergyRegeneration * Time.deltaTime;
        m_defenderEnergy = Mathf.Clamp(m_defenderEnergy, 0.0f, m_energyPoints);

        m_defenderEnergyBar.Activate(m_defenderEnergy / m_energyPoints); //render defender energy progress bar

        //game will end when we played "m_numberOfMatches" number of matches
        if (m_currentMatch == m_numberOfMatches)
        {
            if (m_attackerWinCount > m_defenderWinCount)
            {
                m_gameState = GameState.WIN;
            }
            else if (m_defenderWinCount > m_attackerWinCount)
            {
                m_gameState = GameState.LOOSE;
            }
            else
            {
                m_gameState = GameState.DRAW;
            }

            HandleGameState();
        }

        //if time limit is exceeded then it is a draw
        if (m_gameTimer >= m_timeLimit)
        {
            m_currentMatchState = MatchState.DRAW;
            HandleMatchState();
        }
    }
}
