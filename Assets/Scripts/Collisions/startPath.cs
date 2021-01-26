using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace PathCreation.Examples { }
public class startPath : MonoBehaviour
{
    public int path;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
            GameManager.Instance.StartFollowPath(path);
        }
    }
}
