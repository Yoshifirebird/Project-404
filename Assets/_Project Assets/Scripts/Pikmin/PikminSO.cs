/*
 * PikminSO.cs
 * Created by: Ambrosia
 * Created on: 10/2/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum Colour { Red, Blue, Yellow }
public enum Headtype { Leaf, Bud, Flower }

[CreateAssetMenu(fileName = "GenericPikminObject", menuName = "New Pikmin Type")]
public class PikminSO : ScriptableObject
{
    public Colour _Colour;
    public Headtype _HeadType;

    public float _MovementSpeed;
    public float _RotationSpeed;
    public float _Gravity;

    public int _AttackDamage;
}
