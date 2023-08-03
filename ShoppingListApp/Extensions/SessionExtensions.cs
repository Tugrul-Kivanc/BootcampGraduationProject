using System.Text.Json;

namespace ShoppingListApp.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T instance)
        {
            string data = JsonSerializer.Serialize<T>(instance);
            session.SetString(key, data);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            string? data = session.GetString(key);
            return data == null ? default(T) : JsonSerializer.Deserialize<T>(data);
        }
    }
}
