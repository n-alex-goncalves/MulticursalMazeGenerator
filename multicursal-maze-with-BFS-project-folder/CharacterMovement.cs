using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private void Update()
{
    Rigidbody rb = GetComponent<Rigidbody>();
    if (Input.GetKey(KeyCode.A))
        rb.AddForce(Vector3.left);
    if (Input.GetKey(KeyCode.D))
        rb.AddForce(Vector3.right);
    if (Input.GetKey(KeyCode.W))
        rb.AddForce(Vector3.up);
    if (Input.GetKey(KeyCode.S))
        rb.AddForce(Vector3.down);

}