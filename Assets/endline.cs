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
            GameManager.Instance.AlignEnd();
           GameManager.Instance.StartFollowPath(path);
           UIManager.Instance.EndState();
            GameManager.Instance.StopMove();
        }
    }
}
