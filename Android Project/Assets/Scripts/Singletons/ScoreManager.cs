using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    #region Singleton

    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("ScoreManager already exists");
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    #endregion

    
    private List<int> highScore;
    public int GetHighScore()
    {
        int levelNumber = EventManager.Instance.currentLevel;
        return highScore[levelNumber];
    }
    
    public void SetHighScore(int value)
    {
        int levelNumber = EventManager.Instance.currentLevel;
        highScore[levelNumber] = value;
        PlayerPrefs.SetInt("highScore" + levelNumber, value);
        OnScoreChanged?.Invoke();
    }
    
    private List<int> currentScore;
    
    public int GetCurrentScore()
    {
        int levelNumber = EventManager.Instance.currentLevel;
        return currentScore[levelNumber];
    }
    
    public void SetCurrentScore(int value)
    {
        int levelNumber = EventManager.Instance.currentLevel;
        currentScore[levelNumber] = value;
        PlayerPrefs.SetInt("currentScore" + levelNumber, value);
        CheckAchievements(value);
        OnScoreChanged?.Invoke();
    }

    private void CheckAchievements(int value)
    {
        if(value > 10)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_score_over_10, 100, b => { });
        if(value > 25)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_score_over_25, 100, b => { });
        if(value > 50)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_score_over_50, 100, b => { });
        if(value > 75)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_score_over_75, 100, b => { });
        if(value > 100)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_score_over_100, 100, b => { });
        if(value > 150)
            Social.ReportProgress(ChaosInSpaceAchievements.achievement_dodge_master, 100, b => { });
    }

    public void IncrementCurrentScore(int value)
    {
        int levelNumber = EventManager.Instance.currentLevel;
        currentScore[levelNumber] += value;
        PlayerPrefs.SetInt("currentScore" + levelNumber, currentScore[levelNumber]);
        CheckAchievements(currentScore[levelNumber]);
        OnScoreChanged?.Invoke();
    }

    public Action OnScoreChanged;

    private void OnAwake()
    {
        highScore = new List<int> {0,0,0,0};
        currentScore = new List<int> {0,0,0,0};
    }

    private void Start()
    {
        for (int levelNumber = 1; levelNumber < EventManager.Instance.numLevels + 1; levelNumber++)
        {
            highScore[levelNumber] = PlayerPrefs.GetInt("highScore" + levelNumber);
        }
    }
}
