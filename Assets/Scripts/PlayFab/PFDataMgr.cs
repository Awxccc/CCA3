using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor.PackageManager;

public class PFDataMgr : MonoBehaviour
{
    [SerializeField] TMP_Text xpDisplay;
    [SerializeField] TMP_InputField xpInput;

    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"XP", xpInput.text.ToString() }
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data XP");
            Debug.Log(error.GenerateErrorReport());
            });
    }
    public void GetUserData(string myPlayFabID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabID,
            Keys = null
        },
        result => {
            Debug.Log("Successfully got user data");
            if (result.Data != null || !result.Data.ContainsKey("XP"))
            {
                Debug.Log("No XP");
            }
            else
            {
                Debug.Log("XP: "+result.Data["XP"].Value);
                xpDisplay.text = "XP: " +result.Data["XP"].Value;
            }
        },
        error => {
            Debug.Log("Got error getting user data XP");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
