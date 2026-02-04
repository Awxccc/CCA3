using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Collections;
public class PFUserMgt : MonoBehaviour //EK: Unity Code to Register new user in playfab
{
    [SerializeField] TMP_Text msgbox;
    [SerializeField] TMP_InputField if_regdisplayname, if_regusername, if_regemail, if_regpassword;
    public void OnBtnRegUser()
    { //reg button click handler
        var reqReq = new RegisterPlayFabUserRequest
        { //create request objct
            Email = if_regemail.text, //object fields
            Password = if_regpassword.text,
            Username = if_regusername.text //no comma
        };
        //execute request by calling reg playfab user api
        PlayFabClientAPI.RegisterPlayFabUser(reqReq, OnRegSucc, OnError);
    }
    void OnRegSucc(RegisterPlayFabUserResult r)
    { //function to handle success
        msgbox.text = "Registration Success! Assigned ID:" + r.PlayFabId;
        SetDisplayName();
    }
    void OnError(PlayFabError e)
    { //function to handle failure
        msgbox.text = "Error:" + e.GenerateErrorReport();
    }
    public void SetDisplayName()
    { //set display name button click handler
        var reqReq = new UpdateUserTitleDisplayNameRequest
        { //create request object
            DisplayName = if_regdisplayname.text //object field
        };
        //execute request by calling update user title display name api
        PlayFabClientAPI.UpdateUserTitleDisplayName(reqReq, OnSetDispNameSucc, OnError);
    }
    void OnSetDispNameSucc(UpdateUserTitleDisplayNameResult r)
    { //function to handle success
        msgbox.text += "\nDisplay Name Set!";
    }
    public void OnBtnLogin()
    { //login button click handler using username and password
        var reqReq = new LoginWithPlayFabRequest
        { //create request object
            Username = if_regusername.text, //read username and password from input fields
            Password = if_regpassword.text //no comma
        };
        //execute request by calling login with email address api
        PlayFabClientAPI.LoginWithPlayFab(reqReq, OnLoginSucc, OnError);
    }
    void OnLoginSucc(LoginResult r)
    { //function to handle login success
        msgbox.text = "Login Success! Last login time:" + r.LastLoginTime;
        StartCoroutine(LoadSceneAfterDelay("NextScn", 1f)); //some delay
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds); //wait for a while
        SceneManager.LoadScene(sceneName); //remember to put using UnityEngine.SceneManagement;
    }
    public void OnBtnLogout()
    { //logout button click handler
        PlayFabClientAPI.ForgetAllCredentials(); //clear all playfab login credentials
        SceneManager.LoadScene("MainMenuScn"); //load main scene
    }
    public void OnResetPassword()
    { //reset password button click handler
        var reqReq = new SendAccountRecoveryEmailRequest
        { //create request object
            Email = if_regemail.text, //read email from input field
            TitleId = PlayFabSettings.TitleId //set title id from playfab settings
        };
        //execute request by calling send account recovery email api
        PlayFabClientAPI.SendAccountRecoveryEmail(reqReq, OnResetPassSucc, OnError);
    }
    void OnResetPassSucc(SendAccountRecoveryEmailResult r)
    { //function to handle success
        msgbox.text = "Password reset email sent!";
    }
}