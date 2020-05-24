/*
 * DayManager.cs
 * Created by: Ambrosia
 * Created on: 23/5/2020 (dd/mm/yy)
 */

public enum DayState {
  Morning,
  Afternoon,
  Evening,
  CountDown,
  EndOfDay
}

// TODO: stubbed atm, expand upon this
public static class DayManager {
  public static int _CurrentDay = 0;
  // TODO: day limit? etc.

  public static int _MorningPeriod = 100;
  public static int _AfternoonPeriod = 100;
  public static int _EveningPeriod = 70;
  // Countdown Timer, starting from 10

  public static int _TotalDayTime => _MorningPeriod + _AfternoonPeriod + _EveningPeriod + 10;
}
