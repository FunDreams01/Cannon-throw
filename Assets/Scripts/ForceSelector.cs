using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceSelector : MonoBehaviour {
    [Header("Launch Forces")]
    public float normalForce = 5;
    public float goodForce = 10;
    public float perfectForce = 20;

    [Header("Arrow Speed")]
    public float animSpeed = 1;
    public bool forceSlected = false;
    GameObject myArrow;
    Animator arrowAnim;
    GameObject myRaycast;
    RaycastHit hit;
    int layerMask;

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
           //to be changed to ui behavior of orce selection
            Debug.Log(hit.transform.gameObject.tag);
            // cannon stops moving and shoots character
        }
    }

}