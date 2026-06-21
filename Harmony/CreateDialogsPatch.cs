using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(DialogFromXml), nameof(DialogFromXml.CreateDialogs))]
public class CreateDialogsPatch
{
    private static void Postfix(XmlFile xmlFile)
    {
        QuestUtility.AllowUnlimitedQuests();
    }
}