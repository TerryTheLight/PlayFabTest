﻿using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabManager : MonoBehaviour {

    public static event Action LoginCompleted;
    public static event Action LoginFailed;

    private static PlayFabManager _instance;
    public static PlayFabManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayFabManager>();

                if (_instance)
                    DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    #region Login

    //Login based on devices ID ( Does not require credentials information )
    public void GuestLogin()
    {
        LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            AndroidDevice = SystemInfo.deviceModel,
            OS = SystemInfo.operatingSystem,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerStatistics = true,
                GetUserData = true
            }
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginCompleted, OnLoginFailed);
    }

    public void FacebookLogin(string _fBAccessToken)
    {
        LoginWithFacebookRequest request = new LoginWithFacebookRequest()
        {
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId,
            AccessToken = _fBAccessToken,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerStatistics = true,
                GetUserData = true
            }
        };

        PlayFabClientAPI.LoginWithFacebook(request, OnLoginCompleted, OnLoginFailed);
    }

    void OnLoginCompleted(LoginResult _result)
    {
        if (LoginCompleted != null)
            LoginCompleted();
    }

    void OnLoginFailed(PlayFabError _error)
    {
        if (LoginFailed != null)
            LoginFailed();
    }

    #endregion

    #region PlayerData

    public void UpdateUserData(string _heroCurrency = null, string _energy = null, string _artifactPieces = null, string _playerProgress = null, string _heroLevels = null)
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>();

        if (_heroCurrency != null)
            playerData.Add(PlayFabKey.HeroCurrency, _heroCurrency);

        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = playerData
        };

        PlayFabClientAPI.UpdateUserData(request, (result) => {
            Debug.Log("Successfully updated player data");
        }, (error) => {
            Debug.LogAssertion("Failed to update player data");
        });
    }

    public void GetUserData(Dictionary<string, UserDataRecord> data)
    {
        UserDataRecord heroCurrency;

        if(data.TryGetValue(PlayFabKey.HeroCurrency, out heroCurrency))
        {
            Debug.Log("Success on getting hero currency = " + heroCurrency.Value);
        }
    }

    #endregion

    #region PlayerStat

    public void UpdateUserStat(int? _monsterKilled)
    {
        List<PlayFab.ServerModels.StatisticUpdate> playerStat = new List<PlayFab.ServerModels.StatisticUpdate>();

        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "updatePlayerStat",
            FunctionParameter = new { playerStats = playerStat }
        };
    }

    #endregion

    struct PlayFabKey
    {
        /// <summary>
        /// PlayFab Key for Hero Currency ( Player Data )
        /// </summary>
        public const string HeroCurrency = "HeroCurrency";

        /// <summary>
        /// PlayFab Key for Monsters Killed ( Player Stat )
        /// </summary>
        public const string MonsterKilled = "MonsterKilled";
    }
}
