using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpawn : MonoBehaviour
{
    public AudioSource targetHitSound;

    public void OnTargetHit(GameObject target) {
        Destroy(target);
        targetHitSound.Play();
    }
}
