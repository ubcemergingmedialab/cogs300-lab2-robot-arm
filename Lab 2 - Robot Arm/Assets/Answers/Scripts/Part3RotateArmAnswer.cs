using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part3RotateArmAnswer : MonoBehaviour
{
    public Rigidbody AnchorRb;  // Refers to the anchor of the arm (the sphere at the bottom)
    public GameObject TargetSpawner;
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
    }


    void Update()
    {   
        RandomSpawn rs = TargetSpawner.GetComponent<RandomSpawn>();
        GameObject target = rs.target; // the target variable points to the current target
        
        // A target will randomly spawn somewhere within the arm's reach
        // The arm needs to rotate to face the target, then move down 90 degrees to touch it
        // When the arm touches the target, the target will disappear, and a new target will appear somwhere else
        
        // TODO: implement the arm movement logic so that the arm can automically find the target when it appears
        if (!moving && ArmMovementList.Count == 0) {
            if (target != null) {
                float hAngle = getHRotation(target);
                float vAngle = getVRotation(target);

                // MS: note that movements are now just vectors
                ArmMovementList.Add(new Vector3(0f, hAngle, 0f));
                ArmMovementList.Add(new Vector3(vAngle, 0f, 0f));
                ArmMovementList.Add(new Vector3(-vAngle, 0f, 0f));
            }
        }
        else if (!moving && MovementIndex < ArmMovementList.Count) {
            MoveArm();
        }
        else if (!moving && MovementIndex >= ArmMovementList.Count) {
            ClearMovementList();
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


    // Clear ArmMovementList and set the MovementIndex back to 0
    // Note: this is necessary because moving to the next arm movement only involves
    // incrementing the MovementIndex, meaning any movement already completed
    // will not be automatically removed
    private void ClearMovementList() {
        ArmMovementList.Clear();
        MovementIndex = 0;
    }


    // Get the horizontal rotation required to face the target
    // Input: a reference to the target object
    // Output: signed angle for the required horizontal rotation
    private float getHRotation(GameObject target) {
        
        // X & Z coords from the target
        Vector3 rsXZ = new Vector3(
                    target.transform.position.x,
                    0,
                    target.transform.position.z
        );

        // signed angle between target X&Z and
        // the vector that points forward from
        // our anchor (the blue line in the 3d editor window)
        float hAngle = Vector3.SignedAngle(
            AnchorRb.transform.forward,
            rsXZ,
            Vector3.up
        );

        return hAngle;
    }


    // Get the vertical rotation required to touch the target (assuming already facing target)
    // BONUS: implement this function, then uses it in the appopriate place so that the arm
    // will rotate forward just enough to touch the target instead of the full 90 degrees
    private float getVRotation(GameObject target) {
        float vAngle = Vector3.Angle(
            Vector3.up,
            target.transform.position
        );

        return vAngle;
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