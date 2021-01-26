using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float Speed;
    public Vector3 offset ;
    public GameObject target;
    void Update () {
        transform.position=target.transform.position+offset;
    }

}