using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public Rigidbody AnchorRb;
    public float ArmRelativeAngle;
    public GameObject TargetSpawner;

    private List<ArmMovement> ArmMovementList = new List<ArmMovement>();
    private int MovementIndex = 0;
    private bool moving = false;
    private float TimeToRotate; 
    private float TimeAngleRatio = 1.5f / 90f;
    
    void Start()
    {   
        //Here you add 4 movements to the List
        // ArmMovementList.Add(new ArmMovement("anchor_horizontal", 112f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", 90f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", -90f));
        // ArmMovementList.Add(new ArmMovement("anchor_horizontal", -179f));
        // ArmMovementList.Add(new ArmMovement("anchor_vertical", 90f));
        ArmRelativeAngle = 0;
    }

    void Update()
    {   
        if (!moving && ArmMovementList.Count == 0) {
            RandomSpawn rs = TargetSpawner.GetComponent<RandomSpawn>();
            if (!moving && rs.spawned) {
                float hAngle = GetRotateAngle(GetTargetRelativeAngle(rs.target));
                //Debug.Log(hAngle);
                ArmMovementList.Add(new ArmMovement("anchor_horizontal", hAngle));
                ArmMovementList.Add(new ArmMovement("anchor_vertical", 90f));
                ArmMovementList.Add(new ArmMovement("anchor_vertical", -90f));

                UpdateArmRelativeAngle(hAngle);
            }
        }
        else if (!moving && MovementIndex < ArmMovementList.Count) {
            MoveArm();
        }
        else if (!moving && MovementIndex >= ArmMovementList.Count) {
            ClearMovementList();
        }
    }

    private void UpdateArmRelativeAngle(float RotateAngle) {
        ArmRelativeAngle += RotateAngle;
        if (ArmRelativeAngle < 0) ArmRelativeAngle += 360;
        else if (ArmRelativeAngle >= 360) ArmRelativeAngle = ArmRelativeAngle % 360;
        Debug.Log("Arm Relative Angle");
        Debug.Log(ArmRelativeAngle);

    }

    private void MoveArm() {
        moving = true;
        ArmMovement am = ArmMovementList[MovementIndex];
        MovementIndex += 1;

        Vector3 EulerRotation;
        if (am.MovementType == "anchor_vertical") {
            EulerRotation = new Vector3(am.MovementDegree, 0f, 0f);
        }
        else if (am.MovementType == "anchor_horizontal") {  
            EulerRotation = new Vector3(0f, am.MovementDegree, 0f);               
        }
        else {
            EulerRotation = new Vector3(0f, 0f, 0f);
        }
        TimeToRotate = TimeAngleRatio * Mathf.Abs(am.MovementDegree);

        StartCoroutine(doRotate(EulerRotation));
    }

    private void ClearMovementList() {
        ArmMovementList.Clear();
        MovementIndex = 0;
    }

    IEnumerator doRotate(Vector3 rotation) {
        Quaternion start = AnchorRb.transform.rotation;
        Quaternion destination = start * Quaternion.Euler(rotation);
        float startTime = Time.time;
        float percentComplete = 0f;
        
        moving = true;
        
        while(percentComplete <= 1.0f) {
            percentComplete = (Time.time - startTime) / TimeToRotate;          
            AnchorRb.transform.rotation = Quaternion.Slerp(start, destination, percentComplete);
            yield return null;
        }
        
        moving = false;
    }

    float GetRotateAngle(float TargetRelativeAngle) {
        float rotateAngle;
        if (TargetRelativeAngle > ArmRelativeAngle) {
            rotateAngle = TargetRelativeAngle - ArmRelativeAngle;
            if (rotateAngle >= 180) rotateAngle = -(360 - rotateAngle); 
        }
        else {
            rotateAngle = ArmRelativeAngle - TargetRelativeAngle;
            if (rotateAngle >= 180) rotateAngle = 360 - rotateAngle;
            else rotateAngle = -rotateAngle; 
        }

        
        Debug.Log("Rotate Angle:");
        Debug.Log(rotateAngle);
        return rotateAngle;
    }

    float GetTargetRelativeAngle(GameObject target) {
        // Vector3 relativePos = target.transform.position - AnchorRb.transform.position;

        // var forward = AnchorRb.transform.forward;
        // float angle = Vector3.Angle(relativePos, forward);
        // if (target.transform.position.x < AnchorRb.transform.position.x) angle = -angle + 360;
        
        // Debug.Log("Target Relative Angle:");
        // Debug.Log(angle);
        // return angle;

        Vector3 targetPos = target.transform.position;
        float PosX = targetPos.x;
        float PosZ = targetPos.z;

        float refAngle = Mathf.Atan(Mathf.Abs(PosZ) / Mathf.Abs(PosX)) / Mathf.PI * 180;
        float TargetRelativeAngle;
        if (PosX >= 0 & PosZ < 0) TargetRelativeAngle = refAngle + 90;
        else if (PosX < 0 & PosZ < 0) TargetRelativeAngle = 270 - refAngle;
        else if (PosX < 0 & PosZ >= 0) TargetRelativeAngle = 270 + refAngle;
        else TargetRelativeAngle = refAngle;

        Debug.Log("Target Relative Angle:");
        Debug.Log(TargetRelativeAngle);

        return TargetRelativeAngle;
    }

}


public class ArmMovement
 {

    public string MovementType;
    public float MovementDegree;

    public ArmMovement (string mt, float md)
    {
        this.MovementType = mt;
        this.MovementDegree = (float)md;
    }
 }