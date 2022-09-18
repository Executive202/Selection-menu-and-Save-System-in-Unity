using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectorController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objectsToSelect; // this will be the object prefabs holder
    public int ID; //integer to note the  current object number
    public TextMeshProUGUI uiText; //ui text
    string[] objectName = {"Skull","Bomb","American Football","Football" };
    Vector3 pos; //position
    public bool applySave; //since i created two scene and am using one script to handle it this enables the save function

    public GameObject toRotate;
    void Start()
    {
        //here we check if applysave boolean is true
        if(applySave)
        {
            //we get the objectcost from our saved script
            int cost;
            cost = PlayerSave.playerSaveInstance.objectCost[ID];
            uiText.text = objectName[ID].ToString() + " Cost : " + cost;
        }
        else
        {
            //go with the normal selection menu
            uiText.text = objectName[ID].ToString();
        }
        pos = objectsToSelect[ID].transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateCode();
    }
    void RotateCode()
    {
        //rotates the cylinder at which the objects are placed upon
        toRotate.transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
    public void prev()
    {
        //this will be the prev button
        int cost;
        //sets the current gameObject inactive
        objectsToSelect[ID].SetActive(false);
        //setting the position to the initial position that was saved, so that it always drops.
        objectsToSelect[ID].transform.position = pos;
        ID--;
        if(ID < 0) { ID = objectsToSelect.Length - 1; }
        //activates the object after we have id++
        objectsToSelect[ID].SetActive(true);
        if(applySave)
        {
            cost = PlayerSave.playerSaveInstance.objectCost[ID];
            uiText.text = objectName[ID].ToString() + " Cost : " + cost;
        }
        else
        {
            uiText.text = objectName[ID].ToString();
        }
        

    }
    public void next()
    {
        //next button
        int cost;
        objectsToSelect[ID].SetActive(false);
        objectsToSelect[ID].transform.position = pos;
        ID++;
        if (ID > objectsToSelect.Length - 1)
        {
            ID = 0;
        }
            objectsToSelect[ID].SetActive(true);
        if(applySave)
        {
            cost = PlayerSave.playerSaveInstance.objectCost[ID];
            uiText.text = objectName[ID].ToString() + " Cost : " + cost;
        }
        else
        {
            uiText.text = objectName[ID].ToString();
        }
    }
    public void Select()
    {
        //we handle the selection here
        if (applySave)
        {
            //if object cost is 0, that means its free or already bought
            //then allow player to select it.
            if (PlayerSave.playerSaveInstance.objectCost[ID] == 0)
            {
                uiText.text = "You selected " + objectName[ID].ToString();
            }
            else
            {
                //else if its not yet bought
                if (PlayerSave.playerSaveInstance.objectCost[ID] <= PlayerSave.playerSaveInstance.coins)
                {
                    //first check if we have sufficient balance the buy

                    PlayerSave.playerSaveInstance.coins -= PlayerSave.playerSaveInstance.objectCost[ID];
                    PlayerSave.playerSaveInstance.objectCost[ID] = 0;
                    PlayerSave.playerSaveInstance.UpdateCoin();
                    uiText.text = objectName[ID].ToString() + " Cost : 0";
                    PlayerSave.playerSaveInstance.Save();
                }
                else
                {
                    Debug.Log("You dont have enough coins");
                }
            }

        }
        else
        {
            uiText.text = "You selected " + objectName[ID].ToString();
        }
    }
}
