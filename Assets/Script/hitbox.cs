using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public float damage;

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("arrow"))
        {
            player.isParry = true;
        }
        if (collision.CompareTag("enemy")) 
        {
            player.Mana += player.manaGain; 
        
        }
    }

}