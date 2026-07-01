using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(QuestJournal), nameof(QuestJournal.CompleteQuest))]
public class QuestJournalCompleteQuestPatch
{
    static void Postfix(QuestJournal __instance)
    {
        EfficientQuestingMod.Instance.Logger.LogLine("QuestJournal complete quest!");
        QuestUtility.FindAndTrackNextQuest(__instance, __instance?.OwnerPlayer);
    }
}