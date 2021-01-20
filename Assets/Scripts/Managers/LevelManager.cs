using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static int launch_force=0;
    //current level : to be declared

   private static LevelManager _instance;
   public static LevelManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<LevelManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(LevelManager).Name;
                    _instance=go.AddComponent<LevelManager>();
                    DontDestroyOnLoad(go);
                }
            }
        return _instance;
       }
   }

    private void Awake(){
        if(_instance == null){
            _instance=this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
