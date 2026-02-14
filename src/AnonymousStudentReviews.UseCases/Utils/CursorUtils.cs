using System.Text;
using System.Text.Json;

namespace AnonymousStudentReviews.UseCases.Utils;

public static class CursorUtils
{
    public static string ToCursor<T>(T cursorData)
    {
        if (cursorData == null)
        {
            return string.Empty;
        }

        var json = JsonSerializer.Serialize(cursorData);
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static T? FromCursor<T>(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return default;
        }

        try
        {
            var bytes = Convert.FromBase64String(cursor);
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }
}
