using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace PathCreation.Examples { }

public class endPath : MonoBehaviour
{
    public PathCreator path;
    public int follower;
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Player"){
            GameManager.Instance.DestroyTrajectory(path.gameObject);
            GameManager.Instance.StopFollowPath(follower);
        }
    }
}
