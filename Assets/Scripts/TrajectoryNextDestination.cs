using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryNextDestination : MonoBehaviour
{
     GameObject [] go;
     [SerializeField]
    public GameObject NextDestination;
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

     private void OnTriggerEnter(Collider other)
    {
        if(this.gameObject.tag=="ring" && other.gameObject.tag=="Player"){
            //boost
        }
    }

}
