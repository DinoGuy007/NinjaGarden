using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public Player player;
    public Movement move;

    public List<Slot_UI> slots = new List<Slot_UI>();
    void Start()
    {
        ToggleInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //if the player hits the tab button
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (!inventoryPanel.activeSelf) //if inventory panel is turned off
        {
            inventoryPanel.SetActive(true); //toggle it on
            player.GetComponent<Movement>().enabled = false; //need to set the bool "isMoving" to false"
            Setup();    
        }
        else //if inventory panel is turned on
        {
            inventoryPanel.SetActive(false); //toggle it off
            player.GetComponent<Movement>().enabled = true;
        }
    }

    public void Setup()
    {
        Debug.Log(player.Inventory.slots.Count + "player slots");
        Debug.Log(slots.Count + "Slots");
        if(slots.Count == player.Inventory.slots.Count) //if the player and the slots have the same number
        {
            Debug.Log("TRUE TRUE TRUE");
            for(int i = 0; i<slots.Count; i++)
            {
                Debug.Log("iteration " + i);
                Debug.Log("slot " + i + " type = " + player.Inventory.slots[i].type);
                if(player.Inventory.slots[i].type != CollectableType.NONE) 
                    
                {
                    slots[i].SetItem(player.Inventory.slots[i]);
                    Debug.Log("Slot " + i + " is occupied");
                }
                else
                {
                    slots[i].SetEmpty(); //error that doesn't matter here. It still function perfectly. "object reference not set to instance of object" doesn't matter
                    Debug.Log("Setting up slot " + i);
                }

            }
        }
        else
        {
            Debug.Log("Your code sucks idiot");
        }
    }

}