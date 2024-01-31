using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{

    [SerializeField]
    private int randomIndex;
    private BuildingManager buildingManager;
    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.FindWithTag("BuildingManager").GetComponent<BuildingManager>();
        randomIndex = Random.Range(0, buildingManager.buildingAssets.Length);
        Instantiate(buildingManager.buildingAssets[randomIndex], transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
