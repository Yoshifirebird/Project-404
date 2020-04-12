/*
 * IPikminAttack.cs
 * Created by: Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to attack a given object
 */

using UnityEngine;

public interface IPikminAttack
{
    void OnAttackStart(GameObject attachedPikmin);
    void OnAttackEnd(GameObject detachedPikmin);

    void Attack(GameObject attackingPikmin, float attackDamage);
}
