/*
 * PikminObject.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum PikminColour {
  Red = 0,
  Yellow,
  Blue
}

public enum PikminMaturity {
  Leaf = 0,
  Bud,
  Flower
}

[CreateAssetMenu (fileName = "NewPikminType", menuName = "New Pikmin Type")]
public class PikminObject : ScriptableObject {
  [Header ("Pikmin Specific")]
  public PikminColour _Colour;
  public PikminMaturity _StartingMaturity;

  [Header ("Movement")]
  public float _MaxMovementSpeed = 2.5f;
  public float _AccelerationSpeed = 10;
  public float _RotationSpeed = 10;

  [Header ("Idle")]
  public float _SearchRadius = 5;

  [Header ("Attacking")]
  public float _AttackDamage = 2.5f;
  public float _AttackDelay = 1;

  [Header ("Audio")]
  public float _AudioVolume = 1;
  public AudioClip _DeathNoise = null;
}