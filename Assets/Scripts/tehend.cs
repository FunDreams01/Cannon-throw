using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tehend : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
            CharacterController.Instance.StopFlying();
            CharacterController.Instance.stopForce=true;
            UIManager.Instance.Won();
        }
    }
}
