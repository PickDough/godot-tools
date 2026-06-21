namespace Godough.Addons.DI;

[AttributeUsage(AttributeTargets.Property)]
public class FromRoot : Attribute
{
    public string? Name { get; set; }
}
