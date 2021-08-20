using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    #region Singleton

    public static EventManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("EventManager already exists");
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    #endregion
    public bool gamePaused = true;
    [HideInInspector] public int gameplayCount;
    private IAPManager iapManager;
    public int currentLevel;
    public int numLevels = 3;

    private void OnAwake()
    {
        gamePaused = false;
        gameplayCount = 0;
        iapManager = new IAPManager();
    }

    private void Start()
    {
        iapManager.Start();
    }

    public void PlayerHit()
    {
        var currentScore = ScoreManager.Instance.GetCurrentScore();
        if (currentScore > ScoreManager.Instance.GetHighScore())
            ScoreManager.Instance.SetHighScore(currentScore);

        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.PlaySound("explosion");
    }

    public void RemoveAds()
    {
        iapManager.BuyProductID("removeads");
    }
}