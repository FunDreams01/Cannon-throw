using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackpoin : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
       if(other.gameObject.tag=="Player"){
          GameManager.Instance.StartMove();
          CharacterController.Instance.SetBackTOtrack(false,null);
       }
   }
}
