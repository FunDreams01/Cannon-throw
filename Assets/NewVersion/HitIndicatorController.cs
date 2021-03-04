using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicatorController : MonoBehaviour
{
    public Transform Arrow;
    public float Speed;
    bool isGoingLeft = false;
    public float MaximumAngle = 16.5f; 

    StateManager SM;

    void Awake(){SM = FindObjectOfType<StateManager>();}


    // This function uses the current angle (euler y) of the arrow object in order to calculate a normalized force.
    // 0° → Returns 1
    // > MaximumAngle → Returns 0 or less. 
    public float GetNormalizedForce()
    {
    float cur_angle = Arrow.rotation.eulerAngles.y;
    if(cur_angle > 180) cur_angle -=360;
    cur_angle = Mathf.Abs(cur_angle);
    cur_angle= MaximumAngle - cur_angle;
    return cur_angle/MaximumAngle;
    }

    //Only animate if necessary. No need to run in the background.
    public void Update(){
    
     if(SM.GetGameState() == States.IN_CANNON)
    {
    
    //THIS IS A VERY DIRTY WAY TO HANDLE THE ANIMATION, I WOULD LIKE IT TO BE SHORTER AND MORE ELEGANT IN THE FUTURE, BUT IT WORKS FOR NOW.
    float cur_angle = Arrow.rotation.eulerAngles.y;
    if(cur_angle > 180) cur_angle -=360;
    float angle = Time.deltaTime * Speed;
    if( cur_angle>= MaximumAngle) isGoingLeft = true;
    if(cur_angle<= -MaximumAngle) isGoingLeft = false;
    if(isGoingLeft) angle*=-1;
    
   
        Arrow.Rotate(Vector3.up,angle);
    }
    }
}
