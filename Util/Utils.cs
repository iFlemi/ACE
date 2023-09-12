using System;
using System.Linq;
using Godot;
using Newtonsoft.Json;

namespace Ace.Util;

public static class Utils
{
    public static T GetFirstChildOfType<T>(this Node node) =>
        node is T matchingNode
            ? matchingNode
            : node.GetChildren()
                .Select(GetFirstChildOfType<T>)
                .FirstOrDefault(result => result != null);

    public static Seq<T> GetAllChildrenOfType<T>(this Node node) where T : Node =>
        GetAllChildrenOfType(node, Seq<T>());

    private static Seq<T> GetAllChildrenOfType<T>(Node node, Seq<T> soFar) where T : Node =>
        node switch
        {
            T matchingNode when !soFar.Contains(matchingNode) => soFar.Append(matchingNode).ToSeq(),
            T => soFar,
            _ => node.GetChildren().Bind(childNode => GetAllChildrenOfType(childNode, soFar)).ToSeq()
        };

    public static string FormatPercentage(this float value) =>
        $"{value:P0}";

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