using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation.Examples{
public class RedirectToPath : MonoBehaviour
{ public PathCreator path;
   private void OnTriggerEnter(Collider other)
   {
       if(other.gameObject.tag=="Player"){
           GameManager.Instance.RedirectToPath(path);
       }
   }
}

}

