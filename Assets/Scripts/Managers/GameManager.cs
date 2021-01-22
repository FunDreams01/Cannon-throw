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
    CharacterController.Instance.Swim();
    UIManager.Instance.Won();
}

public void Idle(){
    CharacterController.Instance.Idle();
}

public void Fly(){
    CharacterController.Instance.Fly();
}
public void WalkLeft(){
    CharacterController.Instance.WalkLeft();
}

public void WalkRight(){
    CharacterController.Instance.WalkRight();
}

public void StopWalk(){
    CharacterController.Instance.StopWallWalking();
}
public void StartWalk(){
    CharacterController.Instance.ActivateWallWalking();
}
public void SetDestination(GameObject go){
    CharacterController.Instance.setDestination(go);
}
public void launchCharacter(){
    CharacterController.Instance.launchCharacter();
}
}
