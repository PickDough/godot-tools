using Godot;

namespace Godough.Addons._3D;

public record RaycastResult(CollisionObject3D Collider, Vector3 Normal, Vector3 Point);

public static class Raycast3DExtension
{
    public static RaycastResult? RaycastFromScreen(this Node3D node, uint collisionMask = 4294967295)
    {
        var camera = node.GetViewport().GetCamera3D();
        var from = camera.ProjectRayOrigin(node.GetViewport().GetMousePosition());
        var to = camera.GlobalPosition + camera.ProjectRayNormal(node.GetViewport().GetMousePosition()) * 1000;

        var spaceState = node.GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(from, to, collisionMask);
        var result = spaceState.IntersectRay(query);
        if (result.Count == 0)
            return null;

        return new RaycastResult(
            (CollisionObject3D)result["collider"],
            (Vector3)result["normal"],
            (Vector3)result["position"]);
    }
}
