using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
namespace PathCreation.Examples{}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject currentCannon;
    GameObject trajectory0;
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

    public void RedirectToPoint(GameObject go){
        CharacterController.Instance.RedirectToPoint(go);
    }

    public bool GetPlayerState(){
        return CharacterController.Instance.GetPlayerState();
    }
    public void StopFlying(){
        CharacterController.Instance.StopFlying();
    }
    public float GetSpeed(){
      return  CharacterController.Instance.GetSpeed();
    }
    public void StartFollowPath (int i) {
        CharacterController.Instance.StartFollowPath(i);
    }

    public void StopFollowPath (int i) {
        CharacterController.Instance.StopFollowPath(i);
    }

public void DestroyTrajectory(GameObject path){
    CharacterController.Instance.DestroyTrajectory(path);
}
public void RedirectToPath(PathCreator path){
    CharacterController.Instance.RedirectToPath(path);
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

public void Fall(){
    CharacterController.Instance.Fall();
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

public void launchCharacter(){
    CharacterController.Instance.launchCharacter();
}

public void SetCurrentCanon(GameObject go){
    currentCannon=go;
}

public GameObject GetCurrentCanon(){
    return currentCannon;
}

public void SetDirection(){
    CharacterController.Instance.SetDirection();
}
public void SetSpeed(float s){
    CharacterController.Instance.SetSpeed(s);
}
}
