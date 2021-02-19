using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building : MonoBehaviour
{ Rigidbody m_Rigidbody;

 private void Start()
{
   m_Rigidbody = GetComponent<Rigidbody>();
   m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous; 
}
    private void OnCollisionEnter(Collision other)
{
    if(other.gameObject.tag=="Player"){
        DamageManager.Instance.hitObstacle();
    }
}
}
