using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

public class Items : MonoBehaviour
{
    [SerializeField] 
    public GameObject item;
    //public GameObject item;
    Action<string> _createItemsCallback;
    //public GameObject Item;
    // Start is called before the first frame update
    void Start()
    {
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };
        CreateItems();
    }

    // Update is called once per frame
   public void CreateItems()
    {
        string userId = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetItemsIDs(userId, _createItemsCallback));
    }
    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //parsing the Jsonarray string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        for(int i=0; i<jsonArray.Count; i++)
        {//create a few local variables
            bool isDone = false; //Are we done downloading the information
            string itemId = jsonArray[i].AsObject["itemID"];//getting the itemID from each array's row
            JSONObject itemInfoJson = new();
            //create a callback to get the information from the web.cs script
            Action<string> getItemInfoCallback = (itemInfo) =>
            {//this callback will be called from web.cs once the information of that specific item has been downloaded (isdone = true)
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;//the temparray only has one value
            };
            //wait until WEB.cs calls the callback we passed as parameter
            StartCoroutine(Main.Instance.Web.GetItem(itemId, getItemInfoCallback));
            //wait until the callback is called from WEB (info finished downloading)
            yield return new WaitUntil(() => isDone == true);
           
           //item = Instantiate(Resources.Load("Prefabs/item") as GameObject);
            //GameObject item = Instantiate(Item) as GameObject;
          
            item.transform.SetParent(this.transform);//so that it because a child of the view we are in, this way it will be spaced and scaled the way the other items are
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            Debug.Log(itemInfoJson["name"]);
          
            Debug.Log(itemInfoJson["price"]);
            Debug.Log(itemInfoJson["description"]);

            // fil lthe information that we downloaded inside our prefab
            item.transform.Find("Name").GetComponentInChildren<Text>().text = itemInfoJson["name"];
            item.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            item.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            //continue to the next Item
        }
     
    }
}
