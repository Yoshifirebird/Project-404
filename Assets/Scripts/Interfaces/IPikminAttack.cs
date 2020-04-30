/*
 * IPikminAttack.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

// Not explicitly an interface, but acts like one
public abstract class IPikminAttack : IPikminInteractable {
  // Called when the Pikmin starts attacking the object, like when latched
  public abstract void OnAttackStart (PikminAI pikmin);
  // Called when the Pikmin changes states, or the object dies
  public abstract void OnAttackEnd (PikminAI pikmin);

  // Called when the Pikmin hits the object
  public abstract void OnAttackRecieve ();

  // Called when an idle Pikmin tries to look for an object to interact with
  public PikminIntention GetIntentionType () {
    return PikminIntention.Attack;
  }
}