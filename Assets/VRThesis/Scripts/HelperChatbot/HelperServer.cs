using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
class ErrorStatement
{
    public string errorstat;

    public ErrorStatement(string errorstat)
    {
        this.errorstat = errorstat;
    }
}

public class HelperServer : MonoBehaviour
{
    public Action<string> _chatCallBack;

    public void GetHelperResponse(string errorStatment)
    {
        StartCoroutine(GetBotReponse(errorStatment));
    }

    IEnumerator GetBotReponse(string text)
    {
        ErrorStatement errorStatement = new ErrorStatement(text);
        string jsonData = JsonUtility.ToJson(errorStatement);
        using (UnityWebRequest www = UnityWebRequest.Put(Constants.HELPER_SERVER_URI, jsonData))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                Debug.Log("Form upload complete!"+ response);
                if(_chatCallBack!=null)
                {
                    _chatCallBack(response);
                }
            }
        }
    }

}
