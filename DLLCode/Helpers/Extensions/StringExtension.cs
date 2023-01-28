namespace ACPROpenAI.Helpers.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Converts an HTML string to plain text
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPlainText(this string value)
        {
            return PX.Data.Search.SearchService.Html2PlainText(value);
        }
    }
}
