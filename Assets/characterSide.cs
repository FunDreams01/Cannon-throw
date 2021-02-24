using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSide : MonoBehaviour {
    public string side;
    private void OnCollisionEnter (Collision other) {
        if (other.gameObject.tag == "building") {
            CharacterController.Instance.sideCollision = side;
            if (CharacterController.Instance.sideCollision == "right") {
                CharacterController.Instance.moveRight = false;
            } else if (CharacterController.Instance.sideCollision == "left") {
                CharacterController.Instance.moveLeft = false;
            }

            if (side == "front") {
                Debug.Log("--------");
                CharacterController.Instance.StopForce (true);
                GameManager.Instance.Fall ();
                UIManager.Instance.closeGamePanel ();
                UIManager.Instance.Lost ();
            }
        }
    }

    private void OnCollisionExit (Collision other) {
        if (other.gameObject.tag == "building") {
            CharacterController.Instance.sideCollision = "";
            if (CharacterController.Instance.sideCollision == "right") {
                CharacterController.Instance.moveRight = true;
            } else if (CharacterController.Instance.sideCollision == "left") {
                CharacterController.Instance.moveLeft = true;
            }
        }
    }
}