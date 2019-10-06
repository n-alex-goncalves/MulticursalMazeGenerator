using UnityEngine;

public class CharacterMovement_Player_1: MonoBehaviour
{
    private Rigidbody2D rb;

    //Initizaliation of set distances for the character objec to travel.
    Vector2 x_distance = new Vector2(1.3f, 0);
    Vector2 y_distance = new Vector2(0, 1.3f);

    private void Start()
    {
        //Spawns the character object in the correct positon.
        GameObject Starting_Cell = GameObject.Find("Maze(Clone)/Cell - X:1 Y:1");
        GetComponent<Rigidbody2D>().position = new Vector2(Starting_Cell.transform.position.x, Starting_Cell.transform.position.y);
    }

    private void FixedUpdate()
    {
        //Basic IF statements to determine which key has been pressed.
        rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.A))
            rb.MovePosition(rb.position + -x_distance);
        if (Input.GetKey(KeyCode.D))
            rb.MovePosition(rb.position + x_distance);
        if (Input.GetKey(KeyCode.W))
            rb.MovePosition(rb.position + y_distance);
        if (Input.GetKey(KeyCode.S))
            rb.MovePosition(rb.position + -y_distance);
            
    }


}