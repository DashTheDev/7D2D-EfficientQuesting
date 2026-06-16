namespace EfficientQuesting;

public static class LocalisationUtility
{
    private const string LanguageName = "English";

    public static string GetNoLongerTrackingMessage(string questName)
    {
       return Localization.Get("noLongerTrackingMsg", LanguageName).Format(questName);
    }

    public static string GetNowTrackingMessage(string questName)
    {
        return Localization.Get("nowTrackingMsg", LanguageName).Format(questName);
    }
}