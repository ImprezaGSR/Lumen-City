using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private List<GameObject> enemyList;
    void Start(){
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
    void Update(){
        if(enemyList.Count == 0){
            gameManager.SetReticleImageColor(false);
        }else{
            gameManager.SetReticleImageColor(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy"){
            enemyList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Enemy"){
            enemyList.Remove(other.gameObject);
        }
    }
    public void RemoveOnDestroy(GameObject obj){
        if(enemyList.Contains(obj)){
            enemyList.Remove(obj);
        }
    }
}
