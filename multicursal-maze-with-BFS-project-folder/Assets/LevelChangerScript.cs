using UnityEngine;

public class LevelChangerScript : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("hello");
            Fade_To_Level(1);
        }
    }

    public void Fade_To_Level(int levelIndex)
    {
        animator.SetTrigger("New Trigger");
    }
}
