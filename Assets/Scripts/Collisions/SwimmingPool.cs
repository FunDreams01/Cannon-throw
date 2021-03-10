using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPool : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
       if(other.gameObject.tag == "Player"){
        //  CharacterController.Instance.Swim();
       }
   }
}
