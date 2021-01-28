using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public static int coinBonus = 10;
   private static ScoreManager _instance;
   public static ScoreManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<ScoreManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(ScoreManager).Name;
                    _instance=go.AddComponent<ScoreManager>();
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

    public void CollectCoin(){
        score=score+coinBonus;
        UIManager.Instance.UpdateScore();
    }
}
