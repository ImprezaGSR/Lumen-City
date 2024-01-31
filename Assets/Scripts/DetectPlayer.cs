using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public EnemyMotor motor;

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            motor.detectTarget = true;
        }
    }
}
