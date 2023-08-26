using Godot;
using LanguageExt;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

public static class Utils
{
  public static T GetFirstChildOfType<T>(Node node) =>
  node is T matchingNode
      ? matchingNode
      : node.GetChildren()
            .Select(childNode => GetFirstChildOfType<T>(childNode))
            .FirstOrDefault(result => result != null);

  public static Seq<T> GetAllChildrenOfType<T>(Node node) where T : Node =>
      GetAllChildrenOfType(node, Seq<T>.Empty);

  private static Seq<T> GetAllChildrenOfType<T>(Node node, Seq<T> soFar) where T : Node =>
      node switch
      {
        T matchingNode when !soFar.Contains(matchingNode) => soFar.Append(matchingNode).ToSeq(),
        T _ => soFar,
        _ => node.GetChildren().Bind(childNode => GetAllChildrenOfType(childNode, soFar)).ToSeq()
      };

  public static string FormatPercentage(this float value) =>
      string.Format("{0:P0}", value);

  public static T Log<T>(this T obj, string message = "") 
  {
    GD.Print(message, obj);
    return obj;
  }

  public static T Log<T>(this T obj, Func<T, string> f)
  {
    GD.Print(f(obj));
    return obj;
  }

  public static T LogAsJson<T>(this T obj)
  {
    GD.Print(JsonConvert.SerializeObject(obj));
    return obj;
  }
}