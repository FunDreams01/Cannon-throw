using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
     private static ScenesManager _instance;
   public static ScenesManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<ScenesManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(ScenesManager).Name;
                    _instance=go.AddComponent<ScenesManager>();
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

    public void LoadScene(string s){
        SceneManager.LoadScene(s, LoadSceneMode.Single);
    }
}
