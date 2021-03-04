using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamControl : MonoBehaviour
{
    CinemachineBrain CB;
    public CinemachineVirtualCamera CV1;
    public CinemachineVirtualCamera CV2;
    public CinemachineVirtualCamera CV3;
    public CinemachineVirtualCamera HoleCam;
    
    void Awake(){
        CB = GetComponent<CinemachineBrain>();
    }
    public void AssignCharToCam(){
        CV1.Follow = FindObjectOfType<CharController>().transform;
        CV1.LookAt = FindObjectOfType<CharController>().transform;
        CV2.Follow = FindObjectOfType<CharController>().transform;
        CV2.LookAt = FindObjectOfType<CharController>().transform;
        CV3.Follow = FindObjectOfType<CharController>().transform;
        CV3.LookAt = FindObjectOfType<CharController>().transform;
        HoleCam.Follow = FindObjectOfType<CharController>().transform;
        HoleCam.LookAt = FindObjectOfType<CharController>().transform;
    }
    
    public void ZoomIn()
    {
        CV2.gameObject.SetActive(true);
        CV1.gameObject.SetActive(false);
    }
    public void SideView()
    {
        StartCoroutine(SideWait());
    }
    IEnumerator SideWait()
    {
        CV3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        CV3.gameObject.SetActive(false);
    }

    IEnumerator HoleRoutine(){
        HoleCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        HoleCam.gameObject.SetActive(false);
    }

    public void HoleLook()
    {
        StartCoroutine(HoleRoutine());
    } 
    
}

