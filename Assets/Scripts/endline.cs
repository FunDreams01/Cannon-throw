using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endline : MonoBehaviour
{
    public int path;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
            UIManager.Instance.decStamina = false;       
           UIManager.Instance.EndState();
            GameManager.Instance.StopMove();
            CharacterController.Instance.Jump();
            UIManager.Instance.closeGamePanel();
             CinemachineSwitcher.Instance.playAnim ("side");
        }
    }
}
