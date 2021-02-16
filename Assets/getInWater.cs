using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getInWater : MonoBehaviour
{
    public GameObject water_prefab;
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag=="water"){
           
       } 
    }
}
