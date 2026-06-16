namespace EfficientQuesting;

public static class GeneralUtility
{
    public static void DebugLog(object str)
    {
        if (!EfficientQuestingMod.Config.IsDebug)
        {
            return;
        }

        Log.Out($"[{EfficientQuestingMod.ModInstance.Name}](v{EfficientQuestingMod.ModInstance.VersionString}) {str}");
    }
}