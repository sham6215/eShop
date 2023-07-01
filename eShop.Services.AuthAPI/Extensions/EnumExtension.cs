namespace eShop.Services.AuthAPI.Extensions
{
    public class StringValueAttribute : Attribute
    {
        public string Value { get; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }

    public static class EnumExtension
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var field = type.GetField(name ?? string.Empty);
            var attribute = field?.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attribute?.Length > 0 ? attribute[0].Value : string.Empty;
        }
    }
}
