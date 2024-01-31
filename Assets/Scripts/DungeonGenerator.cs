using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Vector2 xRange = new Vector2(-100f, 100f);
    public Vector2 yRange = new Vector2(-10f, 10f);
    public Vector2 zRange = new Vector2(50f, 100f);
    public GameObject[] roomPrehabs;

    [SerializeField]
    private List<GameObject> spawnableRooms = new List<GameObject>();

    public List<GameObject> portalIns = new List<GameObject>();
    public List<GameObject> portalOuts = new List<GameObject>();
    public int roomsToGenerate = 10;

    [SerializeField]
    private PlayerMotor playerMotor;
    // Start is called before the first frame update
    void Start()
    {
        playerMotor = GameObject.FindWithTag("Player").GetComponent<PlayerMotor>();
        ResetSpawnableRooms();
        GenerateRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateRooms(){
        for (int i = 0; i<roomsToGenerate; i++){
            if(i>0){
                transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(xRange.x,xRange.y), transform.localPosition.y + Random.Range(yRange.x,yRange.y), transform.localPosition.z);
            }
            if(spawnableRooms.Count == 0){
                ResetSpawnableRooms();
            }
            int randomRoomSpawn = Random.Range(0,spawnableRooms.Count);
            var room = Instantiate(spawnableRooms[randomRoomSpawn], transform.position, Quaternion.identity);
            spawnableRooms.Remove(spawnableRooms[randomRoomSpawn]);
            Vector3 roomScale = room.transform.Find("AreaBox").localScale;
            // Vector3 roomScale = new Vector3 (1,1,1);
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z + roomScale.z + Random.Range(zRange.x,zRange.y));
            if(i == 0){
                playerMotor.TransformPosition(room.transform.Find("BridgeBeginning").position + new Vector3(0, 1f, 0));
                Debug.Log("Transport player");
            }
            portalIns.Add(room.transform.Find("BridgeEnd").gameObject);
            room.transform.Find("BridgeEnd").Find("PortalIn").Find("Portal").GetComponent<Portal>().isIn = true;
            room.transform.Find("BridgeEnd").Find("PortalIn").Find("Portal").GetComponent<Portal>().number = portalIns.Count-1;
            portalOuts.Add(room.transform.Find("BridgeBeginning").gameObject);
            room.transform.Find("BridgeBeginning").Find("PortalOut").Find("Portal").GetComponent<Portal>().isIn = false;
            room.transform.Find("BridgeBeginning").Find("PortalOut").Find("Portal").GetComponent<Portal>().number = portalOuts.Count-1;
        }
    }
    void ResetSpawnableRooms(){
        for (int i = 0; i<roomPrehabs.Length; i++){
            spawnableRooms.Add(roomPrehabs[i]);
        }
        Debug.Log("ResetSpawnableRooms");
    }
}
