/*
 * PlayerStats.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: storing global data about the Players statistics
 */

public static class PlayerStats
{
    public static int _Day = 0;

    public static int _RedPikmin = 0;
    public static int _YellowPikmin = 0;
    public static int _BluePikmin = 0;
    public static int _TotalPikmin { get => _RedPikmin + _YellowPikmin + _BluePikmin; }

    public static void IncrementTotal(Colour pikminColour)
    {
        switch (pikminColour)
        {
            case Colour.Red:
                _RedPikmin++;
                break;
            case Colour.Blue:
                _BluePikmin++;
                break;
            case Colour.Yellow:
                _YellowPikmin++;
                break;
            default:
                break;
        }
    }

    public static void DecrementTotal(Colour pikminColour)
    {
        switch (pikminColour)
        {
            case Colour.Red:
                _RedPikmin--;
                break;
            case Colour.Blue:
                _BluePikmin--;
                break;
            case Colour.Yellow:
                _YellowPikmin--;
                break;
            default:
                break;
        }
    }
}