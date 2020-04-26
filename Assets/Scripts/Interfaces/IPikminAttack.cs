/*
 * IPikminAttack.cs
 * Created by: Ambrosia
 * Created on: 15/2/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to attack a given object
 */

using UnityEngine;

public interface IPikminAttack {
    // Called when Pikmin latch onto the object it's attacking
    void OnAttackStart (PikminBehavior attachedPikmin);
    // Called when Pikmin unlatch from the object it was attacking
    void OnAttackEnd (PikminBehavior detachedPikmin);

    // Called whenever the Pikmin 'Attacks', aka after a certain period of time has passed
    void Attack (PikminBehavior attackingPikmin, float attackDamage);
}