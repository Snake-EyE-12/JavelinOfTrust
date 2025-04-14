public static class Utils
{
    public static int Sign(float value) => value > 0 ? 1 : value < 0 ? -1 : 0;
    
    public static string RemoveSuffix(this string input, string suffix)
    {
        if (input != null && input.EndsWith(suffix))
            return input.RemoveLast(suffix.Length);
        return input;
    }
    public static string RemoveLast(this string input, int length)
    {
        return input.Substring(0, input.Length - length);
    }
}