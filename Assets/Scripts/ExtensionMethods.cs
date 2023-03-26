public static class ExtensionMethods
{
    public static bool IsLetter(this char c) 
    {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    }
}