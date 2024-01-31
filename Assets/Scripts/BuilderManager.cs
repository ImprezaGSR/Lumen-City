using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    private BuilderAssetManager asset;
    public Vector2 buildingSpace = new Vector2(5f,5f);
    public bool isHexagonal = false;
    public int floorNum = 1;
    void Start()
    {
        asset = FindObjectOfType<BuildingManager>().GetComponent<BuilderAssetManager>();
        StartBuilding();
    }

    void StartBuilding(){
        Vector3 startTilePos = new Vector3((buildingSpace.x-1f) * 1.5f, 0, (buildingSpace.y-1f) * 1.5f);
        //Build Square Rooms
        for (int i = 0; i < floorNum; i++){
            for(int y = 0; y < buildingSpace.y; y++){
                for(int x = 0; x < buildingSpace.x; x++){
                    Vector3 currentTilePos = new Vector3(x * 3, 0, y*3);
                    if(x == 0){
                        if((y + 1) % 2 == 0){
                            Instantiate(asset.wallWindow, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -270, 0));
                        }else if(i == 0 && y == 2){
                            Instantiate(asset.door, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -270, 0));
                        }else{
                            if(isHexagonal){
                                if(y == 0){
                                    SpawnWallRandomly45(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, 0, 0));
                                }
                            }else{
                                SpawnWallRandomly(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -270, 0));
                            }
                        }
                    }else if (x == buildingSpace.x-1){
                        if((y + 1) % 2 == 0){
                            Instantiate(asset.wallWindow, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -90, 0));
                        }else if(i == 0 && y == 2){
                            Instantiate(asset.door, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -90, 0));
                        }else{
                            if(isHexagonal){
                                if(y == 0){
                                    SpawnWallRandomly45(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -90, 0));
                                }
                            }else{
                                SpawnWallRandomly(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -90, 0));
                            }
                        }
                    }
                    if(y == 0){
                        if(x == 0){
                            if(!isHexagonal){
                                Instantiate(asset.pillar, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, 0, 0));
                            }
                        }else if(x == buildingSpace.x-1){
                            if(!isHexagonal){
                                Instantiate(asset.pillar, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -90, 0));
                            }
                        }
                        if((x+1) % 2 == 0){
                            Instantiate(asset.wallWindow, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, 0, 0));
                        }else if(i == 0 && x == 2){
                            Instantiate(asset.door, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, 0, 0));
                        }else{
                            if(isHexagonal){

                            }else{
                                SpawnWallRandomly(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, 0, 0));
                            }
                        }
                    }else if(y == buildingSpace.y-1){
                        if(x == 0){
                            if(isHexagonal){
                                SpawnWallRandomly45(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -270, 0));
                            }else{
                                Instantiate(asset.pillar, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -270, 0));
                            }
                        }else if(x == buildingSpace.x-1){
                            if(isHexagonal){
                                SpawnWallRandomly45(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -180, 0));
                            }else{
                                Instantiate(asset.pillar, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -180, 0));
                            }
                        }
                        if((x+1) % 2 == 0){
                            Instantiate(asset.wallWindow, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -180, 0));
                        }else if(i == 0 && x == 2){
                            Instantiate(asset.door, transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -180, 0));
                        }else{
                            if(isHexagonal){

                            }else{
                                SpawnWallRandomly(transform.position + startTilePos - currentTilePos, Quaternion.Euler(0, -180, 0));
                            }
                        }
                    }
                    if(i == 0){
                        int randomFloors = Random.Range(0, asset.floors.Length);
                        Instantiate(asset.floors[randomFloors], transform.position + startTilePos - currentTilePos, Quaternion.identity);
                    }else if(i > 0 && (i+2)%2 == 1 && x == 2 && y == 2){
                        Instantiate(asset.stairs, transform.position + startTilePos - currentTilePos, Quaternion.identity);
                    }else if(i > 0 && (i+2)%2 == 0 && (x + 1) % 2 == 0 && y == 2){
                        Instantiate(asset.stairs, transform.position + startTilePos - currentTilePos, Quaternion.identity);
                    }else{
                        int randomFloors = Random.Range(0, asset.floorSeconds.Length);
                        Instantiate(asset.floorSeconds[randomFloors], transform.position + startTilePos - currentTilePos, Quaternion.identity);
                    }
                }
            }
            startTilePos = new Vector3(startTilePos.x, startTilePos.y + 4.5f, startTilePos.z);
        }
        //Build Ceilings
        for(int y = 0; y < buildingSpace.y; y++){
            for(int x = 0; x < buildingSpace.x; x++){
                Vector3 currentTilePos = new Vector3(x * 3, 0, y * 3);
                if((floorNum + 2)%2 == 1 && x == 2 && y == 2){
                    Instantiate(asset.stairs, transform.position + startTilePos - currentTilePos, Quaternion.identity);
                }else if((floorNum + 2)%2 == 0 && (x + 1) % 2 == 0 && y == 2){
                    Instantiate(asset.stairs, transform.position + startTilePos - currentTilePos, Quaternion.identity);
                }else{
                    int randomFloors = Random.Range(0, asset.floorTop.Length);
                    Instantiate(asset.floorTop[randomFloors], transform.position + startTilePos - currentTilePos, Quaternion.identity);
                }
            }
        }
    }

    void SpawnWallRandomly(Vector3 pos, Quaternion qua){
        int randomWalls = Random.Range(0, asset.walls.Length);
        Instantiate(asset.walls[randomWalls], pos, qua);
    }

    void SpawnWallRandomly45(Vector3 pos, Quaternion qua){
        int randomWalls = Random.Range(0, asset.walls.Length);
        Instantiate(asset.walls45[randomWalls], pos, qua);
    }
}
