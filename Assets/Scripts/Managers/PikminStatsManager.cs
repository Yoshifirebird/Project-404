/*
 * PikminStatsManager.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

using System.Collections.Generic;
using UnityEngine;

public enum PikminStatSpecifier {
  InSquad = 0,
  OnField,
  InOnion
}

// Specific information about a maturity of Pikmin
public class PikminMaturityStats {
  public PikminMaturity _Maturity;

  public int _InSquad = 0;
  public int _OnField = 0;
  public int _InOnion = 0;

  public int Total {
    get {
      // OnField contains InSquad as well, so we don't need to add it here
      return _OnField + _InOnion;
    }
  }

  public PikminMaturityStats (PikminMaturity maturity) {
    _Maturity = maturity;
  }

  // Prints out the information relevant to the stats of the Pikmin
  public void Print () {
    Debug.Log ($"{_Maturity.ToString()}\tInSquad: {_InSquad}, OnField: {_OnField}, InOnion: {_InOnion}");
  }

  public override string ToString () {
    return $"{_Maturity.ToString()}\tInSquad: {_InSquad}, OnField: {_OnField}, InOnion: {_InOnion}\n";
  }

  public void AddTo (PikminStatSpecifier specifier) {
    switch (specifier) {
      case PikminStatSpecifier.InSquad:
        _InSquad++;
        break;
      case PikminStatSpecifier.OnField:
        _OnField++;
        break;
      case PikminStatSpecifier.InOnion:
        _InOnion++;
        break;
      default:
        break;
    }
  }

  public void RemoveFrom (PikminStatSpecifier specifier) {
    switch (specifier) {
      case PikminStatSpecifier.InSquad:
        _InSquad--;
        break;
      case PikminStatSpecifier.OnField:
        _OnField--;
        break;
      case PikminStatSpecifier.InOnion:
        _InOnion--;
        break;
      default:
        break;
    }
  }
}

// Specific information about the type of Pikmin (Colour, and maturity)
public class PikminTypeStats {
  public PikminColour _Colour;

  public PikminMaturityStats _Leaf = new PikminMaturityStats (PikminMaturity.Leaf);
  public PikminMaturityStats _Bud = new PikminMaturityStats (PikminMaturity.Bud);
  public PikminMaturityStats _Flower = new PikminMaturityStats (PikminMaturity.Flower);

  public PikminTypeStats (PikminColour colour) {
    _Colour = colour;
  }

  // Prints out the information relevant to the stats of the Pikmin
  public void Print () {
    Debug.Log ($"\tCOLOUR\t{_Colour.ToString()}");
    _Leaf.Print ();
    _Bud.Print ();
    _Flower.Print ();
  }

  public override string ToString () {
    string str = $"\tCOLOUR\t{ _Colour.ToString()}\n";
    str += _Leaf.ToString ();
    str += _Bud.ToString ();
    str += _Flower.ToString ();
    return str;
  }

  // Adds a Pikmin to their specified matury level stats
  public void AddTo (PikminMaturity maturity, PikminStatSpecifier specifier) {
    switch (maturity) {
      case PikminMaturity.Leaf:
        _Leaf.AddTo (specifier);
        break;
      case PikminMaturity.Bud:
        _Bud.AddTo (specifier);
        break;
      case PikminMaturity.Flower:
        _Flower.AddTo (specifier);
        break;
      default:
        break;
    }
  }

  // Removes a Pikmin from their specified maturity level stats
  public void RemoveFrom (PikminMaturity maturity, PikminStatSpecifier specifier) {
    switch (maturity) {
      case PikminMaturity.Leaf:
        _Leaf.RemoveFrom (specifier);
        break;
      case PikminMaturity.Bud:
        _Bud.RemoveFrom (specifier);
        break;
      case PikminMaturity.Flower:
        _Flower.RemoveFrom (specifier);
        break;
      default:
        break;
    }
  }
}

public static class PikminStatsManager {
  // Stores specific stats of each colour
  public static PikminTypeStats _RedStats = new PikminTypeStats (PikminColour.Red);
  public static PikminTypeStats _BlueStats = new PikminTypeStats (PikminColour.Blue);
  public static PikminTypeStats _YellowStats = new PikminTypeStats (PikminColour.Yellow);

  public static List<GameObject> _InSquad = new List<GameObject> ();

  // Adds a Pikmin to the squad, and handles adding to the stats
  public static void AddToSquad (GameObject pikmin, PikminColour colour, PikminMaturity maturity) {
    _InSquad.Add (pikmin);
    Add (colour, maturity, PikminStatSpecifier.InSquad);
  }

  // Removes a Pikmin from the squad, and handles decrementing the stats
  public static void RemoveFromSquad (GameObject pikmin, PikminColour colour, PikminMaturity maturity) {
    _InSquad.Remove (pikmin);
    Remove (colour, maturity, PikminStatSpecifier.InSquad);
  }

  // Adds a Pikmin to the stats
  public static void Add (PikminColour colour, PikminMaturity maturity, PikminStatSpecifier specifier) {
    switch (colour) {
      case PikminColour.Red:
        _RedStats.AddTo (maturity, specifier);
        break;
      case PikminColour.Yellow:
        _YellowStats.AddTo (maturity, specifier);
        break;
      case PikminColour.Blue:
        _BlueStats.AddTo (maturity, specifier);
        break;
      default:
        break;
    }
  }

  // Removes a Pikmin from the stats
  public static void Remove (PikminColour colour, PikminMaturity maturity, PikminStatSpecifier specifier) {
    switch (colour) {
      case PikminColour.Red:
        _RedStats.RemoveFrom (maturity, specifier);
        break;
      case PikminColour.Yellow:
        _YellowStats.RemoveFrom (maturity, specifier);
        break;
      case PikminColour.Blue:
        _BlueStats.RemoveFrom (maturity, specifier);
        break;
      default:
        break;
    }
  }

  // Prints out the information relevant for the stats of the Pikmin
  public static void Print () {
    Debug.Log ($"Length of the 'InSquad' list: {_InSquad.Count}");
    _RedStats.Print ();
    _BlueStats.Print ();
    _YellowStats.Print ();
  }
}
