using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollisionMultiCheck : MonoBehaviour
{
    public GameObject TargetSpawner;
    
    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Target") {
            MultiSpawn rs = TargetSpawner.GetComponent<MultiSpawn>();
            rs.OnTargetHit(collision.collider.gameObject);
        }
    }
}
