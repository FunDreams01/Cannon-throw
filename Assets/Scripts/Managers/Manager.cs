using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GameManager gameManager = GameManager.Instance; 
       LevelManager levelManager = LevelManager.Instance; 
       UIManager uiManager = UIManager.Instance; 
       ScoreManager scoreManager = ScoreManager.Instance; 
       DamageManager damageManager = DamageManager.Instance; 
    }

}
