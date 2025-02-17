using System.Collections;
using System.Reflection;
using System.Text;

namespace BaseAuth.Util;

public class TypeMigrator
{
    private HashSet<Type> processedTypes = []; // Tránh lặp vô hạn

    public string ConvertClassToTypeScript(Type type)
    {
        processedTypes.Clear();

        if (IsCollection(type))
        {
            type = type.GetGenericArguments()[0];
        }
        
        return ConvertClassToTypeScriptRecursive(type);
    }
    
    public string ConvertClassToTypeScriptRecursive(Type type)
    {
        if (processedTypes.Contains(type)) return ""; // Tránh lặp vô hạn
        processedTypes.Add(type);

        StringBuilder tsCode = new StringBuilder();

        if (type.IsEnum)
        {
            tsCode.AppendLine($"export enum {type.Name} {{");
            foreach (var name in Enum.GetNames(type))
            {
                int value = (int)Enum.Parse(type, name);
                tsCode.AppendLine($"    {name} = {value},");
            }
            tsCode.AppendLine("}");
            return tsCode.ToString();
        }

        Type baseType = type.BaseType;
        string inheritance = (baseType != null && baseType != typeof(object) && baseType != typeof(ValueType))
            ? $" extends {baseType.Name}" : "";

        tsCode.AppendLine($"export interface {type.Name}{inheritance} {{");

        foreach (MemberInfo member in GetAllMembers(type))
        {
            string memberName = ToCamelCase(member.Name);
            Type memberType = (member is PropertyInfo prop) ? prop.PropertyType : ((FieldInfo)member).FieldType;
            string tsType = ConvertCSharpTypeToTypeScript(memberType);

            tsCode.AppendLine($"    {memberName}: {tsType};");

            if (IsCustomClass(memberType) || memberType.IsEnum)
            {
                tsCode.AppendLine(ConvertClassToTypeScriptRecursive(memberType));
            }
        }

        tsCode.AppendLine("}");
        return tsCode.ToString();
    }

    public static IEnumerable<MemberInfo> GetAllMembers(Type type)
    {
        List<MemberInfo> members = new List<MemberInfo>();

        while (type != null && type != typeof(object) && type != typeof(ValueType))
        {
            members.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            members.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            type = type.BaseType;
        }

        return members;
    }

    public static string ConvertCSharpTypeToTypeScript(Type type)
    {
        bool isNullable = false;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type);
            isNullable = true;
        }

        string tsType = type switch
        {
            _ when type == typeof(int) || type == typeof(long) || type == typeof(short) ||
                   type == typeof(byte) || type == typeof(float) || type == typeof(double) || type == typeof(decimal) => "number",
            _ when type == typeof(string) || type == typeof(char) => "string",
            _ when type == typeof(bool) => "boolean",
            _ when type == typeof(DateTime) => "Date",
            _ when type.IsEnum => type.Name,
            _ when typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType => ConvertCollectionType(type),
            _ when IsCustomClass(type) => type.Name,
            _ => "any"
        };

        return isNullable ? $"{tsType} | null" : tsType;
    }

    public static string ConvertCollectionType(Type type)
    {
        Type[] genericArgs = type.GetGenericArguments();
        if (genericArgs.Length == 1)
        {
            return $"{ConvertCSharpTypeToTypeScript(genericArgs[0])}[]"; // List<T>, IEnumerable<T>, HashSet<T>, ...
        }
        if (genericArgs.Length == 2)
        {
            return "{ [key: string]: " + ConvertCSharpTypeToTypeScript(genericArgs[1]) + " }"; // Dictionary<K, V>
        }
        return "any";
    }

    private static bool IsCustomClass(Type type)
    {
        return type.IsClass && type != typeof(string) && type != typeof(object);
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
    
    private static bool IsCollection(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
    }
}