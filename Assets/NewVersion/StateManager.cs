//This file both manages the gamestates and works as an intermediary between different constituants of the project. 
//Since this is a very simple project, I didn't bother to seperate them.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The game state enum. I will not use a flagged system (see bit flag enums) since I do not anticipate a use for multiple selection.
 public enum States {
        WELCOME_SCREEN,
        IN_CANNON,
        CATCH,
        SHOOTING,
        FLYING,
        WIN_SCREEN,
        LOSE_SCREEN            
    }

public class StateManager : MonoBehaviour
{
   
    States GameState = States.WELCOME_SCREEN;
    public PathManager CurrentPath;
    CamControl cam;

    //Managers
    [HideInInspector]
    public InterfaceManager IM;
    public CharController CC;
    void Awake()
    {
        IM = GetComponent<InterfaceManager>();
        CC = FindObjectOfType<CharController>();
        cam = FindObjectOfType<CamControl>();
    }

    public void Win()
    {
        StartState(States.WIN_SCREEN);
    }


    public void Lose()
    {
        StartState(States.LOSE_SCREEN);
    }
    //This is called from the Interface Manager. It converts the state from WELCOME_SCREEN to IN_CANNON
    public  void StartGame(){
        if(GameState == States.WELCOME_SCREEN || GameState == States.CATCH)
        {
            StartState(States.IN_CANNON);
        }
        
    }
    
    public void CannonCatch(CannonControl cannon,CharController ch)
    {
        StartState(States.CATCH);
        //ch.transform.SetParent(cannon.transform); Debug.Log("ch:" + ch.name + " " + cannon.name);
        if(cannon!=null)
        cannon.Catch();
        cam.AwayView();
    }

    void StartState(States State)
    {
        //UI Objects.
        IM.SwichToState(State, GameState);
        GameState = State;
        Debug.Log("State Started:" + State.ToString());
    }

    //I couldn't think of a name :/
    public void CanonGoBOOM(float CharacterForce){

        cam.SideView();
        CurrentPath.StartCannon.Shoot();
        CC.RegisterForce(CharacterForce);
        StartState(States.SHOOTING);
        StartCoroutine(CanonWait()); 
    }
    IEnumerator CanonWait()
    {
        yield return new WaitForSeconds(1.05f);
        StartState(States.FLYING);
    }


    public States GetGameState(){
        return GameState;
    }

    

}
