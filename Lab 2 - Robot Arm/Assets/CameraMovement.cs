using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float inputX;

    public float speed;

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        if (inputX != 0) {
            transform.Rotate(new Vector3 (0f, inputX * speed * Time.deltaTime, 0f));
        }
    }

}
