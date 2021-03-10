using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump2TargetFinal : MonoBehaviour {
    public Transform TargetTransform;
    //Make sure Gravity > 0 So that it creates an arc.
    //Make sure Horizontal Speed > 0 So it moves :)
    
    float HorizontalSpeed;
    public float Gravity;

    Vector3 HorizontalVelocity, initPosition;
     float initTime, maxTime;
    float initVerticalVelocity, VerticalDistance;
    bool projectileOnMotion = false;
    void Start () {
      //  Project ();
    }
    bool win;
    public void Project (bool win) {
        this.win = win;
        initPosition = transform.position;
        HorizontalSpeed=GetComponent<CharController>().CharSpeedForward;
        // Velocity is gets its direction from the difference between the target and the current position. 
        // The initial magnitude/norm of the velocity is HorizontalSpeed.    
        Vector3 Trajectory = (TargetTransform.position - transform.position);
        Vector3 HorizontalTrajectory = new Vector3 (Trajectory.x, 0, Trajectory.z);
        HorizontalVelocity = HorizontalTrajectory.normalized * HorizontalSpeed;
        VerticalDistance = Mathf.Abs (Trajectory.y);
        // Let's calculate the time it takes to achieve the goal in the given amount of time.
        // Distance/Speed = Time
        maxTime = HorizontalTrajectory.magnitude / HorizontalSpeed;
        initTime = Time.time;
        //Let's take care of te vertical velocity
        /*
         The usual equation for position with respect to time is:
         ½ · a · t² + v₀·t + x₀
         Let's say the target y-coordinate is 0, and we're x₀ (VerticalDistance) meters away from the target vertically.
         Well, since we want the position to equal 0 at the end, that is, when t = maxTime.
         Let's solve for v₀, initVerticalVelocity;
        */
        initVerticalVelocity = (0.5f * Gravity * maxTime * maxTime - VerticalDistance) / maxTime;
        projectileOnMotion = true;
        
    }
    bool t = false;
    //You can also call this is FixedUpdate(), 
    void Update () {
        //If the motion is marked complete, stop checking for anything.
        if (!projectileOnMotion) return;
        Debug.Log(Time.time - initTime);
        Debug.Log(maxTime);
        //If the time is past the predicted latest time, mark completed, and equal the position to Target to be 100% accurate.
        if (Time.time - initTime >= maxTime) {
            transform.position = TargetTransform.position;
            projectileOnMotion = false;
            Debug.Log("Done");
            if(win)FindObjectOfType<StateManager>().Win();
            if(!win)FindObjectOfType<StateManager>().Lose();
            t = true;
            return;
        }
        if(t) Debug.Log("Still");
        //What is the progress, how much time has passed since the initiation of motion.
        float curTime = (Time.time - initTime);
        //Let's remember x(t) = ½ · a · t² + v₀·t + x₀
        // For the horizontal motion, there is no acceleration, therefore the calculation follows the equation a lot simply with a=0:
        Vector3 tp = initPosition + (HorizontalVelocity * curTime);
        //For vertical, it is more complex.
        tp.y = initPosition.y + initVerticalVelocity * curTime - 0.5f * Gravity * curTime * curTime;
        transform.position = tp;
        Quaternion lookRotation = Quaternion.LookRotation ((TargetTransform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, 0.1f*Time.time);
    }

}