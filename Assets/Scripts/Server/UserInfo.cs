
using UnityEngine;

public class UserInfo : MonoBehaviour
{
     public string UserID { get; private set; }// this means that you can only set it privately, but you can publicly get the value
     string UserName;
     string UserPassword;
     string Level;
     string coins;

    public void setCredentials( string username, string userpassword)
    {
        UserName = username;
        UserPassword = userpassword;
    }
    public void setID(string id)
    {
        UserID = id;
    }
        
        
        
        }
