using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharController : MonoBehaviour
{


    //REFERENCES
    StateManager SM;

    //SPEED STUFF
    //SETTINGS
    public float CharSpeedMultiplier = 5f;  // How much does the force affect the speed.
    public float CharBaseSpeed = 5f; // Minimum Speed for the character

    public float CharSlowdown = 1f;
    public float CharSlowdownMultiplier = 1.5f;

    public float RotationAngularSpeed = 10f;

    public PathManager PM;

    Animator anim;

    //PRIVATE VARIABLES
    float CharSpeedForward;

    int CurrentNodeIndex;

    float CurrentHeight;

    float CurPos;

    [HideInInspector]
    public float MoveExtent;

    public int Coins = 0;

    bool dash = true;
    public BoxCollider CoinCollider;

    Vector3 IdealSidePos;

    Vector3 side;

    Vector3 CurrentSidePos = Vector3.zero;
    public float SideSpeed = 2f;
    public float CharMovePastMultiplier = 1.5f;

    CamControl cam;
    [Flags]
    enum BLOCKED 
    {
        NONE=0,
        LEFT=2,
        RIGHT=4,
        FORWARD=8
    }
    BLOCKED Curblock = BLOCKED.NONE;



    void Awake()
    {
        SM = FindObjectOfType<StateManager>();
        anim = GetComponentInChildren<Animator>();
        cam = FindObjectOfType<CamControl>();
    }


    public void RegisterForce(float Force, bool reset=true)
    {
        CharSpeedForward = CharSpeedMultiplier * Force + CharBaseSpeed;
        

        //EXTRA STUFF
        if(reset)
        side = Vector3.Cross(Vector3.up,PM.Direction);
    }

    public void Update()
    {
        if (CharSpeedForward > CharBaseSpeed)
        {
            CharSpeedForward = Mathf.Max(CharBaseSpeed, CharSpeedForward - CharSlowdown * Time.deltaTime);
        }
        if (SM.GetGameState() == States.FLYING)
        {
            if (dash && CharSpeedForward <= CharBaseSpeed * 1.6f)
            {
                anim.SetTrigger("dash");
                CharSlowdown *= CharSlowdownMultiplier;
                cam.ZoomIn();
                dash = false;
            }
            if((Curblock&BLOCKED.FORWARD)!=BLOCKED.FORWARD) 
            {
                CurPos += CharSpeedForward * Time.deltaTime;
            }
            Vector3 CurPosVec = PM.Origin + CurPos * PM.Direction;
            float angle;
            bool isHole = false;
            CurPosVec.y += PM.GetHeight(CurPos, ref CurrentHeight, ref CurrentNodeIndex, out angle, out isHole);
            
            float MoveX = CharMovePastMultiplier* MoveExtent*2*((Input.mousePosition.x / Screen.width) - 0.5f);
            if((MoveX < 0 && ((Curblock & BLOCKED.LEFT) != BLOCKED.LEFT)) || (MoveX > 0 && (Curblock & BLOCKED.RIGHT) != BLOCKED.RIGHT) || MoveX==0)
            IdealSidePos = PM.Origin + side*MoveX;
            CurrentSidePos = Vector3.MoveTowards(CurrentSidePos, IdealSidePos,SideSpeed * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(angle, 0, 0), RotationAngularSpeed * Time.deltaTime);
            transform.position = CurPosVec+CurrentSidePos;
        }


        




    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "coin")
        {
            Destroy(col.gameObject);
            Coins++;
        }

        if(col.gameObject.tag == "ring")
        {
           // anim.SetTrigger("dashForReal");
            RegisterForce(0.5f,false);
        }

        Debug.Log("enter + " + col.tag);
        if(col.gameObject.tag=="wall")
        {
         //   Curblock = Curblock | BLOCKED.FORWARD;
        }
        if(col.gameObject.tag=="wallLeft")
        {
          //  Curblock = Curblock | BLOCKED.LEFT;
        }

        if(col.gameObject.tag=="wallRight")
        {
        //    Curblock = Curblock | BLOCKED.RIGHT;
        }
    }
    void OnTriggerExit(Collider col)
    {
        Debug.Log("exit + " + col.tag);
        if(col.tag=="wall")
        {
            Curblock = Curblock & ~BLOCKED.FORWARD;
        }
        if(col.tag=="wallLeft")
        {
            Curblock = Curblock & ~BLOCKED.LEFT;
        }

        if(col.tag=="wallRight")
        {
            Curblock = Curblock & ~BLOCKED.RIGHT;
        }
        
    }

    
}
