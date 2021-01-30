using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endline : MonoBehaviour
{
    public int path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player"){
            GameManager.Instance.AlignEnd();
           GameManager.Instance.StartFollowPath(path);
           UIManager.Instance.EndState();
            GameManager.Instance.StopMove();
        }
    }
}
