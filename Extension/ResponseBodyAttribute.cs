namespace BaseAuth.Extension;

[AttributeUsage(AttributeTargets.Method)]
public class ResponseBodyAttribute(Type type) : Attribute
{
    public readonly Type Type = type;
}