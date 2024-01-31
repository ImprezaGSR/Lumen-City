using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAssetSpawner : MonoBehaviour
{
    Vector3 origin;
    Vector3 direction = new Vector3(0, -1, 0);
    public GameObject AssetTest;
    private bool hasSpawned = false;
    private BuilderAssetManager asset;

    void Start(){
        asset = FindObjectOfType<BuilderAssetManager>().GetComponent<BuilderAssetManager>();
        Invoke("DestroySelf", 1.0f);
    }
    void Update()
    {	
        origin = transform.position;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.green);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)){
            if(hit.collider.CompareTag("Island")){
                Debug.Log("IslandHit");
                if(!hasSpawned){
                    int randomIndex = Random.Range(0,asset.vegetationAssets.Length);
                    Instantiate(asset.vegetationAssets[randomIndex], hit.point + new Vector3(0, 1, 0), Quaternion.identity);
                    Debug.Log("Spawned");
                    hasSpawned = true;
                    DestroySelf();
                }
            }
        }
    }
    public void DestroySelf(){
        Destroy(gameObject);
    }
}
