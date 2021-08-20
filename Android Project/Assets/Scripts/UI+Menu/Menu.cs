using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Menu : MonoBehaviour
{
    public GameObject noAdsButton;
    private bool isUserAuthenticated = false;
    
    private void Start()
    {
        /*PlayGamesPlatform.Activate(); // activate playgame platform
        PlayGamesPlatform.DebugLogEnabled = true; //enable debug log
        AdManager.Instance.OnNoAdsPurchased += UpdateNoAdsButton;
        UpdateNoAdsButton();
        if (EventManager.Instance.gameplayCount % 2 == 0)
            AdManager.Instance.ShowAd();*/
    }

    private void Update()
    {
        if (!isUserAuthenticated){ 
            Social.localUser.Authenticate((bool success) => { 
                if (success){ 
                    Debug.Log("You've successfully logged in"); 
                    isUserAuthenticated = true; // set value to true
                } else {
                    Debug.Log("Login failed for some reason"); 
                } 
            });
        }
    }

    public void UpdateNoAdsButton()
    {
        noAdsButton.SetActive(!AdManager.Instance.NoAdsPurchased);
    }

    private void OnDestroy()
    {
        AdManager.Instance.OnNoAdsPurchased += UpdateNoAdsButton;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
        AudioManager.instance.PlaySound("UI");
    }

    public void OpenTwitter()
    {
        string twitterAddress = "http://twitter.com/intent/tweet";
        string message = "Download this awesome spade dodge em up game";
        string descriptionParameter = "Chaos in Space";
        string appStoreLink = "https://play.google.com/store/apps/details?id=com.JacobGallagher.ChaosinSpace";
        
        Application.OpenURL(twitterAddress + "?text=" + 
                            WWW.EscapeURL(
                                message + "\n" + descriptionParameter + "\n" + appStoreLink));
        AudioManager.instance.PlaySound("UI");
    }

    public void RemoveAdsPressed()
    {
        EventManager.Instance.RemoveAds();
        AudioManager.instance.PlaySound("UI");
    }

    public void OpenAchievements()
    {
        Social.localUser.Authenticate((bool success) =>
        {
                if (success)
                {
                    Debug.Log("You've successfully logged in");
                    Social.ShowAchievementsUI();
                }
                else
                {
                    Debug.Log("Login failed");
                }
        });
    }
}
