using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginManager : MonoBehaviour {

	void OnEnable()
    {
        PlayFabManager.LoginCompleted += LoginComplete;
        PlayFabManager.LoginFailed += LoginFailed;
    }

    void OnDisable()
    {
        PlayFabManager.LoginCompleted -= LoginComplete;
        PlayFabManager.LoginFailed -= LoginFailed;
    }

    public void LoginAsGuest()
    {
        PlayFabManager.Instance.GuestLogin();
    }

    public void LoginWithFacebook()
    {
        FacebookManager.Instance.FBLogin();
    }

    void LoginComplete()
    {
        Debug.Log("Successfully login to PlayFab!");
        SceneManager.LoadScene(1);
    }

    void LoginFailed()
    {
        Debug.LogAssertion("Login Failed...");
    }
}
