using Godot;

namespace Godough.Addons.NodeSelection;

public static class NodeSelectorExtension
{
    public static T? FindUnderRoot<T>(this Node node)
        where T : Node
    {
        return node.GetTree().Root.FindUnder<T>();
    }

    public static T? FindUnderRoot<T>(this Node node, string? name)
        where T : Node
    {
        return node.GetTree().Root.FindUnder<T>(name);
    }

    public static IEnumerable<T> FindUnderMany<T>(this Node node, bool recursive = false)
        where T : Node
    {
        foreach (var c in node.GetChildren())
        {
            if (recursive)
                foreach (var sub in c.FindUnderMany<T>(recursive))
                    yield return sub;

            if (c is T ct)
                yield return ct;
        }
    }

    public static T? FindUnder<T>(this Node? node)
        where T : Node
    {
        return node switch
        {
            null => null,
            T currentNode => currentNode,
            _ => node.GetChildren().Select(c => c.FindUnder<T>()).OfType<T>().FirstOrDefault(),
        };
    }

    public static T? FindUnder<T>(this Node? node, string? name)
        where T : Node
    {
        if (name is null)
            return node.FindUnder<T>();

        return node.FindUnderWithName(typeof(T), name) as T;
    }

    public static T? FindAutoload<T>(this Node node, string name) where T : Node, new()
    {
        var root = node.GetTree().Root;
        var autoload = root.GetNode<T>(name);
        if (autoload != null)
            return autoload;

        autoload = new T();
        autoload.Name = name;
        root.AddChild(autoload);

        return autoload;
    }

    private static Node? FindUnderWithName(this Node? node, Type type, string name)
    {
        if (node == null)
            return null;

        if (node.GetType().IsAssignableTo(type) && node.Name == name)
            return node;

        return node.GetChildren()
            .Select(c => c.FindUnderWithName(type, name))
            .OfType<Node>()
            .FirstOrDefault();
    }
}
