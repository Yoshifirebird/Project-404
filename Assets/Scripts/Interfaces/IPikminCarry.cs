/*
 * IPikminCarry.cs
 * Created by: Ambrosia
 * Created on: 11/4/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to carry a given object
 */

public interface IPikminCarry {
    // Called when a Pikmin starts carrying the object
    void OnCarryStart (PikminBehavior p);
    // Called when the Pikmin gets called by the whistle, or reached it's destination
    void OnCarryLeave (PikminBehavior p);

    // Returns whether or not the current amount of Pikmin carrying is > the maximum amount allowed
    bool PikminSpotAvailable ();
}