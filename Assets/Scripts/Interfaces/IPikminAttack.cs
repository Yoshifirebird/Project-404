/*
 * IPikminAttack.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

 public interface IPikminAttack
{
    // Called when the Pikmin starts attacking the object, like when latched
    public void OnAttackStart(PikminAI pikmin);
    // Called when the Pikmin changes states, or the object dies
    public void OnAttackEnd(PikminAI pikmin);

    // Called when the Pikmin hits the object
    public void OnAttackRecieve();
}
