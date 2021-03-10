using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public float RingForceMultiplier = 0.3f;

    public PathManager PM;

    Animator anim;

    //PRIVATE VARIABLES
    public float CharSpeedForward;

    int CurrentNodeIndex;

    float CurrentHeight;

    float CurPos;

    [HideInInspector]
    public float MoveExtent;

    public int Coins = 0;

    bool dash = true;

    Vector3 IdealSidePos;
    
    

    Vector3 side;

    Vector3 CurrentSidePos = Vector3.zero;
    public float SideSpeed = 2f;
    public float CharMovePastMultiplier = 1.5f;
    [HideInInspector]
    public float NozzleDistance;

    [HideInInspector]
    public float LastCannonDistance;
    bool stop = false;
    CamControl cam;
    [Flags]
    enum BLOCKED
    {
        NONE = 0,
        LEFT = 2,
        RIGHT = 4,
        FORWARD = 8
    }
    BLOCKED Curblock = BLOCKED.NONE;
    Material mat;
    public float RunDiff = 5f;

    public float MercyForStaminaEnd = -0.15f;

    bool spin = false;
 Image StaminaBar;
float CurrentStamina = 0f;
public float StaminaDepletion = 0.12f, StaminaFill = 0.25f, RingStaminaMultiplier=0.25f;
    void Awake()
    {
        SM = FindObjectOfType<StateManager>();
        anim = GetComponentInChildren<Animator>();
        
        cam = FindObjectOfType<CamControl>();
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        mat = new Material(smr.sharedMaterial);
        foreach( SkinnedMeshRenderer s in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            s.material = mat;
        }
    }


    public void RegisterForce(float Force, bool reset = true)
    {
        CharSpeedForward = CharSpeedMultiplier * Force + CharBaseSpeed;


        //EXTRA STUFF
        if (reset)
        {
        CurrentStamina = Force/2 + 0.5f;
            side = Vector3.Cross(Vector3.up, PM.Direction);
            transform.rotation = Quaternion.LookRotation(PM.Direction, Vector3.up);
            CurPos = 0;
            CurrentNodeIndex = 0;
            CurrentHeight = -9999f;
            dash = true;
            stop = false;
            wall_index = 0;
            StaminaBar = FindObjectOfType<InterfaceManager>().StaminaBar;
        }
        else
        {
            CurrentStamina+=Force*RingStaminaMultiplier;
        }
        CurrentStamina = Mathf.Min(1,CurrentStamina);

    }
        int wall_index = 0;

    public void Update()
    {
        if(dead)
        {
            CurPos+= CharSpeedForward * Time.deltaTime;
            if(CurPos < PM.End-NozzleDistance)
            go.transform.position += CharSpeedForward*Time.deltaTime*PM.Direction;
            return;
        }

        if (CharSpeedForward > CharBaseSpeed)
        {
            CharSpeedForward = Mathf.Max(CharBaseSpeed, CharSpeedForward - CharSlowdown * Time.deltaTime);
        }

        if (!stop && (SM.GetGameState() == States.FLYING || SM.GetGameState() == States.CATCH))
        {

            SM.IM.RegisterProgress(CurPos / (PM.End - NozzleDistance));
            if (dash && CharSpeedForward <= CharBaseSpeed * 1.6f)
            {
                anim.SetTrigger("dash");
                CharSlowdown *= CharSlowdownMultiplier;
                cam.ZoomIn();
                dash = false;
            }

            else if (spin && CharSpeedForward <= CharBaseSpeed * 1.05f)
            {
                anim.SetTrigger("spin");
                spin = false;
            }

            CurrentStamina -= StaminaDepletion*Time.deltaTime;


            if ((Curblock & BLOCKED.FORWARD) != BLOCKED.FORWARD)
            {
                CurPos += CharSpeedForward * Time.deltaTime;
            }
            Vector3 CurPosVec = PM.Origin + CurPos * PM.Direction;
            float angle;
            bool isHole = false;
            CurPosVec.y += PM.GetHeight(CurPos, ref CurrentHeight, ref CurrentNodeIndex, out angle, out isHole);


            if (CurPos < PM.End - (LastCannonDistance))
            {

                float MoveX = CharMovePastMultiplier * MoveExtent * 2 * ((Input.mousePosition.x / Screen.width) - 0.5f);

                ////DEBUG ONLY///
                if(Application.isEditor){
                MoveX = Mathf.Max(MoveX, -MoveExtent);
                MoveX = Mathf.Min(MoveX, MoveExtent);
                }
                if ((MoveX < 0 && ((Curblock & BLOCKED.LEFT) != BLOCKED.LEFT)) || (MoveX > 0 && (Curblock & BLOCKED.RIGHT) != BLOCKED.RIGHT) || MoveX == 0)
                    IdealSidePos = side * MoveX;
            }
            else
            {
                IdealSidePos = Vector3.zero;
                if (CurPos >= (PM.End - NozzleDistance) && SM.GetGameState() == States.FLYING)
                {
                    if (PM.EndCannon != null)
                    {
                        Debug.Log("CharCatch");
                        SM.CannonCatch(PM.EndCannon, this);
                        anim.SetTrigger("shoot");
                        stop = true;
                    }
                    else
                    {
                        GetComponent<Jump2TargetFinal>().Project(true);
                        SM.CannonCatch(null, this);
                        anim.SetTrigger("shoot");
                        stop = true;
                    }
                }
                if (CurPos >= PM.End)
                {
                    anim.SetTrigger("shoot");
                    Debug.Log("Charstop");
                    stop = true;
                }
            }
            CurrentSidePos = Vector3.MoveTowards(CurrentSidePos, IdealSidePos, (CharSpeedForward / CharBaseSpeed) * SideSpeed * Time.deltaTime);
            anim.SetBool("walk_l", false);
            anim.SetBool("walk_r", false);
            if (Vector3.Dot(CurrentSidePos, side) >= MoveExtent - RunDiff)
            {
                int mul;
                bool is_w = PM.isWall(CurPos+3.5f, ref wall_index, out mul);
                if (is_w && mul == +1)
                {

                    anim.SetBool("walk_r", true);
                    Debug.Log("RUN1");
                    CurrentStamina+=StaminaFill*Time.deltaTime;
        CurrentStamina = Mathf.Min(1,CurrentStamina);
                    
                }


            }
            else if (Vector3.Dot(CurrentSidePos, side) <= -MoveExtent + RunDiff)
            {
                int mul;
                bool is_w = PM.isWall(CurPos+3.5f, ref wall_index, out mul);
                if (is_w && mul == -1)
                {
                    Debug.Log("RUN2");
                    anim.SetBool("walk_l", true);
                    CurrentStamina+=StaminaFill*Time.deltaTime;
        CurrentStamina = Mathf.Min(1,CurrentStamina);
                }
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(angle, transform.rotation.eulerAngles.y, 0), RotationAngularSpeed * Time.deltaTime);
            transform.position = CurPosVec + CurrentSidePos;
        }
        if (stop && SM.GetGameState() == States.CATCH)
        {
            if (PM.EndCannon != null)
            {
                transform.position = PM.EndCannon.CharacterSpawner.position;
                transform.rotation = PM.EndCannon.CharacterSpawner.rotation;
            }
            else
            {
            }
        }
        
        StaminaBar.fillAmount = CurrentStamina;
        if(CurrentStamina <= 0.333333f)
        {
            float norm = Mathf.Max(CurrentStamina*3,0);
            mat.color = new Color(1f,norm*norm,norm*norm);
            if(CurrentStamina <= MercyForStaminaEnd)
            {
                Die();
            }
        }
        else{mat.color = Color.white;}


    }
    bool dead = false;
    GameObject go;
void Die()
{
    stop=true;
    dead = true;
    go = new GameObject();
    go.transform.position = transform.position;
    go.transform.rotation= transform.rotation;

    var go2 = Instantiate(go);
    go2.transform.position += 90*PM.Direction;
    go2.transform.position += 20*Vector3.down;
    
    GetComponent<Jump2TargetFinal>().TargetTransform = go2.transform;
    GetComponent<Jump2TargetFinal>().Project(false);
    cam.AssignCharToCam(go.transform);
}
    IEnumerator Spin()
    {

        yield return new WaitForSeconds(0.4f);

        //anim.SetBool("Spin", false);
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "coin")
        {
            Destroy(col.gameObject);
            SM.IM.RegisterCoin(++Coins);
        }

        if (col.gameObject.tag == "ring")
        {
            if (!spin)
            {
                anim.SetTrigger("shoot");
                spin = true;
            }
            //StartCoroutine(Spin());
            RegisterForce(RingForceMultiplier, false);
        }

        if (col.gameObject.tag == "wall")
        {
            //   Curblock = Curblock | BLOCKED.FORWARD;
        }
        if (col.gameObject.tag == "wallLeft")
        {
            //  Curblock = Curblock | BLOCKED.LEFT;
        }

        if (col.gameObject.tag == "wallRight")
        {
            //    Curblock = Curblock | BLOCKED.RIGHT;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "wall")
        {
            Curblock = Curblock & ~BLOCKED.FORWARD;
        }
        if (col.tag == "wallLeft")
        {
            Curblock = Curblock & ~BLOCKED.LEFT;
        }

        if (col.tag == "wallRight")
        {
            Curblock = Curblock & ~BLOCKED.RIGHT;
        }

    }


}
