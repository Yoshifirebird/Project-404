/*
 * PlayerStats.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: storing global data about the Players statistics
 */

using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats {
    public static int _Day = 0;

    public static PikminStats _RedInSquad = new PikminStats (0, 0, 0);
    public static PikminStats _YellowInSquad = new PikminStats (0, 0, 0);
    public static PikminStats _BlueInSquad = new PikminStats (0, 0, 0);
    public static List<PikminBehavior> _InSquad = new List<PikminBehavior> (); // Contains squad

    public static PikminStats _RedOnField = new PikminStats (0, 0, 0);
    public static PikminStats _YellowOnField = new PikminStats (0, 0, 0);
    public static PikminStats _BlueOnField = new PikminStats (0, 0, 0);
    public static List<PikminBehavior> _OnField = new List<PikminBehavior> (); // Contains squad & not in squad

    public static PikminStats _RedTotal = new PikminStats (0, 0, 0);
    public static PikminStats _YellowTotal = new PikminStats (0, 0, 0);
    public static PikminStats _BlueTotal = new PikminStats (0, 0, 0); // Contains squad & not in squad & in onions

    public static int _TotalPikmin { get => _RedTotal.GetTotal () + _YellowTotal.GetTotal () + _BlueTotal.GetTotal (); }

    public static void PrintAll () {
        Debug.Log ("In Squad:");
        _RedInSquad.Print ();
        _YellowInSquad.Print ();
        _BlueInSquad.Print ();

        Debug.Log ("On Field:");
        _RedOnField.Print ();
        _YellowOnField.Print ();
        _BlueOnField.Print ();

        Debug.Log ("Total:");
        _RedTotal.Print ();
        _YellowTotal.Print ();
        _BlueTotal.Print ();
    }

    #region Getting Stats

    public static int GetInOnion (Colour col) {
        switch (col) {
            case Colour.Red:
                return _RedTotal - _RedOnField;
            case Colour.Blue:
                return _BlueTotal - _BlueOnField;
            case Colour.Yellow:
                return _YellowTotal - _YellowOnField;
            default:
                return default;
        }
    }

    public static void GetTotalStats (Colour col) {

    }

    public static void GetFieldStats (Colour col) {

    }

    public static void GetSquadStats (Colour col) {

    }

    #endregion

    public struct PikminStats {
        int _Leaf;
        int _Bud;
        int _Flower;

        public PikminStats (int leaf, int bud, int flower) {
            _Leaf = leaf;
            _Bud = bud;
            _Flower = flower;
        }

        public int GetTotal () => _Leaf + _Bud + _Flower;

        public void Increment (Headtype head) {
            switch (head) {
                case Headtype.Leaf:
                    _Leaf++;
                    break;
                case Headtype.Bud:
                    _Bud++;
                    break;
                case Headtype.Flower:
                    _Flower++;
                    break;
                default:
                    break;
            }
        }

        public void Decrement (Headtype head) {
            switch (head) {
                case Headtype.Leaf:
                    _Leaf--;
                    break;
                case Headtype.Bud:
                    _Bud--;
                    break;
                case Headtype.Flower:
                    _Flower--;
                    break;
                default:
                    break;
            }
        }

        public void Print () {
            Debug.Log ($"Leaf {_Leaf} Bud {_Bud} Flower {_Flower}");
        }

        public static int operator - (PikminStats first, PikminStats compareTo) {
            return first.GetTotal () - compareTo.GetTotal ();
        }
    }

}