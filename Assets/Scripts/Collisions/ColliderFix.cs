using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFix : MonoBehaviour
{
    public string animationName;
    public Vector3 position;
    public Vector3 rotation;

     public ColliderFix(string _animationName, Vector3 _position, Vector3 _rotation){
        animationName = _animationName;
        position = _position;
        rotation = _rotation;
    }
}
