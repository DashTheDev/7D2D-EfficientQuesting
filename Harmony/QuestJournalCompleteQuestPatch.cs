using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(QuestJournal), nameof(QuestJournal.CompleteQuest))]
public class QuestJournalCompleteQuestPatch
{
    static void Postfix(QuestJournal __instance)
    {
        GeneralUtility.DebugLog("QuestJournal complete quest!");
        QuestUtility.FindAndTrackNextQuest(__instance, __instance?.OwnerPlayer);
    }
}