using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public IEnumerator createGuild(string name)
    {
        
            WWWForm form = new WWWForm();
        form.AddField("guildName", name);

        string uri = "http://localhost/unityserver/CreateGuild.php";
            using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        
        
    }
    // Update is called once per frame

    public IEnumerator RegisterUser(string username, string password, string confirmPass)
    {
        if (password.Equals(confirmPass))
        {
            WWWForm form = new WWWForm();
            form.AddField("loginUser", username);
            form.AddField("loginPass", password);
            string uri = "http://localhost/unityserver/signup.php";
            using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
        else Debug.Log("Passwords don't match");
    }
    public IEnumerator Login(string username, string password)//IEnumerator HAS to be called with coroutine
    {

        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        //connect.text = "Loading...";
        string uri = "http://localhost/unityserver/login.php";//the URI of the login
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form)) //post request needs a uri and a form of data
        {
            yield return request.SendWebRequest();//yield is necessary with coroutines, so as to wait for the response
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);//an error message is shown
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                //or success
                Main.Instance.UserInfo.setCredentials(username, password);
                Main.Instance.UserInfo.setID(request.downloadHandler.text);//set the current ID to the user who just logged in's ID
                //this if means we didn't login correctly, we put a small message to try again
                if ((request.downloadHandler.text.Contains("Wrong Password") ||
                    request.downloadHandler.text.Contains("Username does not exist")))
                {
                    Debug.Log("Try Agin");
                }
                else 
                { 
                    Main.Instance.UserProfile.SetActive(true);//show us the new view after we log in
                    Main.Instance.login.gameObject.SetActive(false);//turn off the login view
                }



            }
        }
    }
    public IEnumerator GetItemsIDs(string userID, System.Action<string> callback)
    {
        //sign the server needs some time to send a response, thats why we are using a callback
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);//the id of the person who holds some item for example youssef is ID 1 and he has the item "machete" which has an id of 2 so userID is 1 (Youssef)
        string uri = "http://localhost/unityserver/GetItemsIDs.php";
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
        
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);//an error message is shown
            }
            else
            {
                Debug.Log(request.downloadHandler.text);//or success
                string jsonArray = request.downloadHandler.text;
                //call callback function to pass results
                callback(jsonArray);
            }
        }
    }
    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        //sign the server needs some time to send a response, thats why we are using a callback
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        string uri = "http://localhost/unityserver/GetItem.php";
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {

            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);//an error message is shown
            }
            else
            {
                Debug.Log(request.downloadHandler.text);//or success
                string jsonArray = request.downloadHandler.text;
                //call callback function to pass results
                callback(jsonArray);
            }
        }
    }


}
