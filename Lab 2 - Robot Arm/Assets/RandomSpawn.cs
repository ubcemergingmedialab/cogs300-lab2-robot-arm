using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject target;
    public GameObject[] targets;
    public Transform targetPos;
    public bool spawned = false;

    private int prefabIdx;

    void Update() {
        if (!spawned) {
            SpawnTarget();
        }
    }

    void SpawnTarget() {
        prefabIdx = 0; //Random.Range(0, targets.Length);  

        float xPos = Random.Range(-5f, -3f);
        if (Random.value >= 0.5) xPos = Random.Range(3f, 5f);
        float yPos = Random.Range(8f, 11f);
        float zPos = Random.Range(-5f, -3f);
        if (Random.value >= 0.5) zPos = Random.Range(3f, 5f);

        targetPos.position = new Vector3(xPos, yPos, zPos);
        //targetPos.position = new Vector3(6f, yPos, 0f);
        target = Instantiate(targets[prefabIdx], targetPos.position, targetPos.rotation);
        target.tag = "Target";
        spawned = true;
    }

    public void OnTargetHit() {
        Destroy(target);
        spawned = false;
    }
}
