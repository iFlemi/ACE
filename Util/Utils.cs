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

  public static Seq<T> Log<T>(this Seq<T> seq, string message = "")
  {
    GD.Print($"{message}{JsonConvert.SerializeObject(seq)}");
    return seq;
  }
}