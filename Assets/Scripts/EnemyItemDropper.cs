using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDropper : MonoBehaviour
{
    public GameObject[] goldAssets;
    public EnemyStatus status;
    public float scatterRadius;
    public Vector2 radiusRange;
    public Transform dropperCore;

    public void ScatterItem(Rigidbody rb){
        float radius = scatterRadius + Random.Range(radiusRange.x, radiusRange.y);
        float xAxis = Random.Range(0, radius);
        float zAxis = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(xAxis, 2));
        Vector3 direction = new Vector3(xAxis * (Random.Range(0, 2) * 2 - 1), 0f, zAxis * (Random.Range(0, 2) * 2 - 1));
        rb.AddForce(direction);
    }

    public void SpawnRequiredGolds(){
        int remainingGold = status.gold;
        foreach(var asset in goldAssets){
            if(asset.transform.GetChild(0).GetComponent<Golds>().goldWorth > 1){
                int spawnTimes = (int)remainingGold / asset.transform.GetChild(0).GetComponent<Golds>().goldWorth;
                for(int i = 0; i < spawnTimes; i++){
                    GameObject spawnedAsset = Instantiate(asset, dropperCore.position, dropperCore.rotation);
                    ScatterItem(spawnedAsset.GetComponent<Rigidbody>());
                }
                Debug.Log("Spawned "+spawnTimes+" "+asset.name);

                remainingGold -= asset.transform.GetChild(0).GetComponent<Golds>().goldWorth * spawnTimes;
            }else if(asset.transform.GetChild(0).GetComponent<Golds>().goldWorth == 1){
                for(int i = 0; i < remainingGold; i++){
                    GameObject spawnedAsset = Instantiate(asset, dropperCore.position, dropperCore.rotation);
                    ScatterItem(spawnedAsset.GetComponent<Rigidbody>());
                }
                Debug.Log("Spawned "+remainingGold+" "+asset.name);
                remainingGold = 0;
                
            }
        }
    }
    
}
