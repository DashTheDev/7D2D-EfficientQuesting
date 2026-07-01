namespace EfficientQuesting;

public static class XUiExtensions
{
    public static XUiV_Window? GetMapAreaQuestContextWindow(this XUi xui)
    {
        return xui.GetWindow("mapAreaQuestContext");
    }

    public static void HideMapAreaQuestContextWindow(this XUi xui)
    {
        xui.GetMapAreaQuestContextWindow()?.IsVisible = false;
    }
}