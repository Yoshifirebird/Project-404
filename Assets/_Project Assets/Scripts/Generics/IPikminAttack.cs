using UnityEngine;

public interface IPikminAttack
{
    void OnAttach(GameObject attachedPikmin);
    void Attack(GameObject attackingPikmin, int attackDamage);
}
