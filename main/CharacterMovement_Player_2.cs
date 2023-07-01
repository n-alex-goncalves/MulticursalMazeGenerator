using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_Player_2: MonoBehaviour
{
    private Rigidbody2D rb;

    Vector2 x_distance = new Vector2(1, 0);
    Vector2 y_distance = new Vector2(0, 1);

    private void Start()
    {
        GameObject Starting_Cell = GameObject.Find("Maze/Cell - X:1 Y:1");
        GetComponent<Rigidbody2D>().position = new Vector2(Starting_Cell.transform.position.x, Starting_Cell.transform.position.y);
    }

    private void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.LeftArrow))
            rb.MovePosition(rb.position + -x_distance);
        if (Input.GetKey(KeyCode.RightArrow))
            rb.MovePosition(rb.position + x_distance);
        if (Input.GetKey(KeyCode.UpArrow))
            rb.MovePosition(rb.position + y_distance);
        if (Input.GetKey(KeyCode.DownArrow))
            rb.MovePosition(rb.position + -y_distance);

    }


}