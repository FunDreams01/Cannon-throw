﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building : MonoBehaviour
{private void OnCollisionEnter(Collision other)
{
    if(other.gameObject.tag=="Player"){
       GameManager.Instance.Fall();
        CinemachineSwitcher.Instance.playAnim ("down");
        CinemachineSwitcher.Instance.StopFollowing("down");
    }
}
}