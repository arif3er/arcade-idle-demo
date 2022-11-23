using System.Collections.Generic;
using System.Globalization;

public static class Utility
{
    public static string ToTitleCase(string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }

    public static void ListPop<T>(List<T> list)
    {
        list.RemoveAt(list.Count - 1);
    }
}
