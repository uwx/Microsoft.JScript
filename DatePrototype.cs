// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DatePrototype
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides static methods and properties for <see cref="T:Microsoft.JScript.DateObject" /> objects. All methods of this class are called by passing in a reference to the object that is requesting access to the method. This class belongs to the built-in object model category.</summary>
public class DatePrototype : DateObject
{
  internal static readonly DatePrototype ob = new DatePrototype(ObjectPrototype.ob);
  internal static DateConstructor _constructor;
  private const double HoursPerDay = 24.0;
  private const double MinutesPerHour = 60.0;
  private const double SecondsPerMinute = 60.0;
  private const double msPerSecond = 1000.0;
  private const double msPerMinute = 60000.0;
  private const double msPerHour = 3600000.0;
  private const double msPerDay = 86400000.0;
  internal const double msTo1970 = 62135596800000.0;
  internal const double ticksPerMillisecond = 10000.0;
  internal const double maxDate = 8.64E+15;
  internal const double minDate = -8.64E+15;
  private static readonly int[] daysToMonthEnd = new int[12]
  {
    31 /*0x1F*/,
    59,
    90,
    120,
    151,
    181,
    212,
    243,
    273,
    304,
    334,
    365
  };
  private static readonly int[] leapDaysToMonthEnd = new int[12]
  {
    31 /*0x1F*/,
    60,
    91,
    121,
    152,
    182,
    213,
    244,
    274,
    305,
    335,
    366
  };
  private static readonly double localStandardTZA;
  private static readonly double localDaylightTZA;
  private static readonly bool useDST;
  private static readonly string[] WeekDayName = new string[7]
  {
    "Sun",
    "Mon",
    "Tue",
    "Wed",
    "Thu",
    "Fri",
    "Sat"
  };
  private static readonly string[] MonthName = new string[12]
  {
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec"
  };
  private static readonly string[] Strings = new string[37]
  {
    "bc",
    "b.c",
    "ad",
    "a.d",
    "am",
    "a.m",
    "pm",
    "p.m",
    "est",
    "edt",
    "cst",
    "cdt",
    "mst",
    "mdt",
    "pst",
    "pdt",
    "gmt",
    "utc",
    "sunday",
    "monday",
    "tuesday",
    "wednesday",
    "thursday",
    "friday",
    "saturday",
    "january",
    "february",
    "march",
    "april",
    "may",
    "june",
    "july",
    "august",
    "september",
    "october",
    "november",
    "december"
  };
  private static readonly Tk[] Tokens = new Tk[37]
  {
    Tk.BcAd,
    Tk.BcAd,
    Tk.BcAd,
    Tk.BcAd,
    Tk.AmPm,
    Tk.AmPm,
    Tk.AmPm,
    Tk.AmPm,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Zone,
    Tk.Day,
    Tk.Day,
    Tk.Day,
    Tk.Day,
    Tk.Day,
    Tk.Day,
    Tk.Day,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month,
    Tk.Month
  };
  private static readonly int[] Values = new int[37]
  {
    -1,
    -1,
    1,
    1,
    -1,
    -1,
    1,
    1,
    -300,
    -240,
    -360,
    -300,
    -420,
    -360,
    -480,
    -420,
    0,
    0,
    0,
    1,
    2,
    3,
    4,
    5,
    6,
    0,
    1,
    2,
    3,
    4,
    5,
    6,
    7,
    8,
    9,
    10,
    11
  };

  internal DatePrototype(ObjectPrototype parent)
    : base((ScriptObject) parent, 0.0)
  {
    this.noExpando = true;
  }

  private static double Day(double time) => Math.Floor(time / 86400000.0);

  private static double TimeWithinDay(double time)
  {
    double num = time % 86400000.0;
    if (num < 0.0)
      num += 86400000.0;
    return num;
  }

  private static int DaysInYear(double year)
  {
    return year % 4.0 != 0.0 || year % 100.0 == 0.0 && year % 400.0 != 0.0 ? 365 : 366;
  }

  private static double DayFromYear(double year)
  {
    return 365.0 * (year - 1970.0) + Math.Floor((year - 1969.0) / 4.0) - Math.Floor((year - 1901.0) / 100.0) + Math.Floor((year - 1601.0) / 400.0);
  }

  private static double TimeFromYear(double year) => 86400000.0 * DatePrototype.DayFromYear(year);

  private static double YearFromTime(double time)
  {
    double num1 = Math.Floor(time / 86400000.0);
    double year = 1970.0 + Math.Floor((400.0 * num1 + 398.0) / 146097.0);
    double num2 = DatePrototype.DayFromYear(year);
    if (num1 < num2)
      --year;
    return year;
  }

  private static bool InLeapYear(double year)
  {
    return year % 4.0 == 0.0 && (year % 100.0 != 0.0 || year % 400.0 == 0.0);
  }

  private static int MonthFromTime(double time)
  {
    int index = 0;
    int num = DatePrototype.DayWithinYear(time) + 1;
    if (DatePrototype.InLeapYear(DatePrototype.YearFromTime(time)))
    {
      while (num > DatePrototype.leapDaysToMonthEnd[index])
        ++index;
    }
    else
    {
      while (num > DatePrototype.daysToMonthEnd[index])
        ++index;
    }
    return index;
  }

  private static int DayWithinYear(double time)
  {
    return (int) (DatePrototype.Day(time) - DatePrototype.DayFromYear(DatePrototype.YearFromTime(time)));
  }

  private static int DateFromTime(double time)
  {
    int index = 0;
    int num = DatePrototype.DayWithinYear(time) + 1;
    if (num <= 31 /*0x1F*/)
      return num;
    if (DatePrototype.InLeapYear(DatePrototype.YearFromTime(time)))
    {
      while (num > DatePrototype.leapDaysToMonthEnd[index])
        ++index;
      return num - DatePrototype.leapDaysToMonthEnd[index - 1];
    }
    while (num > DatePrototype.daysToMonthEnd[index])
      ++index;
    return num - DatePrototype.daysToMonthEnd[index - 1];
  }

  private static int WeekDay(double time)
  {
    double num = (DatePrototype.Day(time) + 4.0) % 7.0;
    if (num < 0.0)
      num += 7.0;
    return (int) num;
  }

  private static bool DaylightSavingsTime(double localTime)
  {
    if (!DatePrototype.useDST)
      return false;
    double ticks = (localTime + 62135596800000.0) * 10000.0;
    if ((double) long.MinValue <= ticks)
    {
      if (ticks <= (double) long.MaxValue)
      {
        try
        {
          return TimeZone.CurrentTimeZone.IsDaylightSavingTime(new DateTime((long) ticks));
        }
        catch (ArgumentOutOfRangeException ex)
        {
        }
      }
    }
    int num1 = DatePrototype.MonthFromTime(localTime);
    if (num1 < 3 || num1 > 9)
      return false;
    if (num1 > 3 && num1 < 9)
      return true;
    int num2 = DatePrototype.DateFromTime(localTime);
    if (num1 == 3)
    {
      if (num2 > 7)
        return true;
      int num3 = DatePrototype.WeekDay(localTime);
      return num3 > 0 ? num2 > num3 : DatePrototype.HourFromTime(localTime) > 1;
    }
    if (num2 < 25)
      return true;
    int num4 = DatePrototype.WeekDay(localTime);
    return num4 > 0 ? num2 - num4 < 25 : DatePrototype.HourFromTime(localTime) < 1;
  }

  static DatePrototype()
  {
    DateTime dateTime1 = new DateTime(DateTime.Now.Year, 1, 1);
    double num1 = (double) (dateTime1.Ticks - dateTime1.ToUniversalTime().Ticks) / 10000.0;
    DateTime dateTime2 = new DateTime(DateTime.Now.Year, 7, 1);
    double num2 = (double) (dateTime2.Ticks - dateTime2.ToUniversalTime().Ticks) / 10000.0;
    if (num1 < num2)
    {
      DatePrototype.localStandardTZA = num1;
      DatePrototype.localDaylightTZA = num2;
    }
    else
    {
      DatePrototype.localStandardTZA = num2;
      DatePrototype.localDaylightTZA = num1;
    }
    DatePrototype.useDST = DatePrototype.localStandardTZA != DatePrototype.localDaylightTZA;
  }

  private static double LocalTime(double utcTime)
  {
    return utcTime + (DatePrototype.DaylightSavingsTime(utcTime + DatePrototype.localStandardTZA) ? DatePrototype.localDaylightTZA : DatePrototype.localStandardTZA);
  }

  internal static double UTC(double localTime)
  {
    return localTime - (DatePrototype.DaylightSavingsTime(localTime) ? DatePrototype.localDaylightTZA : DatePrototype.localStandardTZA);
  }

  private static int HourFromTime(double time)
  {
    double num = Math.Floor(time / 3600000.0) % 24.0;
    if (num < 0.0)
      num += 24.0;
    return (int) num;
  }

  private static int MinFromTime(double time)
  {
    double num = Math.Floor(time / 60000.0) % 60.0;
    if (num < 0.0)
      num += 60.0;
    return (int) num;
  }

  private static int SecFromTime(double time)
  {
    double num = Math.Floor(time / 1000.0) % 60.0;
    if (num < 0.0)
      num += 60.0;
    return (int) num;
  }

  private static int msFromTime(double time)
  {
    double num = time % 1000.0;
    if (num < 0.0)
      num += 1000.0;
    return (int) num;
  }

  internal static double MakeTime(double hour, double min, double sec, double ms)
  {
    if (double.IsInfinity(hour) || double.IsInfinity(min) || double.IsInfinity(sec) || double.IsInfinity(ms) || hour != hour || min != min || sec != sec || ms != ms)
      return double.NaN;
    hour = (double) (int) Runtime.DoubleToInt64(hour);
    min = (double) (int) Runtime.DoubleToInt64(min);
    sec = (double) (int) Runtime.DoubleToInt64(sec);
    ms = (double) (int) Runtime.DoubleToInt64(ms);
    return hour * 3600000.0 + min * 60000.0 + sec * 1000.0 + ms;
  }

  internal static double MakeDay(double year, double month, double date)
  {
    if (double.IsInfinity(year) || double.IsInfinity(month) || double.IsInfinity(date) || year != year || month != month || date != date)
      return double.NaN;
    year = (double) (int) Runtime.DoubleToInt64(year);
    month = (double) (int) Runtime.DoubleToInt64(month);
    date = (double) (int) Runtime.DoubleToInt64(date);
    year += Math.Floor(month / 12.0);
    month %= 12.0;
    if (month < 0.0)
      month += 12.0;
    double num = 0.0;
    if (month > 0.0)
      num = !DatePrototype.InLeapYear((double) (int) Runtime.DoubleToInt64(year)) ? (double) DatePrototype.daysToMonthEnd[(int) (month - 1.0)] : (double) DatePrototype.leapDaysToMonthEnd[(int) (month - 1.0)];
    return DatePrototype.DayFromYear(year) - 1.0 + num + date;
  }

  internal static double MakeDate(double day, double time)
  {
    return double.IsInfinity(day) || double.IsInfinity(time) ? double.NaN : day * 86400000.0 + time;
  }

  internal static double TimeClip(double time)
  {
    return double.IsInfinity(time) || -8.64E+15 > time || time > 8.64E+15 ? double.NaN : (double) (long) time;
  }

  internal static string DateToLocaleDateString(double time)
  {
    if (double.IsNaN(time))
      return "NaN";
    StringBuilder stringBuilder = new StringBuilder();
    int num1 = DatePrototype.MonthFromTime(time) + 1;
    if (num1 < 10)
      stringBuilder.Append("0");
    stringBuilder.Append(num1);
    stringBuilder.Append("/");
    int num2 = DatePrototype.DateFromTime(time);
    if (num2 < 10)
      stringBuilder.Append("0");
    stringBuilder.Append(num2);
    stringBuilder.Append("/");
    stringBuilder.Append(DatePrototype.YearString(time));
    return stringBuilder.ToString();
  }

  internal static string DateToLocaleString(double time)
  {
    if (double.IsNaN(time))
      return "NaN";
    StringBuilder sb = new StringBuilder();
    int num1 = DatePrototype.MonthFromTime(time) + 1;
    if (num1 < 10)
      sb.Append("0");
    sb.Append(num1);
    sb.Append("/");
    int num2 = DatePrototype.DateFromTime(time);
    if (num2 < 10)
      sb.Append("0");
    sb.Append(num2);
    sb.Append("/");
    sb.Append(DatePrototype.YearString(time));
    sb.Append(" ");
    DatePrototype.AppendTime(time, sb);
    return sb.ToString();
  }

  internal static string DateToLocaleTimeString(double time)
  {
    if (double.IsNaN(time))
      return "NaN";
    StringBuilder sb = new StringBuilder();
    DatePrototype.AppendTime(time, sb);
    return sb.ToString();
  }

  private static void AppendTime(double time, StringBuilder sb)
  {
    int num1 = DatePrototype.HourFromTime(time);
    if (num1 < 10)
      sb.Append("0");
    sb.Append(num1);
    sb.Append(":");
    int num2 = DatePrototype.MinFromTime(time);
    if (num2 < 10)
      sb.Append("0");
    sb.Append(num2);
    sb.Append(":");
    int num3 = DatePrototype.SecFromTime(time);
    if (num3 < 10)
      sb.Append("0");
    sb.Append(num3);
  }

  private static string TimeZoneID(double utcTime)
  {
    int num = (int) (DatePrototype.localStandardTZA / 3600000.0);
    if (DatePrototype.DaylightSavingsTime(utcTime + DatePrototype.localStandardTZA))
    {
      switch (num)
      {
        case -8:
          return "PDT";
        case -7:
          return "MDT";
        case -6:
          return "CDT";
        case -5:
          return "EDT";
      }
    }
    else
    {
      switch (num)
      {
        case -8:
          return "PST";
        case -7:
          return "MST";
        case -6:
          return "CST";
        case -5:
          return "EST";
      }
    }
    return (num >= 0 ? "UTC+" : "UTC") + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  private static string YearString(double time)
  {
    double num = DatePrototype.YearFromTime(time);
    return num > 0.0 ? num.ToString((IFormatProvider) CultureInfo.InvariantCulture) : (1.0 - num).ToString((IFormatProvider) CultureInfo.InvariantCulture) + " B.C.";
  }

  internal static string DateToDateString(double utcTime)
  {
    if (double.IsNaN(utcTime))
      return "NaN";
    StringBuilder stringBuilder = new StringBuilder();
    double time = DatePrototype.LocalTime(utcTime);
    stringBuilder.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(time)]);
    stringBuilder.Append(" ");
    int index = DatePrototype.MonthFromTime(time);
    stringBuilder.Append(DatePrototype.MonthName[index]);
    stringBuilder.Append(" ");
    stringBuilder.Append(DatePrototype.DateFromTime(time));
    stringBuilder.Append(" ");
    stringBuilder.Append(DatePrototype.YearString(time));
    return stringBuilder.ToString();
  }

  internal static string DateToString(double utcTime)
  {
    if (double.IsNaN(utcTime))
      return "NaN";
    StringBuilder sb = new StringBuilder();
    double time = DatePrototype.LocalTime(utcTime);
    sb.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(time)]);
    sb.Append(" ");
    int index = DatePrototype.MonthFromTime(time);
    sb.Append(DatePrototype.MonthName[index]);
    sb.Append(" ");
    sb.Append(DatePrototype.DateFromTime(time));
    sb.Append(" ");
    DatePrototype.AppendTime(time, sb);
    sb.Append(" ");
    sb.Append(DatePrototype.TimeZoneID(utcTime));
    sb.Append(" ");
    sb.Append(DatePrototype.YearString(time));
    return sb.ToString();
  }

  internal static string DateToTimeString(double utcTime)
  {
    if (double.IsNaN(utcTime))
      return "NaN";
    StringBuilder sb = new StringBuilder();
    DatePrototype.AppendTime(DatePrototype.LocalTime(utcTime), sb);
    sb.Append(" ");
    sb.Append(DatePrototype.TimeZoneID(utcTime));
    return sb.ToString();
  }

  internal static string UTCDateToString(double utcTime)
  {
    if (double.IsNaN(utcTime))
      return "NaN";
    StringBuilder sb = new StringBuilder();
    sb.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(utcTime)]);
    sb.Append(", ");
    sb.Append(DatePrototype.DateFromTime(utcTime));
    sb.Append(" ");
    sb.Append(DatePrototype.MonthName[DatePrototype.MonthFromTime(utcTime)]);
    sb.Append(" ");
    sb.Append(DatePrototype.YearString(utcTime));
    sb.Append(" ");
    DatePrototype.AppendTime(utcTime, sb);
    sb.Append(" UTC");
    return sb.ToString();
  }

  private static bool NotSpecified(object value) => value == null || value is Missing;

  private static bool isASCII(char ch) => ch < '\u0080';

  private static bool isalpha(char ch)
  {
    if ('A' <= ch && ch <= 'Z')
      return true;
    return 'a' <= ch && ch <= 'z';
  }

  private static bool isdigit(char ch) => '0' <= ch && ch <= '9';

  internal static double ParseDate(string str)
  {
    long num1 = 2147483648 /*0x80000000*/;
    int num2 = 0;
    int num3 = 0;
    Ps ps = Ps.Initial;
    long year = num1;
    long month = num1;
    long date = num1;
    long num4 = num1;
    long num5 = num1;
    long num6 = num1;
    str = str.ToLowerInvariant();
    int num7 = 0;
    int length1 = str.Length;
    while (num7 < length1)
    {
      char ch1 = str[num7++];
      if (ch1 > ' ')
      {
        switch (ch1)
        {
          case '(':
            int num8 = 1;
            while (num7 < length1)
            {
              switch (str[num7++])
              {
                case '(':
                  ++num8;
                  continue;
                case ')':
                  if (--num8 <= 0)
                    goto label_113;
                  continue;
                default:
                  continue;
              }
            }
            continue;
          case '+':
            if (num1 != year)
            {
              ps = Ps.AddOffset;
              continue;
            }
            continue;
          case ',':
          case '/':
          case ':':
            continue;
          case '-':
            if (num1 != year)
            {
              ps = Ps.SubOffset;
              continue;
            }
            continue;
          default:
            if (DatePrototype.isalpha(ch1))
            {
              int num9 = num7 - 1;
              while (num7 < length1)
              {
                ch1 = str[num7++];
                if (!DatePrototype.isalpha(ch1) && '.' != ch1)
                  break;
              }
              int length2 = num7 - num9 - (num7 < length1 ? 1 : 0);
              if ('.' == str[num7 - (num7 < length1 ? 2 : 1)])
                --length2;
              while (ch1 == ' ' && num7 < length1)
                ch1 = str[num7++];
              if (1 == length2)
              {
                if (num1 != num5)
                  return double.NaN;
                char ch2 = str[num9];
                if (ch2 <= 'm')
                {
                  if (ch2 == 'j' || ch2 < 'a')
                    return double.NaN;
                  num5 = -(long) ((int) ch2 - 97 + (ch2 < 'j' ? 1 : 0)) * 60L;
                }
                else if (ch2 <= 'y')
                {
                  num5 = (long) ((int) ch2 - 109) * 60L;
                }
                else
                {
                  if (ch2 != 'z')
                    return double.NaN;
                  num5 = 0L;
                }
                ps = '+' != ch1 ? ('-' != ch1 ? Ps.Initial : Ps.SubOffset) : Ps.AddOffset;
                continue;
              }
              for (int index = DatePrototype.Strings.Length - 1; index >= 0; --index)
              {
                string strB = DatePrototype.Strings[index];
                if (strB.Length >= length2)
                {
                  if (string.CompareOrdinal(str, num9, strB, 0, length2) != 0)
                  {
                    if (index == 0)
                      return double.NaN;
                  }
                  else
                  {
                    switch (DatePrototype.Tokens[index])
                    {
                      case Tk.BcAd:
                        if (num3 != 0)
                          return double.NaN;
                        num3 = DatePrototype.Values[index];
                        goto label_57;
                      case Tk.AmPm:
                        if (num2 != 0)
                          return double.NaN;
                        num2 = DatePrototype.Values[index];
                        goto label_57;
                      case Tk.Zone:
                        if (num1 != num5)
                          return double.NaN;
                        num5 = (long) DatePrototype.Values[index];
                        if ('+' == ch1)
                        {
                          ps = Ps.AddOffset;
                          ++num7;
                          goto label_57;
                        }
                        if ('-' == ch1)
                        {
                          ps = Ps.SubOffset;
                          ++num7;
                          goto label_57;
                        }
                        ps = Ps.Initial;
                        goto label_57;
                      case Tk.Month:
                        if (num1 != month)
                          return double.NaN;
                        month = (long) DatePrototype.Values[index];
                        goto label_57;
                      default:
                        goto label_57;
                    }
                  }
                }
              }
label_57:
              if (num7 < length1)
              {
                --num7;
                continue;
              }
              continue;
            }
            if (!DatePrototype.isdigit(ch1))
              return double.NaN;
            int num10 = 0;
            int num11 = num7;
            do
            {
              num10 = num10 * 10 + (int) ch1 - 48 /*0x30*/;
              if (num7 < length1)
                ch1 = str[num7++];
              else
                break;
            }
            while (DatePrototype.isdigit(ch1));
            if (num7 - num11 > 6)
              return double.NaN;
            while (ch1 == ' ' && num7 < length1)
              ch1 = str[num7++];
            switch (ps)
            {
              case Ps.Minutes:
                if (num10 >= 60)
                  return double.NaN;
                num4 += (long) (num10 * 60);
                if (ch1 == ':')
                {
                  ps = Ps.Seconds;
                  continue;
                }
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              case Ps.Seconds:
                if (num10 >= 60)
                  return double.NaN;
                num4 += (long) num10;
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              case Ps.AddOffset:
                if (num1 != num6)
                  return double.NaN;
                num6 = num10 < 24 ? (long) (num10 * 60) : (long) (num10 % 100 + num10 / 100 * 60);
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              case Ps.SubOffset:
                if (num1 != num6)
                  return double.NaN;
                num6 = num10 < 24 ? (long) (-num10 * 60) : (long) -(num10 % 100 + num10 / 100 * 60);
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              case Ps.Date:
                if (num1 != date)
                  return double.NaN;
                date = (long) num10;
                if ('/' == ch1 || '-' == ch1)
                {
                  ps = Ps.Year;
                  continue;
                }
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              case Ps.Year:
                if (num1 != year)
                  return double.NaN;
                year = (long) num10;
                ps = Ps.Initial;
                if (num7 < length1)
                {
                  --num7;
                  continue;
                }
                continue;
              default:
                if (num10 >= 70)
                {
                  if (num1 != year)
                    return double.NaN;
                  year = (long) num10;
                  if (num7 < length1)
                  {
                    --num7;
                    continue;
                  }
                  continue;
                }
                switch (ch1)
                {
                  case '-':
                  case '/':
                    if (num1 != month)
                      return double.NaN;
                    month = (long) (num10 - 1);
                    ps = Ps.Date;
                    continue;
                  case ':':
                    if (num1 != num4 || num10 >= 24)
                      return double.NaN;
                    num4 = (long) (num10 * 3600);
                    ps = Ps.Minutes;
                    continue;
                  default:
                    if (num1 != date)
                      return double.NaN;
                    date = (long) num10;
                    if (num7 < length1)
                    {
                      --num7;
                      continue;
                    }
                    continue;
                }
            }
        }
      }
      else
        continue;
label_113:;
    }
    if (num1 == year || num1 == month || num1 == date)
      return double.NaN;
    if (num3 != 0)
    {
      if (num3 < 0)
        year = -year + 1L;
    }
    else if (year < 100L)
      year += 1900L;
    if (num2 != 0)
    {
      if (num1 == num4)
        return double.NaN;
      if (num4 >= 43200L && num4 < 46800L)
      {
        if (num2 < 0)
          num4 -= 43200L;
      }
      else if (num2 > 0)
      {
        if (num4 >= 43200L)
          return double.NaN;
        num4 += 43200L;
      }
    }
    else if (num1 == num4)
      num4 = 0L;
    bool flag = false;
    if (num1 != num5)
    {
      num4 -= num5 * 60L;
      flag = true;
    }
    if (num1 != num6)
      num4 -= num6 * 60L;
    double localTime = DatePrototype.MakeDate(DatePrototype.MakeDay((double) year, (double) month, (double) date), (double) (num4 * 1000L));
    if (!flag)
      localTime = DatePrototype.UTC(localTime);
    return localTime;
  }

  /// <summary>Gets a reference to a prototype-based object that is using this prototype.</summary>
  /// <returns>A reference to a prototype-based object.</returns>
  public static DateConstructor constructor => DatePrototype._constructor;

  /// <summary>Gets the date value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The date value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getDate)]
  public static double getDate(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.DateFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the day-of-the-week value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The day-of-the-week value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getDay)]
  public static double getDay(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.WeekDay(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the year value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The year value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getFullYear)]
  public static double getFullYear(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : DatePrototype.YearFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the hour value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The hour value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getHours)]
  public static double getHours(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.HourFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the millisecond value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The millisecond value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMilliseconds)]
  public static double getMilliseconds(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.msFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the minutes value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The minutes value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMinutes)]
  public static double getMinutes(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.MinFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the month value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The month value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMonth)]
  public static double getMonth(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.MonthFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the seconds value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The seconds value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getSeconds)]
  public static double getSeconds(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (double) DatePrototype.SecFromTime(DatePrototype.LocalTime(utcTime));
  }

  /// <summary>Gets the time value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The time value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getTime)]
  public static double getTime(object thisob)
  {
    return thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Gets the offset of the specified <see cref="T:Microsoft.JScript.DateObject" /> from Coordinated Universal Time (UTC).</summary>
  /// <returns>The difference between <paramref name="thisob" /> and Coordinated Universal Time (UTC).</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getTimezoneOffset)]
  public static double getTimezoneOffset(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return utcTime != utcTime ? utcTime : (utcTime - DatePrototype.LocalTime(utcTime)) / 60000.0;
  }

  /// <summary>Gets the date value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The date value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCDate)]
  public static double getUTCDate(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.DateFromTime(time);
  }

  /// <summary>Gets the day-of-the-week value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The day-of-the-week value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCDay)]
  public static double getUTCDay(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.WeekDay(time);
  }

  /// <summary>Gets the year value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The year value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCFullYear)]
  public static double getUTCFullYear(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : DatePrototype.YearFromTime(time);
  }

  /// <summary>Gets the hour value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The hour value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCHours)]
  public static double getUTCHours(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.HourFromTime(time);
  }

  /// <summary>Gets the millisecond value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The millisecond value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMilliseconds)]
  public static double getUTCMilliseconds(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.msFromTime(time);
  }

  /// <summary>Gets the minutes value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The minutes value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMinutes)]
  public static double getUTCMinutes(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.MinFromTime(time);
  }

  /// <summary>Gets the month value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The month value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMonth)]
  public static double getUTCMonth(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.MonthFromTime(time);
  }

  /// <summary>Gets the seconds value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The seconds value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCSeconds)]
  public static double getUTCSeconds(object thisob)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    return time != time ? time : (double) DatePrototype.SecFromTime(time);
  }

  /// <summary>Returns the specified <see cref="T:Microsoft.JScript.DateObject" /> as a <see cref="T:System.DateTime" /> object.</summary>
  /// <returns>The value of <paramref name="thisob" />, expressed as a <see cref="T:System.DateTime" /> object.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getVarDate)]
  public static object getVarDate(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    if (utcTime != utcTime)
      return (object) null;
    long ticks;
    try
    {
      ticks = checked ((long) unchecked (DatePrototype.LocalTime(utcTime) + 62135596800000.0) * 10000L);
    }
    catch (OverflowException ex)
    {
      return (object) null;
    }
    if (ticks >= DateTime.MinValue.Ticks)
    {
      if (ticks <= DateTime.MaxValue.Ticks)
      {
        DateTime varDate;
        try
        {
          varDate = new DateTime(ticks);
        }
        catch (ArgumentOutOfRangeException ex)
        {
          return (object) null;
        }
        return (object) varDate;
      }
    }
    return (object) null;
  }

  /// <summary>Gets the year value of the specified <see cref="T:Microsoft.JScript.DateObject" />, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The year value of <paramref name="thisob" />, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [NotRecommended("getYear")]
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getYear)]
  public static double getYear(object thisob)
  {
    double utcTime = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    if (utcTime != utcTime)
      return utcTime;
    double num = DatePrototype.YearFromTime(DatePrototype.LocalTime(utcTime));
    return 1900.0 <= num && num <= 1999.0 ? num - 1900.0 : num;
  }

  /// <summary>Sets the date value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified date value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="ddate">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setDate)]
  public static double setDate(object thisob, double ddate)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.MakeDay(DatePrototype.YearFromTime(time), (double) DatePrototype.MonthFromTime(time), ddate), DatePrototype.TimeWithinDay(time))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the year value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified year value.</summary>
  /// <returns>The year value that is set for <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dyear">The year value to set for <paramref name="thisob" />.</param>
  /// <param name="month">The month value to set for <paramref name="thisob" />.</param>
  /// <param name="date">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setFullYear)]
  public static double setFullYear(object thisob, double dyear, object month, object date)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double month1 = DatePrototype.NotSpecified(month) ? (double) DatePrototype.MonthFromTime(time) : Convert.ToNumber(month);
    double date1 = DatePrototype.NotSpecified(date) ? (double) DatePrototype.DateFromTime(time) : Convert.ToNumber(date);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.MakeDay(dyear, month1, date1), DatePrototype.TimeWithinDay(time))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the hour value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified hour value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dhour">The hour value to set for <paramref name="thisob" />.</param>
  /// <param name="min">The minutes value to set for <paramref name="thisob" />.</param>
  /// <param name="sec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setHours)]
  public static double setHours(object thisob, double dhour, object min, object sec, object msec)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double min1 = DatePrototype.NotSpecified(min) ? (double) DatePrototype.MinFromTime(time) : Convert.ToNumber(min);
    double sec1 = DatePrototype.NotSpecified(sec) ? (double) DatePrototype.SecFromTime(time) : Convert.ToNumber(sec);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime(dhour, min1, sec1, ms))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the minutes value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified minutes value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmin">The minutes value to set for <paramref name="thisob" />.</param>
  /// <param name="sec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMinutes)]
  public static double setMinutes(object thisob, double dmin, object sec, object msec)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double sec1 = DatePrototype.NotSpecified(sec) ? (double) DatePrototype.SecFromTime(time) : Convert.ToNumber(sec);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), dmin, sec1, ms))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the millisecond value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified millisecond value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmsec">The millisecond value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMilliseconds)]
  public static double setMilliseconds(object thisob, double dmsec)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), (double) DatePrototype.MinFromTime(time), (double) DatePrototype.SecFromTime(time), dmsec))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the month value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified month value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmonth">The month value to set for <paramref name="thisob" />.</param>
  /// <param name="date">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMonth)]
  public static double setMonth(object thisob, double dmonth, object date)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double date1 = DatePrototype.NotSpecified(date) ? (double) DatePrototype.DateFromTime(time) : Convert.ToNumber(date);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.MakeDay(DatePrototype.YearFromTime(time), dmonth, date1), DatePrototype.TimeWithinDay(time))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the seconds value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified seconds value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dsec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setSeconds)]
  public static double setSeconds(object thisob, double dsec, object msec)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), (double) DatePrototype.MinFromTime(time), dsec, ms))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the time value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified time value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="time">The time value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setTime)]
  public static double setTime(object thisob, double time)
  {
    if (!(thisob is DateObject))
      throw new JScriptException(JSError.DateExpected);
    time = DatePrototype.TimeClip(time);
    ((DateObject) thisob).value = time;
    return time;
  }

  /// <summary>Sets the date value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified date value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="ddate">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCDate)]
  public static double setUTCDate(object thisob, double ddate)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.MakeDay(DatePrototype.YearFromTime(time), (double) DatePrototype.MonthFromTime(time), ddate), DatePrototype.TimeWithinDay(time)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the year value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified year value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dyear">The year value to set for <paramref name="thisob" />.</param>
  /// <param name="month">The month value to set for <paramref name="thisob" />.</param>
  /// <param name="date">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCFullYear)]
  public static double setUTCFullYear(object thisob, double dyear, object month, object date)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double month1 = DatePrototype.NotSpecified(month) ? (double) DatePrototype.MonthFromTime(time) : Convert.ToNumber(month);
    double date1 = DatePrototype.NotSpecified(date) ? (double) DatePrototype.DateFromTime(time) : Convert.ToNumber(date);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.MakeDay(dyear, month1, date1), DatePrototype.TimeWithinDay(time)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the hour value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified hour value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dhour">The hour value to set for <paramref name="thisob" />.</param>
  /// <param name="min">The minutes value to set for <paramref name="thisob" />.</param>
  /// <param name="sec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCHours)]
  public static double setUTCHours(
    object thisob,
    double dhour,
    object min,
    object sec,
    object msec)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double min1 = DatePrototype.NotSpecified(min) ? (double) DatePrototype.MinFromTime(time) : Convert.ToNumber(min);
    double sec1 = DatePrototype.NotSpecified(sec) ? (double) DatePrototype.SecFromTime(time) : Convert.ToNumber(sec);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime(dhour, min1, sec1, ms)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the minutes value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified minutes value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmin">The minutes value to set for <paramref name="thisob" />.</param>
  /// <param name="sec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMinutes)]
  public static double setUTCMinutes(object thisob, double dmin, object sec, object msec)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double sec1 = DatePrototype.NotSpecified(sec) ? (double) DatePrototype.SecFromTime(time) : Convert.ToNumber(sec);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), dmin, sec1, ms)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the millisecond value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified millisecond value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmsec">The millisecond value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMilliseconds)]
  public static double setUTCMilliseconds(object thisob, double dmsec)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), (double) DatePrototype.MinFromTime(time), (double) DatePrototype.SecFromTime(time), dmsec)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the month value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified month value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dmonth">The month value to set for <paramref name="thisob" />.</param>
  /// <param name="date">The date value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMonth)]
  public static double setUTCMonth(object thisob, double dmonth, object date)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double date1 = DatePrototype.NotSpecified(date) ? (double) DatePrototype.DateFromTime(time) : Convert.ToNumber(date);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.MakeDay(DatePrototype.YearFromTime(time), dmonth, date1), DatePrototype.TimeWithinDay(time)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the seconds value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified seconds value, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set, expressed in UTC.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dsec">The seconds value to set for <paramref name="thisob" />.</param>
  /// <param name="msec">The milliseconds value to set for <paramref name="thisob" />.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCSeconds)]
  public static double setUTCSeconds(object thisob, double dsec, object msec)
  {
    double time = thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
    double ms = DatePrototype.NotSpecified(msec) ? (double) DatePrototype.msFromTime(time) : Convert.ToNumber(msec);
    double num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(time), DatePrototype.MakeTime((double) DatePrototype.HourFromTime(time), (double) DatePrototype.MinFromTime(time), dsec, ms)));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Sets the year value of a <see cref="T:Microsoft.JScript.DateObject" /> to the specified year value.</summary>
  /// <returns>The new value of <paramref name="thisob" /> after the value is set.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  /// <param name="dyear">The year value to set for <paramref name="thisob" />.</param>
  [NotRecommended("setYear")]
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setYear)]
  public static double setYear(object thisob, double dyear)
  {
    double time = thisob is DateObject ? DatePrototype.LocalTime(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
    if (double.IsNaN(dyear))
    {
      ((DateObject) thisob).value = dyear;
      return dyear;
    }
    dyear = Convert.ToInteger(dyear);
    if (0.0 <= dyear && dyear <= 99.0)
      dyear += 1900.0;
    double num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.MakeDay(dyear, (double) DatePrototype.MonthFromTime(time), (double) DatePrototype.DateFromTime(time)), DatePrototype.TimeWithinDay(time))));
    ((DateObject) thisob).value = num;
    return num;
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the day of the week, the month, the date, and the year.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toDateString)]
  public static string toDateString(object thisob)
  {
    return thisob is DateObject ? DatePrototype.DateToDateString(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the day of the week, the month, the date, the year, and the time, expressed in Coordinated Universal Time (UTC). Use <see cref="M:Microsoft.JScript.DatePrototype.toUTCString(System.Object)" /> instead, when you can.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [NotRecommended("toGMTString")]
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toGMTString)]
  public static string toGMTString(object thisob)
  {
    return thisob is DateObject ? DatePrototype.UTCDateToString(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the date but not the time.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleDateString)]
  public static string toLocaleDateString(object thisob)
  {
    object varDate = DatePrototype.getVarDate(thisob);
    return varDate != null ? ((DateTime) varDate).ToLongDateString() : DatePrototype.DateToDateString(((DateObject) thisob).value);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the date and time.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleString)]
  public static string toLocaleString(object thisob)
  {
    object varDate = DatePrototype.getVarDate(thisob);
    return varDate != null ? $"{((DateTime) varDate).ToLongDateString()} {((DateTime) varDate).ToLongTimeString()}" : DatePrototype.DateToString(((DateObject) thisob).value);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the time but not the date.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleTimeString)]
  public static string toLocaleTimeString(object thisob)
  {
    object varDate = DatePrototype.getVarDate(thisob);
    return varDate != null ? ((DateTime) varDate).ToLongTimeString() : DatePrototype.DateToTimeString(((DateObject) thisob).value);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the day of the week, the month, the date, the time, the time zone, and the year.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toString)]
  public static string toString(object thisob)
  {
    return thisob is DateObject ? DatePrototype.DateToString(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the time and the time zone.</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toTimeString)]
  public static string toTimeString(object thisob)
  {
    return thisob is DateObject ? DatePrototype.DateToTimeString(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Creates a string representation of the specified <see cref="T:Microsoft.JScript.DateObject" /> that contains the day of the week, the month, the date, the year, and the time, expressed in Coordinated Universal Time (UTC).</summary>
  /// <returns>A string representation of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toUTCString)]
  public static string toUTCString(object thisob)
  {
    return thisob is DateObject ? DatePrototype.UTCDateToString(((DateObject) thisob).value) : throw new JScriptException(JSError.DateExpected);
  }

  /// <summary>Returns the value of the specified <see cref="T:Microsoft.JScript.DateObject" />.</summary>
  /// <returns>The value of <paramref name="thisob" />.</returns>
  /// <param name="thisob">The object that this method is acting upon.</param>
  [JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_valueOf)]
  public static double valueOf(object thisob)
  {
    return thisob is DateObject ? ((DateObject) thisob).value : throw new JScriptException(JSError.DateExpected);
  }
}
