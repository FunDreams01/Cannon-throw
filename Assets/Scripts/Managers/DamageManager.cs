using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    public int damage_count = 0;
    private static DamageManager _instance;
    public static DamageManager Instance {

        get {
            if (_instance == null) {
                _instance = FindObjectOfType<DamageManager> ();
                if (_instance == null) {
                    GameObject go = new GameObject ();
                    go.name = typeof (DamageManager).Name;
                    _instance = go.AddComponent<DamageManager> ();
                    DontDestroyOnLoad (go);
                }
            }
            return _instance;
        }
    }

    private void Awake () {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (this.gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    public void hitObstacle () {
        if (damage_count<3) {
            damage_count++;
            UIManager.Instance.damage (damage_count);
        } else {
            CharacterController.Instance.stopForce=false;
            GameManager.Instance.Fall ();
            UIManager.Instance.closeGamePanel ();
            UIManager.Instance.Lost ();          
        }
    }

}