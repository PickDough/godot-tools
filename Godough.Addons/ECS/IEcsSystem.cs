using Godot;

namespace Godough.Addons.ECS;

public interface IEcsSystem
{
    public Func<Node, bool> Query { get; }

    public void AddEntities(params Node[] entities)
    {
    }

    public void RemoveEntity(Node entity)
    {
    }
}

public interface IEcsSystem<T> : IEcsSystem where T : class
{

    public List<T> Entities { get; }

    void IEcsSystem.AddEntities(params Node[] entities)
    {
        foreach (var e in entities.OfType<T>())
        {
            Entities.Add(e);
            OnEntityAdded(e);
        }
    }

    void IEcsSystem.RemoveEntity(Node entity)
    {
        Entities.Remove(entity as T ?? throw new InvalidOperationException());
    }

    public void OnEntityAdded(T entity);
}
