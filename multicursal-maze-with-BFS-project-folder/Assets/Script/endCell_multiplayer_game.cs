using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class endCell_multiplayer_game : MonoBehaviour
{
    //The current score of player_1 (remains consistent throughout scenes)
    private static int player_1_score = 0;
    //The current score of player_2 (remains consistent throughout scenes)
    private static int player_2_score = 0;

    //Textmesh variables - used to manipulate the text
    static TextMeshProUGUI textMesh;
    static TextMeshProUGUI textMesh_2;

    private void Start()
    {
        GameObject text = GameObject.Find("IngameTEXT/Player_1 Text");
        textMesh = text.GetComponent<TextMeshProUGUI>();
        textMesh.text = "PLAYER ONE SCORE  - " + player_1_score;

        GameObject text_2 = GameObject.Find("IngameTEXT/Player_2 Text");
        textMesh_2 = text_2.GetComponent<TextMeshProUGUI>();
        textMesh_2.text = "PLAYER TWO SCORE  - " + player_2_score;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("You win!");
            player_1_score += 1;
            SceneManager.LoadScene("Multiplayer_Game_Scene");
        }
        else if (other.tag == "Player_2")
        {
            Debug.Log("You lose.");
            player_2_score += 1;
            SceneManager.LoadScene("Multiplayer_Game_Scene");
        }
    }
}