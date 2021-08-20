using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    #region Singleton

    public static AdManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("AdManager already exists");
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        OnAwake();
    }
    #endregion
    
    public bool testMode = true;
    private const string GameId = "4098047";
    private bool noAdsPurchased;

    public bool NoAdsPurchased
    {
        get => noAdsPurchased;
        set
        {
            noAdsPurchased = value;
            OnNoAdsPurchased?.Invoke();
        }
    }

    public Action OnNoAdsPurchased;

    private void OnAwake()
    {
        noAdsPurchased = PlayerPrefs.GetInt("noAdsPurchased") == 1;
    }

    private void Start()
    {
        OnNoAdsPurchased?.Invoke();
        Advertisement.Initialize(GameId, testMode);
    }

    public void ShowAd()
    {
        if(noAdsPurchased) return;
        const string placementId = "video";
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId, new ShowOptions(){ resultCallback = adViewResult});
            return;
        }
        Debug.Log("Ad not ready at the moment");
    }

    public void ShowRewardedAd()
    {
        const string rewardedPlacementId = "rewardedVideo";
        if (Advertisement.IsReady())
        {
            Advertisement.Show(rewardedPlacementId, new ShowOptions(){ resultCallback = adViewResult});
            return;
        }
        Debug.Log("Rewarded Ad not ready at the moment");
    }
    
    public void adViewResult(ShowResult result) { 
        switch (result) { 
            case ShowResult.Finished: 
                Debug.Log(" Player viewed complete Ad"); 
                break; 
            case ShowResult.Skipped: 
                Debug.Log(" Player Skipped Ad "); break; 
            case ShowResult.Failed: 
                Debug.Log("Problem showing Ad "); break;
        }
    }
}