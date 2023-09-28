using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable : MonoBehaviour
{

    public CollectableType type;

    public Sprite icon;

    //player walks into collectable
    //add collectable to player
    //delete collectable


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>(); //sees if the collision is from the player

        if (player)
        {
            
            player.Inventory.Add(this); //adds the collectable
            Destroy(this.gameObject);
        }
    }

}


public enum CollectableType
{
    NONE, CARROT_SEED, TEST, TURNIP
}