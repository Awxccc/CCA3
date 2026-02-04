//PFLBManager.cs
using PlayFab; //for playfab sdk
using PlayFab.ClientModels; //for playfab client api
using System.Collections.Generic; //for list
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PFLBManager : MonoBehaviour
{
    [SerializeField] TMP_InputField if_score;
    [SerializeField] TMP_Text t_msg, t_leaderboard, t_titleData;

    public void ClientGetTitleData()
    { //non arrow function
        //PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnGTDSucc, OnGTDFail);
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
result => {
    if (result.Data == null || !result.Data.ContainsKey("MOTD")) Debug.Log("No MOTD found");
    else Debug.Log("MOTD: " + result.Data["MOTD"]);
},
error => {
    Debug.Log("Got error getting titleData:");
    Debug.Log(error.GenerateErrorReport());
}
);
    }
    /*
    void OnGTDSucc(GetTitleDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("MOTD")) Debug.Log("No MOTD found");
        else Debug.Log("MOTD: " + result.Data["MOTD"]);
    }
    void OnGTDFail(PlayFabError error)
    {
        Debug.Log("Got error getting titleData:");
        Debug.Log(error.GenerateErrorReport());
    }*/
    void Start()
    {
        ClientGetTitleData();
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), DisplayTitleData, OnError);
    }

    public void OnSubmitScore()
    { //to submit score to leaderboard
        var updStatReq = new UpdatePlayerStatisticsRequest
        { //update playstat request
            Statistics = new List<StatisticUpdate>{ //list of statistic to send
new StatisticUpdate{ //statistic update object
StatisticName="highscore", //leaderboard name
Value=int.Parse(if_score.text) //value to update for current login player
}
}
        };
        try
        {
            PlayFabClientAPI.UpdatePlayerStatistics(updStatReq, UpdStatSucc, OnError);
        }
        catch (System.Exception e)
        {
            t_msg.text = "Exception:" + e.Message;
        }
    }
    void UpdStatSucc(UpdatePlayerStatisticsResult result)
    { //on success
        t_msg.text = "Score [" + if_score.text + "] Submitted!";
    }
    void OnError(PlayFabError error)
    { //if unsuccessful
        t_msg.text = "Error Submitting Score!" + error.GenerateErrorReport();
    }
    public void OnGetLeaderboard()
    { //to get leaderboard
        var getLBReq = new GetLeaderboardRequest
        { //get leaderboard request
            StatisticName = "highscore", //leaderboard name
            StartPosition = 0, //from top
            MaxResultsCount = 10 //top 10
        };
        try
        {
            PlayFabClientAPI.GetLeaderboard(getLBReq, GetLBSucc, OnError);
        }
        catch (System.Exception e)
        {
            t_msg.text = "Exception:" + e.Message;
        }
    }
    void GetLBSucc(GetLeaderboardResult result)
    { //on success
        t_msg.text = "Leaderboard Retrieved!";
        t_leaderboard.text = "--- Leaderboard ---\n";
        foreach (var item in result.Leaderboard)
        { //iterate through each leaderboard item
            t_leaderboard.text += item.Position + 1 + ". " + item.DisplayName + " : " + item.StatValue + "\n";
        }
    }

    void DisplayTitleData(GetTitleDataResult result)
    {
        t_titleData.text = "--- Title Data ---\n";
        foreach (var item in result.Data)
        { //iterate through each title data item
            t_titleData.text += item.Key + " : " + item.Value + "\n";
        }
    }

    public void OpenCatalog()
    {
        SceneManager.LoadScene("CatalogScn");
    }
    public void OpenFriendList()
    {
        SceneManager.LoadScene("FriendsScn");
    }
    public void OpenPlay()
    {
        SceneManager.LoadScene("Flappy Bird");
    }
}