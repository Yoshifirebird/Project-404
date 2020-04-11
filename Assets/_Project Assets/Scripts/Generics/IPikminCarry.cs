/*
 * IPikminCarriable.cs
 * Created by: Ambrosia
 * Created on: 11/4/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to carry a given object
 */

public interface IPikminCarry
{
    void OnCarryStart(PikminBehavior p);
    void OnCarryLeave(PikminBehavior p);
}