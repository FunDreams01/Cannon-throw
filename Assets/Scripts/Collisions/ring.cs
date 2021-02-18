using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ring : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
          transform.Find("RiseRing").GetComponent<ParticleSystem>().Play();
          CharacterController.Instance.nbr_spins=CharacterController.Instance.nbr_spins+1;
          
        }
    }
}
