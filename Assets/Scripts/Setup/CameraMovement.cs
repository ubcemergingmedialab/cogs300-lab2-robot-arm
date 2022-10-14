using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        float inputX, inputY;
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (inputX != 0) {
            transform.Rotate(new Vector3 (0f, -(inputX * speed * Time.deltaTime), 0f));
        }

        if (inputY != 0) {
            transform.Translate(new Vector3(0f, inputY * 0.5f * speed  * Time.deltaTime, 0f));
        }
        
    }

}
