using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    Animator CannonAnimator;
    ParticleSystem CannonLight;
    public Transform CharacterSpawner;
    public bool rotating, scaling;
    public Vector3 newForward;

    float RotateStart;
    public float RotationTime = 1f;
    public float ScaleTime = 0.5f;
    Quaternion curRot;

    LevelBuilder LB;
    StateManager SM;
    void Awake()
    {
        CannonAnimator = GetComponentInChildren<Animator>();
        CannonLight = GetComponentInChildren<ParticleSystem>();
        SM = FindObjectOfType<StateManager>();
        LB = FindObjectOfType<LevelBuilder>();
    }

    public void Shoot()
    {
        StartCoroutine(Particles());
    }

    public void Catch()
    {
        StartCoroutine(StartRotation());
        CannonAnimator.SetTrigger("catch");
    }
    IEnumerator StartRotation()
    {
        yield return new WaitForSeconds(0.80f);
        rotating = true;
        initial_rot = transform.rotation;
        curRot = transform.rotation;
        RotateStart = Time.time;
    }
    Quaternion current;
    Quaternion initial_rot;
// GameObject go;
    void Update()
    {
        if (rotating)
        {
            current = Quaternion.Slerp(initial_rot,Quaternion.LookRotation(newForward,Vector3.up),(Time.time-RotateStart)/RotationTime);
            
            transform.rotation = current;

            if (Time.time - RotateStart > RotationTime)
            {
                rotating = false; scaling = true; RotateStart = Time.time;
            }
        }
        else if (scaling)
        {
            transform.localScale = Vector3.one * (1 + (2f * (ScaleTime - (Time.time - RotateStart)) / ScaleTime));
            if (transform.localScale.x <= 1f)
            {
                transform.localScale = Vector3.one;
                scaling = false;
                LB.NextPath();
                SM.StartGame();
            }
        }
    }

    IEnumerator Particles()
    {
        yield return new WaitForSeconds(0.25f);
        CannonAnimator.SetTrigger("shoot");
        yield return new WaitForSeconds(0.3f);
        CannonLight.Play();
    }
}

