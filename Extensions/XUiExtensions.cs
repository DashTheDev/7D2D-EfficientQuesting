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

#if V2_6_0
    public static bool TryGetChildController<TController>(this XUiController controller, string id, out TController foundController) where TController : XUiController
    {
        foreach (TController childController in controller.GetChildrenByType<TController>())
        {
            foundController = childController;
            return true;
        }

        foundController = null;
        return false;
    }

    public static bool TryGetChildView<TView>(this XUiController controller, string id, out TView view) where TView : XUiView
    {
        foreach (TView childView in controller.GetChildrenByViewType<TView>())
        {
            if (childView.ID != id)
            {
                continue;
            }

            view = childView;
            return true;
        }

        view = null;
        return false;
    }
#endif
}