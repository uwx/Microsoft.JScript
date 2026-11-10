// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DateConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public class DateConstructor : ScriptFunction
{
    internal static readonly DateConstructor ob = new DateConstructor();
    private DatePrototype originalPrototype;

    internal DateConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "Date", 7)
    {
        this.originalPrototype = DatePrototype.ob;
        DatePrototype._constructor = this;
        this.proto = (object)DatePrototype.ob;
    }

    internal DateConstructor(LenientFunctionPrototype parent, LenientDatePrototype prototypeProp)
        : base((ScriptObject)parent, "Date", 7)
    {
        this.originalPrototype = (DatePrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob) => (object)this.Invoke();

    internal DateObject Construct(DateTime dt)
    {
        return new DateObject((ScriptObject)this.originalPrototype,
            (double)dt.ToUniversalTime().Ticks / 10000.0 - 62135596800000.0);
    }

    internal override object Construct(object[] args) => (object)this.CreateInstance(args);

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>An instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public DateObject CreateInstance(params object[] args)
    {
        if (args.Length == 0)
        {
            DatePrototype originalPrototype = this.originalPrototype;
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.ToUniversalTime();
            double num = (double)dateTime.Ticks / 10000.0 - 62135596800000.0;
            return new DateObject((ScriptObject)originalPrototype, num);
        }

        if (args.Length == 1)
        {
            object ob = args[0];
            IConvertible iconvertible = Convert.GetIConvertible(ob);
            if (Convert.GetTypeCode(ob, iconvertible) == TypeCode.DateTime)
            {
                DatePrototype originalPrototype = this.originalPrototype;
                DateTime dateTime = iconvertible.ToDateTime((IFormatProvider)null);
                dateTime = dateTime.ToUniversalTime();
                double num = (double)dateTime.Ticks / 10000.0 - 62135596800000.0;
                return new DateObject((ScriptObject)originalPrototype, num);
            }

            object primitive = Convert.ToPrimitive(ob, PreferredType.Either, ref iconvertible);
            if (Convert.GetTypeCode(primitive, iconvertible) == TypeCode.String)
                return new DateObject((ScriptObject)this.originalPrototype,
                    DateConstructor.parse(iconvertible.ToString((IFormatProvider)null)));
            double number = Convert.ToNumber(primitive, iconvertible);
            return -8.64E+15 <= number && number <= 8.64E+15
                ? new DateObject((ScriptObject)this.originalPrototype, number)
                : new DateObject((ScriptObject)this.originalPrototype, double.NaN);
        }

        double num1 = Convert.ToNumber(args[0]);
        double number1 = Convert.ToNumber(args[1]);
        double date = args.Length > 2 ? Convert.ToNumber(args[2]) : 1.0;
        double hour = args.Length > 3 ? Convert.ToNumber(args[3]) : 0.0;
        double min = args.Length > 4 ? Convert.ToNumber(args[4]) : 0.0;
        double sec = args.Length > 5 ? Convert.ToNumber(args[5]) : 0.0;
        double ms = args.Length > 6 ? Convert.ToNumber(args[6]) : 0.0;
        int int64 = (int)Runtime.DoubleToInt64(num1);
        if (!double.IsNaN(num1) && 0 <= int64 && int64 <= 99)
            num1 = (double)(int64 + 1900);
        return new DateObject((ScriptObject)this.originalPrototype,
            DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.MakeDay(num1, number1, date),
                DatePrototype.MakeTime(hour, min, sec, ms)))));
    }

    /// <summary>Returns the number of milliseconds that have elapsed since January 1, 1970.</summary>
    /// <returns>The number of milliseconds that have elapsed since January 1, 1970.</returns>
    public string Invoke()
    {
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToUniversalTime();
        return DatePrototype.DateToString((double)dateTime.Ticks / 10000.0 - 62135596800000.0);
    }

    /// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.Double" /> equivalent.</summary>
    /// <returns>A <see cref="T:System.Double" /> equivalent of the date and time that is contained in <paramref name="str" />.</returns>
    /// <param name="str">A string that contains a date and time to convert.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Date_parse)]
    public static double parse(string str) => DatePrototype.ParseDate(str);

    /// <summary>Converts the specified date and time to its <see cref="T:System.Double" /> equivalent.</summary>
    /// <returns>A <see cref="T:System.Double" /> equivalent of the specified date and time.</returns>
    /// <param name="year">The year of the date and time to convert.</param>
    /// <param name="month">The month of the date and time to convert.</param>
    /// <param name="date">The date of the date and time to convert.</param>
    /// <param name="hours">The hours of the date and time to convert.</param>
    /// <param name="minutes">The minutes of the date and time to convert.</param>
    /// <param name="seconds">The seconds of the date and time to convert.</param>
    /// <param name="ms">The milliseconds of the date and time to convert.</param>
    [JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Date_UTC)]
    public static double UTC(
        object year,
        object month,
        object date,
        object hours,
        object minutes,
        object seconds,
        object ms)
    {
        if (year is Missing)
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.ToUniversalTime();
            return (double)dateTime.Ticks / 10000.0 - 62135596800000.0;
        }

        double num = Convert.ToNumber(year);
        double month1 = month is Missing ? 0.0 : Convert.ToNumber(month);
        double date1 = date is Missing ? 1.0 : Convert.ToNumber(date);
        double hour = hours is Missing ? 0.0 : Convert.ToNumber(hours);
        double min = minutes is Missing ? 0.0 : Convert.ToNumber(minutes);
        double sec = seconds is Missing ? 0.0 : Convert.ToNumber(seconds);
        double ms1 = ms is Missing ? 0.0 : Convert.ToNumber(ms);
        int int64 = (int)Runtime.DoubleToInt64(num);
        if (!double.IsNaN(num) && 0 <= int64 && int64 <= 99)
            num = (double)(int64 + 1900);
        return DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.MakeDay(num, month1, date1),
            DatePrototype.MakeTime(hour, min, sec, ms1)));
    }
}