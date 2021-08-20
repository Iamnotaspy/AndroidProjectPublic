using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public enum ScoreToUpdate
    {
        HighScore,
        CurrentScore
    }

    public ScoreToUpdate scoreToUpdate;
    public int levelNumber;
    
    private void Start()
    {
        UpdateScoreText();
        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
    }

    private void UpdateScoreText()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = scoreToUpdate == ScoreToUpdate.CurrentScore
            ? PlayerPrefs.GetInt("currentScore" + levelNumber).ToString()
            : PlayerPrefs.GetInt("highScore" + levelNumber).ToString();
    }
}
