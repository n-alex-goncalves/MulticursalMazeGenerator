using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class endCell_standard_game : MonoBehaviour
{
    GameObject text;
    private static int level = 1;


    static TextMeshProUGUI textMesh;

    public void Start()
    {
        GameObject text = GameObject.Find("IngameTEXT/Level Text");
        textMesh = text.GetComponent<TextMeshProUGUI>();
        textMesh.text = "LVL " + level;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("You win!");
            CPU.moveSpeed += 1f;
            level += 1;
            SceneManager.LoadScene("Standard_Game_Scene");
        }
        else if (other.tag == "CPU")
        {
            Debug.Log("You lose.");
            Debug.Break();
        }
    }
}