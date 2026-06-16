using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(QuestJournal), nameof(QuestJournal.FailedQuest))]
public class QuestJournalFailedQuestPatch
{
    static void Postfix(QuestJournal __instance)
    {
        GeneralUtility.DebugLog("QuestJournal failed quest!");
        QuestUtility.FindAndTrackNextQuest(__instance, __instance?.OwnerPlayer);
    }
}