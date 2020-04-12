/*
 * PlayerStats.cs
 * Created by: Ambrosia
 * Created on: 12/2/2020 (dd/mm/yy)
 * Created for: storing global data about the Players statistics
 */

public static class PlayerStats
{
    public static int _Day = 0;

    public static PikminStats _Red = new PikminStats(0, 0, 0);
    public static PikminStats _Yellow = new PikminStats(0, 0, 0);
    public static PikminStats _Blue = new PikminStats(0, 0, 0);
    public static int _TotalPikmin { get => _Red.GetTotal() + _Yellow.GetTotal() + _Yellow.GetTotal(); }

    public static void IncrementTotal(Colour pikminColour, Headtype headtype)
    {
        switch (pikminColour)
        {
            case Colour.Red:
                _Red.Increment(headtype);
                break;
            case Colour.Yellow:
                _Yellow.Increment(headtype);
                break;
            case Colour.Blue:
                _Blue.Increment(headtype);
                break;
            default:
                break;
        }
    }

    public static void DecrementTotal(Colour pikminColour, Headtype headtype)
    {
        switch (pikminColour)
        {
            case Colour.Red:
                _Red.Decrement(headtype);
                break;
            case Colour.Yellow:
                _Yellow.Decrement(headtype);
                break;
            case Colour.Blue:
                _Blue.Decrement(headtype);
                break;
            default:
                break;
        }
    }

    public struct PikminStats
    {
        int _Leaf;
        int _Bud;
        int _Flower;
        public PikminStats(int leaf, int bud, int flower)
        {
            _Leaf = leaf;
            _Bud = bud;
            _Flower = flower;
        }

        public int GetTotal() => _Leaf + _Bud + _Flower;

        public void Increment(Headtype head)
        {
            switch (head)
            {
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
        public void Decrement(Headtype head)
        {
            switch (head)
            {
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

    }

}