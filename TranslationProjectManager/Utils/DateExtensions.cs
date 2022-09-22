namespace TranslationProjectManager.Utils
{
    public static class DateExtensions
    {
        public static string ToRelativeDateTimeFromNow(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return "just now";
            }

            if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return timeSpan.Minutes + " minutes ago";
            }

            if (timeSpan <= TimeSpan.FromHours(24))
            {
                return timeSpan.Hours + " hours ago";
            }

            if (timeSpan <= TimeSpan.FromDays(7))
            {
                return timeSpan.Days + " days ago";
            }

            return timeSpan.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
