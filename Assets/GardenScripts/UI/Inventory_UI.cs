using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;

    public GameObject player;
    public string inventoryName;

    public Movement move;

    public Animator animate;

    public List<Slot_UI> slots = new List<Slot_UI>();

    private Canvas canvas;

    private Slot_UI draggedSlot;
    private Image draggedIcon;

    private bool dragAll;

    private inventory Inventory;
   
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    void Start()
    {
        Inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetUpSlots();
        ToggleInventory();
        Refresh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) //if the player hits the tab button
        {
            ToggleInventory();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragAll = true;
           
        }
        else
        {
            dragAll = false;
           
        }
    }

    public void ToggleInventory() //turns on and off the inventory
    {
        if (inventoryPanel != null) //MAKE SURE TO LEAVE THE INVENTORY PANEL NULL FOR THE TOOLBAR //makes sure only the actual inventory toggles
        {



            if (!inventoryPanel.activeSelf) //if inventory panel is turned off
            {
                inventoryPanel.SetActive(true); //toggle it on
                player.GetComponent<Movement>().enabled = false; //Disables movement 
                animate.SetBool("isMoving", false);
                Refresh();
            }
            else //if inventory panel is turned on
            {
                inventoryPanel.SetActive(false); //toggle it off
                player.GetComponent<Movement>().enabled = true; //enables movement
                                                                //move.animator.SetBool("isMoving", true);
            }
        }
    }

    public void Refresh()
    {
        Debug.Log(Inventory.slots.Count + "player slots");
        Debug.Log(slots.Count + "Slots");
        if(slots.Count == Inventory.slots.Count) //if the player and the slots have the same number
        {
            //Debug.Log("TRUE TRUE TRUE");
            for(int i = 0; i<slots.Count; i++)
            {
                Debug.Log("iteration " + i + " of" + Inventory.slots.Count);
                Debug.Log("slot " + i + " type = " + Inventory.slots[i].itemName);
                if(Inventory.slots[i].itemName != "") //if the slot isn't an empty string
                    
                {
                    slots[i].SetItem(Inventory.slots[i]);
                    Debug.Log("Slot " + i + " is occupied");
                }
                else
                {
                    Debug.Log("Slot " + i + " is already empty");
                    slots[i].SetEmpty();
                    Debug.Log("Setting empty slot " + i);
                }

            }
        }
      
    }

    public void Remove()
    {
        Debug.Log("Inventory_UI remove has been called");
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(Inventory.slots[draggedSlot.SlotID].itemName); //gets the type of the item we are removing (problem here, everything but the turnip is null)
        //Debug.Log(itemToDrop.ToString()); //doesn't work
         
        if(itemToDrop != null)
        {
            Debug.Log("called item isn't null");
            if (dragAll == true)
            {
                GameManager.instance.player.DropItem(itemToDrop, Inventory.slots[draggedSlot.SlotID].numInSlot); //drops all items
                Inventory.Remove(draggedSlot.SlotID, Inventory.slots[draggedSlot.SlotID].numInSlot); //removes all items
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop); //drops a single item
                Inventory.Remove(draggedSlot.SlotID); //removes a single item
            }
            
            Refresh(); //refreshes inventory
        }
        draggedSlot = null;

    }


    public void SlotBeginDrag(Slot_UI slot)
    {
        //get slotID somewhere here
        UI_Manager.draggedSlot = slot;
        
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.raycastTarget = false; //so that it doesn't interfere with drag and drop

        draggedIcon.rectTransform.sizeDelta = new Vector2(50f, 50f);
        draggedIcon.transform.SetParent(canvas.transform); //makes the icon a child of the canvas so that it shows up properly
       
       

        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
        Debug.Log("Dragging: " + draggedSlot.name);
    }

    public void SlotEndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedIcon = null;


        //Debug.Log("Done Dragging: " + draggedSlot.name);
    }

    public void SlotDrop(Slot_UI slot)
    {
        //Debug.Log("Dropping:" + draggedSlot.name + " on " + slot.name); //drags draggedSlot onto Slot
        draggedSlot.Inventory.MoveSlot(draggedSlot.SlotID, slot.SlotID, slot.Inventory);
        Refresh();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if(canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position); //null is the camera

            toMove.transform.position = canvas.transform.TransformPoint(position); //moves the icon to mouse position

           
        }
    }

    private void SetUpSlots() //automatically sets the slotID so I dont need to manually do it
    {
        int counter = 0;
        foreach(Slot_UI slot in slots)
        {
            slot.SlotID = counter;
            counter++;
            slot.Inventory = Inventory;
        }
    }

  
}

