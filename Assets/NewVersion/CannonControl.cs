using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    Animator CannonAnimator;
    ParticleSystem CannonLight;
    public Transform CharacterSpawner;

    void Awake()
    {
        CannonAnimator = GetComponentInChildren<Animator>();
        CannonLight = GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot(){
        
        StartCoroutine(Particles());
    }

    IEnumerator Particles(){
        FindObjectOfType<CamControl>().SideView();
        yield return new WaitForSeconds(0.25f);
        CannonAnimator.SetTrigger("shoot");
        yield return new WaitForSeconds(0.3f);
        CannonLight.Play();
    }
}

