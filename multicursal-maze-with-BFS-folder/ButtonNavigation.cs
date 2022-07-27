using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonNavigation : MonoBehaviour
{
    int index = 0;
    public int total_index = 2;
    public float yOffset = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (index < total_index - 1)
            {
                index = index + 1;
                Vector2 Position = transform.position;
                Position.y -= yOffset;
                transform.position = Position;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (index > 0)
            {
                index = index - 1;
                Vector2 Position = transform.position;
                Position.y += yOffset;
                transform.position = Position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (index == 0)
            {
                SceneManager.LoadScene("Standard_Game_Scene");
            }

            if (index == 1)
            {
                SceneManager.LoadScene("Multiplayer_Game_Scene");
            }

            if (index == 2)
            {
                SceneManager.LoadScene("Quit");
            }


        }
    }
}