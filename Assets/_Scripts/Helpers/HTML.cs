using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class HTML
{
    public const string ALPHA = "<color=#00000000>";
    public const string DIALOGUE_PAUSE_PLACEHOLDER = "pause";

    public static List<string> ALL_UNIQUE_TAGS = new List<string>()
    {
        DIALOGUE_PAUSE_PLACEHOLDER, 
    };

    public static string CreatePauseTag(float pauseDuration)
    {
        return "<" + DIALOGUE_PAUSE_PLACEHOLDER + h.Str(pauseDuration) + ">";
    }

    public static string RemoveAllTags(string input)
    {
        return Regex.Replace(input, @"<[^>]*>", "");
    }
    
    public static string RemoveUniqueTags(string input)
    {
        string result = input;
        foreach (string tag in ALL_UNIQUE_TAGS)
        {
            result = Regex.Replace(result, @"<" + tag + @"[^>]*>", "");
        }
        return result;
    }
}
