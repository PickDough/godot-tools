namespace Godough.Addons.DI;

[AttributeUsage(AttributeTargets.Property)]
public class FromOwner : Attribute
{
    public string? Name { get; set; }
}
