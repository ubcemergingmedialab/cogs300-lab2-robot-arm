using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2RotateArmAnswer : MonoBehaviour
{
    public Rigidbody AnchorRb;  // Refers to the anchor of the arm (the sphere at the bottom)
    public float speed = 1; // The rotating speed

    // ArmMovementList: the list of rotation movements (each as a Vector3) to be done
    // Each Vector3 specifies the degree to rotate on each axis
    // Example: Vector3(90f, 0f, 0f) specifies a rotation of 90 degrees around the X axis and 0 degrees on Y and Z axis
    private List<Vector3> ArmMovementList;  
    private int MovementIndex; // The position (in ArmMovementList) of the arm movement currently being processed
    private bool moving; // Indicates whether the arm is currently moving
    private float RotateStep; 
    

    void Start()
    {   
        Initialize();

        // There are now 3 targets
        // TODO: Add appropriate sequence of movement so that
        // the arm hits all 3 targets 
        ArmMovementList.Add(new Vector3(0f, 90f, 0f));
        ArmMovementList.Add(new Vector3(90f, 0f, 0f));
        ArmMovementList.Add(new Vector3(-90f, 0f, 0f));

        ArmMovementList.Add(new Vector3(0f, 60f, 0f));
        ArmMovementList.Add(new Vector3(90f, 0f, 0f));
        ArmMovementList.Add(new Vector3(-90f, 0f, 0f));

        ArmMovementList.Add(new Vector3(0f, -170f, 0f));  // or 190f, but it will spin in the same direction
        ArmMovementList.Add(new Vector3(90f, 0f, 0f));
        ArmMovementList.Add(new Vector3(-90f, 0f, 0f));
    }


    void Update()
    {   
        if (!moving && MovementIndex < ArmMovementList.Count) {
            MoveArm();
        }
    }


    // Initalizes all private variables 
    private void Initialize() {
        ArmMovementList = new List<Vector3>();
        MovementIndex = 0;
        moving = false;
        RotateStep = 90f * speed; // 90 degrees per second at speed 1
    }


    // Executes the next arm rotation movement in ArmMovementList
    private void MoveArm() {
        moving = true;

        StartCoroutine(doRotate(ArmMovementList[MovementIndex]));

        MovementIndex += 1;
    }


    private IEnumerator doRotate(Vector3 rotation) {
        Quaternion destination = AnchorRb.transform.rotation * Quaternion.Euler(rotation);
        // Rotation is in fixed degrees per second
        // We measure: 
        // 1) current time; (Time.time)
        // 2) time at previous movement; (previousTime)
        // 3) time elapsed since previous movement (deltaTime)
        // then we move towards target by deltaTime * RotateStep degrees
        // and we stop when the target rotation is reached

        float previousTime = Time.time;
        float deltaTime;
        
        moving = true;
        
        while(AnchorRb.transform.rotation != destination) {
            deltaTime = Time.time - previousTime; // how much time has elapsed?
            previousTime = Time.time; // this time is now previous time
            AnchorRb.transform.rotation = Quaternion.RotateTowards(
                AnchorRb.transform.rotation, 
                destination, 
                deltaTime * RotateStep
            ); // nudge towards destination rotation
            yield return null;
        }
        
        moving = false;
    }
}