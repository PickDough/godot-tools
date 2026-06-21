using Godot;

namespace Godough.Addons._3D;

public static class NodeAabb
{
    public static Aabb GetAabb(this Node3D node)
    {
        return GetAabb(node, node.GlobalTransform);
    }

    private static Aabb GetAabb(Node3D? node, Transform3D boundsTransform)
    {
        if (node == null)
            return new Aabb(new Vector3(-0.2f, -0.2f, -0.2f), new Vector3(0.4f, 0.4f, 0.4f));

        var topXform = boundsTransform.AffineInverse() * node.GlobalTransform;
        var box = node is VisualInstance3D visual ? visual.GetAabb() : new Aabb();

        box = topXform * box;
        foreach (var child in node.GetChildren())
        {
            if (child is not Node3D childNode) continue;
            var childBox = GetAabb(childNode, boundsTransform);
            box = box.Merge(childBox);
        }

        return box;
    }
}
