using System.Reflection;
using System.Runtime.Serialization;

public static class EnumExtensions
{
    public static string GetEnumMemberValue<TEnum>(this TEnum value) where TEnum : Enum
    {
        var member = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
        var attr = member?.GetCustomAttribute<EnumMemberAttribute>();
        return attr?.Value ?? value.ToString();
    }
}