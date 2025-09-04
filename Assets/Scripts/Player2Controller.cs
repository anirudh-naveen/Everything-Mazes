using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float movementSpeed;
    float speedX, speedY;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
        }
    }
}
