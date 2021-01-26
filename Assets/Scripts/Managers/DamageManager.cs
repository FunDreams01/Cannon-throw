using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static int mistakes = 3;

    private static DamageManager _instance;
   public static DamageManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<DamageManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(DamageManager).Name;
                    _instance=go.AddComponent<DamageManager>();
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

public  void hitObstacle(){
    if(mistakes>0){
        mistakes--;
        UIManager.Instance.UpdateMistakes();
        if(mistakes == 0){
            //GameManager.Instance.Lost();
            
        }
    }

}


}
