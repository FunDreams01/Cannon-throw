using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject c1;
    public GameObject c2;
    [SerializeField]
    Text scoreText;
   [SerializeField]
    Text coinText;


    [SerializeField]
    GameObject LostPanel;
    [SerializeField]
    GameObject WinPanel;
     [SerializeField]
    GameObject StartPanel;
    [SerializeField]
    GameObject GamePanel;
    public Image progress;
    public float maxdist;
   public  float d;
    public string state;
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
private void Start()
{
    state="half1";
    progress.fillAmount=0;
    maxdist= Vector3.Distance(player.transform.position, c1.transform.position);
}

private void Update()
{
    if(state=="half1"){
        if(Vector3.Distance(player.transform.position, c1.transform.position)>0){
             d=1 -(Vector3.Distance(player.transform.position, c1.transform.position)/maxdist);
            progress.fillAmount=d/2;
        }else{
            state="half2";
            maxdist= Vector3.Distance(player.transform.position, c2.transform.position);
        }
       
    }/*else if(state=="half2"){
        if(Vector3.Distance(player.transform.position, c2.transform.position)>0){
             d=1 -(Vector3.Distance(player.transform.position, c2.transform.position)/maxdist);
            progress.fillAmount=progress.fillAmount+d/2;
        }else{
            state="end";
        }
       
    }*/
}
    public void UpdateScore(){
       scoreText.text=ScoreManager.score.ToString();
    coinText.text=ScoreManager.score.ToString();
    }

    public void Lost(){
        LostPanel.gameObject.SetActive(true);
    }
     public void Won(){
        WinPanel.gameObject.SetActive(true);
    }
     public void Game(){
        GamePanel.gameObject.SetActive(true);
    }

    public void closeStartPanel(){
        StartPanel.SetActive(false);
    }

     public void closeGamePanel(){
       GamePanel.SetActive(false);
    }

    public void init(){
        GameManager.Instance.init();
    }
}
