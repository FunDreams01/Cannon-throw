using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{

    StateManager SM;
    public ScreenDesignator[] Screens;
    HitIndicatorController HIC;

    public TextMeshProUGUI Curr,Prev,Coin;
    public Image ProgBar,StaminaBar;

    

    void Awake()
    {
        HIC = FindObjectOfType<HitIndicatorController>();
        SM = GetComponent<StateManager>();
    }
    public void SwichToState(States NewState, States OldState){

        /*
        Actually the old state is accessible until the state change is completed but I do not want to store each screen in a seperate variable,
        Or do I want to make a search amongst the screens as there are only going to be a handful of them, so it is not worth to complicate the code.
        Clean code is better, no performance boost will come from that.
        */
        foreach (ScreenDesignator Screen in Screens)
        {  //Disable all UI Screens who does not match the game state, and enable all those that match.
           Screen.gameObject.SetActive(Screen.ScreenState == NewState);
        }
    }


    public void RegisterCoin(int coin)
    {
        Coin.text = coin.ToString();
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RegisterProgress(float normalizedProgress)
    {
        ProgBar.fillAmount = normalizedProgress;
        
    }
    //This function is just here so that the UI only interacts through the InterfaceManager.
    //This void is called from the "Tap To Play" Button.
    public void CallGameStart(){
        SM.StartGame();
    }
    //This function is called from the UI, upon calling, it takes the angular information from the HitIndicator and sends this through to the Satate/Game Manager.
    public void RegisterCanonForce()
    {
        SM.CanonGoBOOM(HIC.GetNormalizedForce());
    }
    

}
