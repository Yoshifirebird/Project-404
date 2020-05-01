/*
 * Test_PikminAttacking.cs
 * Created by: Ambrosia
 * Created on: 1/5/2020 (dd/mm/yy)
 */

using UnityEngine;

public class Test_PikminAttacking : MonoBehaviour, IPikminAttack {
  public PikminIntention IntentionType => PikminIntention.Attack;

  public void OnAttackEnd (PikminAI pikmin) {
    print ("PIKMIN HAS STOPPED ATTACKING ME");
  }

  public void OnAttackRecieve () {
    print ("PIKMIN IS ATTACKING ME");
  }

  public void OnAttackStart (PikminAI pikmin) {
    print ("PIKMIN HAS STARTED TO ATTACK ME");
  }
}