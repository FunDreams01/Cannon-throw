using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   private static GameManager _instance;
   public static GameManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<GameManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(GameManager).Name;
                    _instance=go.AddComponent<GameManager>();
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

public void hitObstacle(){
    DamageManager.Instance.hitObstacle();
}
public void CollectCoin(){
    ScoreManager.Instance.CollectCoin();
}
public void Lost(){
    UIManager.Instance.Lost();
}

public void EndPointReached(){
    // activate swimming animation of player
    UIManager.Instance.Won();
}


}
