using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {
    public bool rotateNow=false;
    public float rotationSmooth;
    public GameObject rotateTowards;
    private static EnvironmentController _instance;
    public static EnvironmentController Instance {

        get {
            if (_instance == null) {
                _instance = FindObjectOfType<EnvironmentController> ();
                if (_instance == null) {
                    GameObject go = new GameObject ();
                    go.name = typeof (EnvironmentController).Name;
                    _instance = go.AddComponent<EnvironmentController> ();
                }
            }
            return _instance;
        }
    }

    private void Awake () {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy (gameObject);
        }
    }
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if(rotateNow){
              // Smoothly rotates towards target 
            var targetPoint = rotateTowards.transform.position;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth); 
            if(Quaternion.Angle(targetRotation, transform.rotation)<0.0001f){
                rotateNow=false;
                GameManager.Instance.init();
            }  
        }
    }

    public void RotateEnv () {
        rotateNow=true;
    }
}