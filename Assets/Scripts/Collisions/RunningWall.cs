using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation.Examples {

    public class RunningWall : MonoBehaviour {
        public GameObject go;
        private void OnCollisionEnter (Collision other) {
            if (other.gameObject.tag == "Player") {

                if (this.gameObject.tag == "wallLeft") {
                    //  GameManager.Instance.WalkLeft();
                } else if (this.gameObject.tag == "wallRight") {
                    // GameManager.Instance.WalkRight();
                }
                GameManager.Instance.StartWalk ();
            }
        }

        private void OnCollisionExit (Collision other) {
            if (other.gameObject.tag == "Player") {
                GameManager.Instance.StopWalk ();
                //GameManager.Instance.Fly ();
                GetComponent<Collider> ().enabled = false;
                GameManager.Instance.RedirectToPoint (go);
            }
        }
    }
}