using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour{
    public GameObject [] cannons_rings_endpoints;
    private static TrajectoryManager _instance;
    public static TrajectoryManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<TrajectoryManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(TrajectoryManager).Name;
                    _instance=go.AddComponent<TrajectoryManager>();
                }
            }
        return _instance;
       }
   }

    private void Awake(){
        if(_instance == null){
            _instance=this;
        } else {
            Destroy(gameObject);
        }
    }
}
