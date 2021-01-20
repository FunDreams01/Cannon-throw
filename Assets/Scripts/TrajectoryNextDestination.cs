using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryNextDestination : MonoBehaviour
{
     GameObject [] go;
     [SerializeField]
     GameObject NextDestination;
    // Start is called before the first frame update
    void Start(){
        int i=0;
        go= TrajectoryManager.Instance.cannons_rings_endpoints;
        foreach( GameObject g in go){
            if(GameObject.ReferenceEquals(this.gameObject, g)){
                NextDestination=go[i+1];
                break;
            }
            i++;
        }
    }

}
