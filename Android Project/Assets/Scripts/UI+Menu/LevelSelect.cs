using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Instance.currentLevel = 0;
    }

    public void PlayGame(int levelNumber)
    {
        EventManager.Instance.currentLevel = levelNumber;
        ScoreManager.Instance.SetCurrentScore(0);
        SceneManager.LoadScene("Level" + levelNumber);
        EventManager.Instance.gameplayCount++;
        AudioManager.instance.PlaySound("UI");
    }
}