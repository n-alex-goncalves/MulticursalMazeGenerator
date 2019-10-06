using UnityEngine;
using System.Collections;

public class ButtonNviagtion : MonoBehaviour
{
    int index = 0;
    public int total_index = 3;
    public float yOffset = 1f;

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.DownArrow)) {
            if (index < total_index - 1)
            {
                index++;
                Vector2 Position = transform.position;
                Position.y -= yOffset;
                transform.position = Position;
            }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (index > 0)
            {
                 index--;
                 Vector2 Position = transform.position;
                 Position.y += yOffset;
                 transform.position = Position;
             }
    }

     
}
