using System.Linq;
using Godot;

namespace Ace.Util;

public static class NodeUtils
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
}