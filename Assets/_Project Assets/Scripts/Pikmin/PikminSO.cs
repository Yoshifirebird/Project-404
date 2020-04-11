/*
 * PikminSO.cs
 * Created by: Ambrosia
 * Created on: 10/2/2020 (dd/mm/yy)
 */

using UnityEngine;

public enum Colour { Red, Blue, Yellow, SIZE }
public enum Headtype { Leaf, Bud, Flower, SIZE }

[CreateAssetMenu(fileName = "GenericPikminObject", menuName = "Pikmin/New Pikmin Type")]
public class PikminSO : ScriptableObject
{
    public Colour _Colour;

    public GameObject _Leaf;
    public GameObject _Bud;
    public GameObject _Flower;

    public float _MovementSpeed;
    public float _RotationSpeed;

    public float _TimeBetweenAttacks;
    public float _AttackDamage;
}
