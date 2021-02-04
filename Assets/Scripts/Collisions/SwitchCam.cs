using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour {
  /*private void OnTriggerEnter (Collider other) {
    if (other.gameObject.tag == "Player") {
      if (GameManager.Instance.GetCamState () == "close") {
        GameManager.Instance.changeCam ("closeToFar");
      } else if (GameManager.Instance.GetCamState () == "far") {
        GameManager.Instance.changeCam ("farToClose");
      }
    }
  }*/
  private void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.tag=="Player"){
      if(CinemachineSwitcher.Instance.GetCurrentAnim()=="start"){
        CinemachineSwitcher.Instance.playAnim("main");
      }/*else if(CinemachineSwitcher.Instance.GetCurrentAnim()=="main"){
         CinemachineSwitcher.Instance.playAnim("start");
      }*/
    }
  }
}