namespace BaseAuth.Extension;

[AttributeUsage(AttributeTargets.Method)]
public class RequestBodyAttribute(Type type) : Attribute
{
    public readonly Type Type = type;
}