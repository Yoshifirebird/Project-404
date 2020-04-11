/*
 * PikminSO.cs
 * Created by: Ambrosia, Kman
 * Created on: 10/2/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum Colour { Red, Blue, Yellow, SIZE }
public enum Headtype { Leaf, Bud, Flower, SIZE }

[CreateAssetMenu(fileName = "GenericPikminObject", menuName = "Pikmin/New Pikmin Type")]
public class PikminSO : ScriptableObject
{
    public Colour _Colour;

    [Header("Head Types")]
    public GameObject _Leaf;
    public GameObject _Bud;
    public GameObject _Flower;

    [Header("Movement")]
    public float _MovementSpeed;
    public float _RotationSpeed;
    public float _HeadSpeedMultiplier;

    [Header("Attacking")]
    public float _TimeBetweenAttacks;
    public float _AttackDamage;

    [Header("Carrying")]
    public float _SearchRange;

}
