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
  public float _MaxMovementSpeed;
  public float _AccelerationSpeed;
  public float _StoppingDistance;
  public float _RotationSpeed;

  [Header ("Audio")]
  public float _AudioVolume;

  public AudioClip _DeathNoise;
}