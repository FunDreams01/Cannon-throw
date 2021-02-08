using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfScene : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
{
    if(other.gameObject.tag=="Player"){
       GameManager.Instance.Fall();
        UIManager.Instance.closeGamePanel();
       UIManager.Instance.Lost();
    }
}
}
