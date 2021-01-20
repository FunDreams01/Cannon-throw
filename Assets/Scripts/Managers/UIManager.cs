using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField] 
    Image mistake1;
    [SerializeField]
    Image mistake2;
    [SerializeField]
    Image mistake3;

    [SerializeField]
    Image LostPanel;
    [SerializeField]
    Image WinPanel;
    private static UIManager _instance;
    public static UIManager Instance{

        get {
            if(_instance == null){
                _instance=FindObjectOfType<UIManager>();
                if(_instance == null){
                    GameObject go = new GameObject();
                    go.name = typeof(UIManager).Name;
                    _instance=go.AddComponent<UIManager>();
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
    public void UpdateMistakes(){
        if(DamageManager.mistakes == 2){
            mistake1.gameObject.SetActive(true);
        }else if(DamageManager.mistakes == 1){
            mistake2.gameObject.SetActive(true);
        }else if(DamageManager.mistakes == 0){
            mistake3.gameObject.SetActive(true);
        }
    }

    public void UpdateScore(){
        scoreText.text=ScoreManager.score.ToString();
    }

    public void Lost(){
        LostPanel.gameObject.SetActive(true);
    }
     public void Won(){
        WinPanel.gameObject.SetActive(true);
    }
}
