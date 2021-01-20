using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private static ButtonManager _instance;
    public static ButtonManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<ButtonManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(ButtonManager).Name;
                    _instance=go.AddComponent<ButtonManager>();
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
