using Godot;

namespace Godough.Addons.ECS;

public class ECS : Node
{
    public List<IEcsSystem> Systems { get; } = [];

    public override void _EnterTree()
    {
        GetTree().Root.ChildEnteredTree += OnNodeAdded;
    }

    private void OnNodeAdded(Node node)
    {
        if (node.HasMeta("ecs")) return;
        node.ChildEnteredTree += OnNodeAdded;
        node.SetMeta("ecs", true);

        if (node is IEcsSystem system)
        {
            Systems.Add(system);
            system.AddEntities(LookupEntities(system).ToArray());
        }
        else
        {
            foreach (var s in Systems.Where(s => s.Query(node)))
            {
                s.AddEntities(node);
                node.TreeExited += () => OnNodeRemoved(s, node);
            }
        }
    }

    private List<Node> LookupEntities(IEcsSystem system)
    {
        var result = new List<Node>();

        void Search(Node node)
        {
            if (system.Query(node))
            {
                result.Add(node);
                node.TreeExited += () => OnNodeRemoved(system, node);
            }

            foreach (var child in node.GetChildren())
                Search(child);
        }

        Search(GetTree().Root);

        return result;
    }

    private static void OnNodeRemoved(IEcsSystem ecsSystem, Node node)
    {
        ecsSystem.RemoveEntity(node);
    }
}
