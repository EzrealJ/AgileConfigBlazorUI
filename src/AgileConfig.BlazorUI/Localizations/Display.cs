namespace AgileConfig.BlazorUI.Localizations
{
    public static class Display
    {
        public static string DateTimeAsString(System.DateTime? dateTime)
        {
            return dateTime?.ToString(Consts.Format.DATE_TIME_YYYY_MM_DD_HH_MM_SS) ?? string.Empty;
        }
    }
}
