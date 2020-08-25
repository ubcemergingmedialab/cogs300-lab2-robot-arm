using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm_m : MonoBehaviour
{
    public Rigidbody AnchorRb;
    public float ArmRelativeAngle;
    public GameObject TargetSpawner;

    // MS: ArmMovementList is now just a list of Vector3's
    // this allows us to simplify the MoveArm function!
    private List<Vector3> ArmMovementList = new List<Vector3>();
    private int MovementIndex = 0;
    private bool moving = false;
    private float RotateStep = 90f; // 90 degrees per second
    
    void Start()
    {   
        //Here you add 4 movements to the List
        // ArmMovementList.Add(new ArmMovement("anchor_horizontal", 112f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", 90f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", -90f));
        // ArmMovementList.Add(new ArmMovement("anchor_horizontal", -179f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", 90f));
    }

    void Update()
    {   
        if (!moving && ArmMovementList.Count == 0) {
            RandomSpawn rs = TargetSpawner.GetComponent<RandomSpawn>();
    
            // MS: kept your general logic here, but
            // made the rotation-calculator
            // much simpler!
            float hAngle = getRotation(rs.target);

            // MS: note that movements are now just vectors
            ArmMovementList.Add(new Vector3(0f, hAngle, 0f));
            ArmMovementList.Add(new Vector3(90f, 0f, 0f));
            ArmMovementList.Add(new Vector3(-90f, 0f, 0f));
        }
        else if (!moving && MovementIndex < ArmMovementList.Count) {
            MoveArm();
        }
        else if (!moving && MovementIndex >= ArmMovementList.Count) {
            ClearMovementList();
        }
    }

    private void MoveArm() {
        moving = true;

        StartCoroutine(doRotate(ArmMovementList[MovementIndex]));

        MovementIndex += 1;
    }

    private void ClearMovementList() {
        ArmMovementList.Clear();
        MovementIndex = 0;
    }

    IEnumerator doRotate(Vector3 rotation) {
        Quaternion destination = AnchorRb.transform.rotation * Quaternion.Euler(rotation);
        // MS: note slightly changed logic here -- rotation is in
        // fixed degrees per second
        // we measure 
        // 1) current time; (Time.time)
        // 2) time at previous movement; (previousTime)
        // 3) time elapsed since previous movement (deltaTime)
        // then we move towards target by deltaTime * RotateStep degrees
        // and we stop when the target rotation is reached!

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

    float getRotation(GameObject target) {
        
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

}