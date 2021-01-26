using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public string swcitchToCam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
            CinemachineSwitcher.Instance.playAnim(swcitchToCam);
        }
    }
}
