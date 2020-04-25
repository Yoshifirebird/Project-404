/*
 * IPikminAttack.cs
 * Created by: Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to attack a given object
 */

using UnityEngine;

public interface IPikminAttack
{
    void OnAttackStart(PikminBehavior attachedPikmin);
    void OnAttackEnd(PikminBehavior detachedPikmin);

    void Attack(PikminBehavior attackingPikmin, float attackDamage);
}
