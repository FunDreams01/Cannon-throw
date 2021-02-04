using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    public GameObject [] vcams;
    Animator anim;
    private static CinemachineSwitcher _instance;
   public static CinemachineSwitcher Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<CinemachineSwitcher>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(CinemachineSwitcher).Name;
                    _instance=go.AddComponent<CinemachineSwitcher>();
                }
            }
        return _instance;
       }
   }

    private void Awake(){
        if(_instance == null){
            _instance=this;
        } else {
            Destroy(gameObject);
        }
    }
 private void Start()
{
    anim=GetComponent<Animator>();  
    playAnim("start");
}
    public void playAnim(string s){
        anim.Play(s);
    }

    public void StopFollowing(string name){
        foreach(GameObject g in vcams){
            if(g.name==name){
                g.GetComponent<CinemachineVirtualCamera>().Follow=null;
            }
        }
    }

    public string GetCurrentAnim(){
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("start")){
            return "start";
        }else if(anim.GetCurrentAnimatorStateInfo(0).IsName("main")){
            return "main";
        }else if(anim.GetCurrentAnimatorStateInfo(0).IsName("side")){
            return "side";
        }else{
            return "";
        }
    }
}
