using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonRenderTest : MonoBehaviour
{
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    public GameObject raycast;

    private bool hasSpawned;
    List<Vector2> points;

    //Raycast
    Vector3 origin;
    Vector3 direction = new Vector3(0, -1, 0);
    public GameObject AssetTest;
    private bool hasAssetSpawned = false;
    private BuilderAssetManager asset;
    void Start()
    {
        asset = FindObjectOfType<BuilderAssetManager>().GetComponent<BuilderAssetManager>();
        points = PoissonSample.GeneratePoints(radius,regionSize, rejectionSamples);
        // Invoke("DestroySelf", 1.0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(regionSize.x, 0, regionSize.y)/2, new Vector3(regionSize.x, 0, regionSize.y));
        if(points != null){
            foreach(Vector2 point in points){
                Gizmos.DrawSphere(transform.position + new Vector3(point.x, 0, point.y), displayRadius);
            }
        }
    }

    void Update(){
        Debug.Log(points);
        if(points != null){
            if(!hasSpawned){
                foreach(Vector2 point in points){
                    //Instantiate(raycast, transform.position + new Vector3(point.x, 0, point.y), Quaternion.identity);

                    origin = transform.position + new Vector3(point.x, 0, point.y);
                    Ray ray = new Ray(origin, direction);
                    Debug.DrawRay(ray.origin, ray.direction * 200, Color.green);
                    RaycastHit hit;
                    LayerMask mask = LayerMask.GetMask("Ground");
                    if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask)){
                        if(hit.collider.CompareTag("Island")){
                            Debug.Log("IslandHit");
                            int randomIndex = Random.Range(0,asset.vegetationAssets.Length);
                            Instantiate(asset.vegetationAssets[randomIndex], hit.point + new Vector3(0, 1, 0), Quaternion.identity);
                            Debug.Log("Spawned");
                        }
                    }
                }
                hasSpawned = true;
            }
        }
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
