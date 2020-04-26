/*
 * PikminSO.cs
 * Created by: Ambrosia, Kman
 * Created on: 10/2/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum Colour { Red, Blue, Yellow, SIZE }
public enum Headtype { Leaf, Bud, Flower, SIZE }

[CreateAssetMenu (fileName = "GenericPikminObject", menuName = "Pikmin/New Pikmin Type")]
public class PikminSO : ScriptableObject {
    public Colour _Colour = Colour.Red;

    [Header ("Head Types")]
    public GameObject _Leaf;
    public GameObject _Bud;
    public GameObject _Flower;

    [Header ("Movement")]
    public float _MovementSpeed = 5;
    public float _RotationSpeed = 15;
    public float _HeadSpeedMultiplier = 1.15f;

    [Header ("Awareness")]
    public float _HeightDifferenceMax = 1;
    public float _SearchRange = 5;

    [Header ("Attacking")]
    public float _AttackDamage = 2.5f;
    public float _AttackDistance = 1;
    public float _TimeBetweenAttacks = 0.5f;

    [Header ("Carrying")]
    public float _CarryDistance = 1;

}