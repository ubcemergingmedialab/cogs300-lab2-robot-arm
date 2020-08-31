using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollisionCheck : MonoBehaviour
{
    public GameObject TargetSpawner;
    
    void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Target") {
            RandomSpawn rs = TargetSpawner.GetComponent<RandomSpawn>();
            rs.OnTargetHit();
        }
    }
}
