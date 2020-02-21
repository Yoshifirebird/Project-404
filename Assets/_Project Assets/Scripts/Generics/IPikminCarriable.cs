/*
 * IPikminCarriable.cs
 * Created by: Ambrosia
 * Created on: 16/2/2020 (dd/mm/yy)
 * Created for: needing a general interface for Pikmin to carry a given object
 */

using UnityEngine;

public interface IPikminCarriable
{
    void OnStartCarry();
    void OnEndCarry();

    void Move();
}
