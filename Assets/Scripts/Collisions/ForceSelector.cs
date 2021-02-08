﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSelector : MonoBehaviour {
  
    [Header("Arrow Speed")]
    public float animSpeed = 1;
    public bool forceSlected = false;
    GameObject myArrow;
    Animator arrowAnim;
    GameObject myRaycast;
    RaycastHit hit;
    int layerMask;
    float speed;

    // Start is called before the first frame update
    void Start () {
        myArrow = transform.Find("Arrow").gameObject;
        arrowAnim = myArrow.GetComponent<Animator> ();
        myRaycast = myArrow.transform.Find ("raycast").gameObject;
        layerMask = LayerMask.GetMask("forceSelection");
        arrowAnim.speed = animSpeed;
    }

   public void selectForce () {
       arrowAnim.enabled = false;
        forceSlected=true;
          if (Physics.Raycast(myRaycast.transform.position,Vector3.down, out hit,Mathf.Infinity, layerMask)){
            speed=GameManager.Instance.GetForce(hit.collider.tag);
            GameManager.Instance.SetSpeed(speed);
            GameManager.Instance.SetForce(hit.collider.tag);
            GameManager.Instance.GetCurrentCanon().GetComponent<CannonController>().ShootCannon();
            GameManager.Instance.GetCurrentCanon().GetComponent<CannonController>().setState("launch");     
            UIManager.Instance.tapOff();
            UIManager.Instance.slideOn();         
        }
    }
}