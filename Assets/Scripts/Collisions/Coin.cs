using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            GameManager.Instance.CollectCoin();
            Destroy(this.gameObject);
        }
    }
        
}
