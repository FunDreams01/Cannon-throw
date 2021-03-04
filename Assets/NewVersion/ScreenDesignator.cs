using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDesignator : MonoBehaviour
{

   //This variable designates the game state in which the Canvas will be visible.
   //I would use a struct for this if not for MonoBehaviour inheritance and Unity's stupid design choices.
   public States ScreenState; 
}
