using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Vector2 movement;


    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        movement.x = 0f;
        movement.y = 0f;
        
        if (Input.GetKey(KeyCode.W))
            movement.y = 1f;
        if (Input.GetKey(KeyCode.S))
            movement.y = -1f;
        if (Input.GetKey(KeyCode.A))
            movement.x = -1f;
        if (Input.GetKey(KeyCode.D))
            movement.x = 1f;
        
        movement = movement.normalized;
    }
    
    void FixedUpdate()
    {
        moveCharacter(movement);
    }
    
    // 'moveCharacter' Function for moving the game object
    void moveCharacter(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
}
