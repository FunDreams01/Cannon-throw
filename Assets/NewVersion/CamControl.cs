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
    public CinemachineVirtualCamera CV7;
    public CinemachineVirtualCamera HoleCam;
    public CinemachineVirtualCamera FirstCam;
    public CinemachineVirtualCamera AwayCam;
    
    void Awake(){
        CB = GetComponent<CinemachineBrain>();
    }
    public void AssignCharToCam(Transform ch){
        CV1.Follow = ch;
        CV1.LookAt = ch; 
        CV2.Follow = ch; 
        CV2.LookAt = ch; 
        CV3.Follow = ch; 
        CV3.LookAt = ch; 
        CV7.Follow = ch; 
        CV7.LookAt = ch; 
        FirstCam.Follow = ch; 
        FirstCam.LookAt = ch; 
        HoleCam.Follow = ch; 
        HoleCam.LookAt = ch; 
        AwayCam.Follow = ch; 
        AwayCam.LookAt = ch; 
    }
    
    
    public void ZoomIn()
    {
        CV2.gameObject.SetActive(true);
        CV1.gameObject.SetActive(false);
        CV7.gameObject.SetActive(false);
    }
    public void SideView()
    {
        FirstCam.gameObject.SetActive(false);
        StartCoroutine(SideWait());
    }
    IEnumerator SideWait()
    {
        CV3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        CV3.gameObject.SetActive(false);
    }
    IEnumerator assign(Transform go)
    {
        yield return new WaitForSeconds(1.5f);
        AwayCam.Follow = go; 
        AwayCam.LookAt = go; 
    }
    
    public void AssignAwayCamObject(Transform go)
    {
        StartCoroutine(assign(go));
    }

    IEnumerator Away()
    {
        AwayCam.gameObject.SetActive(true);
        CV2.gameObject.SetActive(false);
        CV7.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        AwayCam.gameObject.SetActive(false);
    }
    bool hole = false;
        public void AwayView()
    {
        StartCoroutine(Away());
    }

    IEnumerator HoleRoutine(){
        HoleCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        HoleCam.gameObject.SetActive(false);
        hole=false;
    }

    public void HoleLook()
    {
//        if(!hole)StartCoroutine(HoleRoutine());
        if(!hole)
        HoleCam.gameObject.SetActive(true);
        hole=true;
    } 

    public void AntiHoleLook()
    {

        if(hole)
        HoleCam.gameObject.SetActive(false);
        hole=false;
    }
    
}

