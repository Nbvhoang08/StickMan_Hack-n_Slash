using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesSight : MonoBehaviour
{
    // Start is called before the first frame update
    public Enemies enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(collision.GetComponent<Player>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(null);
        }
    }
}
