/*
 * GameManager.cs
 * Created by: Ambrosia
 * Created on: 30/4/2020 (dd/mm/yy)
 */

public enum Language
{ 
  English,
  French,
}

public static class GameManager {
  public static bool _IsPaused = false; // Used in checks to see if the game is paused
  public static bool _FunMode = false; // Used in checks when you want to mess around, enables odd features

  public static Language _Language = Language.English; // Used for alternate texts

  public static DayManager _DayManager = null;
  public static Player _Player = null; // Static reference to the Player in the current scene
}
