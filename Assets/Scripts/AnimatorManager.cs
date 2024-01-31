using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    [SerializeField]
    private string currentState;
    [SerializeField]
    private bool layerChanging = false;
    [SerializeField]
    private float layerTransSpeed;
    [SerializeField]
    private float layerWeightSet;
    [SerializeField]
    private int layerIndexSet;

    [SerializeField]
    private bool magnitudeChanging;
    [SerializeField]
    private string magnitudeState;
    [SerializeField]
    private float magnitudeAmount;
    [SerializeField]
    private float magnitudeDamping;
    private float dampingTemp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if(layerChanging){
            float weight = animator.GetLayerWeight(layerIndexSet);
            if(weight < layerWeightSet){
                weight += Time.deltaTime / layerTransSpeed;
                if(weight > layerWeightSet){
                    weight = layerWeightSet;
                }
            }else if(weight > layerWeightSet){
                weight -= Time.deltaTime / layerTransSpeed;
                if(weight < layerWeightSet){
                    weight = layerWeightSet;
                }
            }
            animator.SetLayerWeight(layerIndexSet, weight);
            if(weight == layerWeightSet){
                layerChanging = false;
            }
        }
        if(magnitudeState != null){
            animator.SetFloat(magnitudeState, magnitudeAmount, magnitudeDamping, Time.deltaTime);
        }
    }

    public void SetAnimationSpeed(float newSpeed)
    {
        animator.speed = newSpeed;
    }

    public void SetMagnitude(string name, float magnitude, float damping)
    {
        // magnitudeChanging = false;
        // animator.SetFloat(name, magnitude, damping, Time.deltaTime);

        magnitudeState = name;
        magnitudeAmount = magnitude;
        magnitudeDamping = damping;
        dampingTemp = damping;
    }

    public string GetAnimationState(){
        return currentState;
    }

    public void ChangeAnimationState(string newState)
    {
        if(currentState == newState){
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

    public void SetAnimationLayerWeight(int layerIndex, float layerWeight, float damping){
        if(damping <= 0){
            animator.SetLayerWeight(layerIndex, layerWeight);
        }else{
            layerIndexSet = layerIndex;
            layerTransSpeed = damping;
            layerWeightSet = layerWeight;
            layerChanging = true;
        }
    }

    public void SetAnimationBool(string name, bool isTrue){
        animator.SetBool(name, isTrue);
    }
}
