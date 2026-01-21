namespace OnboardingSIGDB1.Domain.Utils
{
    public static class StringUtils
    {
        public static string RemoveMask(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            
            return new string(value.Where(char.IsDigit).ToArray());
        }
    }
}