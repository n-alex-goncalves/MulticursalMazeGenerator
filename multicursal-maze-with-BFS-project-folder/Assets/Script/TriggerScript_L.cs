 using UnityEngine;
using System.Collections;

public class TriggerScript_L : MonoBehaviour
{
    private Rigidbody2D _rb;

    IEnumerator ExecuteAfterTime(float time)
    {
        string grandparent_name = transform.parent.parent.name;
        string parent_name = transform.parent.name;
        GameObject opp_wall = GameObject.Find(grandparent_name + "/" + parent_name + "/TriggerWall - R");

        yield return new WaitForSeconds(time);
        opp_wall.GetComponent<Collider2D>().isTrigger = true;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        string grandparent_name = transform.parent.parent.name;
        string parent_name = transform.parent.name;
        GameObject opp_wall = GameObject.Find(grandparent_name + "/" + parent_name + "/TriggerWall - R");

        opp_wall.GetComponent<Collider2D>().isTrigger = false;
        StartCoroutine(ExecuteAfterTime(0.2f));

        //wall has to be set as static
        //activate the rigidbody of the opposite wall
        //timedelay of one second
        //deactivate the rigidbody of the opposite wall
    }
}