using System;
using System.Diagnostics;
using Godot;
using Newtonsoft.Json;

namespace Ace.Util;

public static class Logging
{
  public static T Log<T>(this T obj)
  {
    GD.Print($"{CallingMethodName}\r\n", obj);
    return obj;
  }
    
  public static T Log<T>(this T obj, string message) 
  {
    GD.Print($"{CallingMethodName}\r\n{message}", obj);
    return obj;
  }

  public static T Log<T>(this T obj, Func<T, string> f)
  {
    GD.Print($"{CallingMethodName}\r\n{f(obj)}");
    return obj;
  }

  public static T LogAsJson<T>(this T obj)
  {
    GD.Print($"{CallingMethodName}\r\n", JsonConvert.SerializeObject(obj));
    return obj;
  }
   private static string CallingMethodName => 
     new StackTrace().GetFrame(2)?.GetMethod()?.Name ?? "Unknown Method";
}