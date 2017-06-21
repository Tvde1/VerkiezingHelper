using System.Linq;

namespace VerkiezingHelper.Helpers
{
    public static class Extentions
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}