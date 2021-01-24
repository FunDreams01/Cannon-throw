using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
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
}
