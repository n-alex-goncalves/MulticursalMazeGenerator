using UnityEngine;
using System.Collections;

public class TriggerScript_D : MonoBehaviour
{
    private Rigidbody2D _rb;

    //Executation function that delays the following program
    IEnumerator ExecuteAfterTime(float time)
    {
        //Searchs for the opposing trigger wall
        string grandparent_name = transform.parent.parent.name;
        string parent_name = transform.parent.name;
        GameObject opp_wall = GameObject.Find(grandparent_name + "/" + parent_name + "/TriggerWall - U");

        yield return new WaitForSeconds(time);
        //Opposing wall deactivates and becomes trigger
        opp_wall.GetComponent<Collider2D>().isTrigger = true;

    }

    //Function that detects object collision
    void OnTriggerExit2D(Collider2D other)
    {
        //Searches for the opposing trigger wall
        string grandparent_name = transform.parent.parent.name;
        string parent_name = transform.parent.name;
        GameObject opp_wall = GameObject.Find(grandparent_name + "/" + parent_name + "/TriggerWall - U");

        //Opposing wall becomes solid
        opp_wall.GetComponent<Collider2D>().isTrigger = false;
        StartCoroutine(ExecuteAfterTime(0.2f));
    }
}