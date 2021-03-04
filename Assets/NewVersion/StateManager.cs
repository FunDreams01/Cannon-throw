//This file both manages the gamestates and works as an intermediary between different constituants of the project. 
//Since this is a very simple project, I didn't bother to seperate them.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The game state enum. I will not use a flagged system (see bit flag enums) since I do not anticipate a use for multiple selection.
 public enum States {
        WELCOME_SCREEN,
        IN_CANNON,
        SHOOTING,
        FLYING,
        MENU_SCREEN
    }

public class StateManager : MonoBehaviour
{
   
    States GameState = States.WELCOME_SCREEN;
    public PathManager CurrentPath;

    //Managers
    InterfaceManager IM;
    public CharController CC;
    void Awake()
    {
        IM = GetComponent<InterfaceManager>();
        CC = FindObjectOfType<CharController>();
    }

    void Update()
    {
        
    }
    //This is called from the Interface Manager. It converts the state from WELCOME_SCREEN to IN_CANNON
    public  void StartGame(){
        if(GameState == States.WELCOME_SCREEN)
        {
            StartState(States.IN_CANNON);
        }
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
