using Godot;
using LanguageExt;
using System.Linq;

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
}
