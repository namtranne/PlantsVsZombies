using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BowlingType", menuName = "Bowling")]
public class BowlingType : ScriptableObject
{
   public Sprite sprite;
   public bool canExplode;
}
