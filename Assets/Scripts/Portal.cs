using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isIn;
    public int number;
    private DungeonGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<DungeonGenerator>().GetComponent<DungeonGenerator>();
    }

    // Update is called once per frame
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            if(other.GetComponent<PlayerMotor>().canWarp){
                if(isIn){
                    Vector3 pos = generator.portalOuts[number+1].transform.position;
                    other.GetComponent<PlayerMotor>().TransformPosition(pos + new Vector3(0, 1f, 0));
                }else if(!isIn && number > 0){
                    Vector3 pos = generator.portalIns[number-1].transform.position;
                    other.GetComponent<PlayerMotor>().TransformPosition(pos + new Vector3(0, 1f, 0));
                }
            }
        }
    }

}
