using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextPoint : MonoBehaviour
{
    public GameObject nextP;
      private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Player") {
            if (nextP == null) {
                CharacterController.Instance.follow = false;
                CharacterController.Instance.pathTarget = null;
                CharacterController.Instance.StartMove();
                CharacterController.Instance.moveLeft=true;
                CharacterController.Instance.moveRight=true;
            } else {
                CharacterController.Instance.pathTarget = nextP;
                CharacterController.Instance.follow = true;
                CharacterController.Instance.StopMove();
            }
             Destroy(this.gameObject);
        }
    }
}
