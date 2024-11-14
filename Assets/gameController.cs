using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    Vector2 CheckPointPos;
    
    private void Start()
    {
       CheckPointPos = transform.position; 
    }

    private void OnTriggerEnter2D(Collider2D collision) // Changed 'enter' to 'Enter'
    {
        if (collision.CompareTag("obstacle"))
        {
            Die();
        }
    }

    void Die()
    {
        Respawn();
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        CheckPointPos = pos;
    }

    void Respawn()
    {
        transform.position = CheckPointPos;
    }
}
