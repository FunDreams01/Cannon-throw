using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation.Examples {

    public class RunningWall : MonoBehaviour {
        public GameObject go;
        private void OnCollisionEnter (Collision other) {
            if (other.gameObject.tag == "Player") {
                if (this.gameObject.tag == "wallLeft") {
                    CharacterController.Instance.moveLeft = false;
                    GameManager.Instance.WalkLeft ();
                } else if (this.gameObject.tag == "wallRight") {
                    CharacterController.Instance.moveRight = false;
                    GameManager.Instance.WalkRight ();
                }
                GameManager.Instance.StartWalk ();
                //GameManager.Instance.StopMove();
                CharacterController.Instance.StraightenPlayer();
            }
        }

        private void OnCollisionExit (Collision other) {
            if (other.gameObject.tag == "Player") {
                if (this.gameObject.tag == "wallLeft") {
                    CharacterController.Instance.moveLeft = true;
                    GameManager.Instance.WalkLeft ();
                } else if (this.gameObject.tag == "wallRight") {
                    CharacterController.Instance.moveRight = true;
                    GameManager.Instance.WalkRight ();
                }
                //GameManager.Instance.StopWalk ();
                GameManager.Instance.Fly ();
                //GetComponent<Collider> ().enabled = false;
                //CharacterController.Instance.SetBackTOtrack(true,go);
            }
        }
    }
}