using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(Quest), nameof(Quest.AdvancePhase))]
public class QuestAdvancePhasePatch
{
    static void Postfix(Quest __instance)
    {
        Utility.DebugLog("Quest advance phase!");

        if (__instance.QuestClass is null || __instance.OwnerJournal is null || __instance.OwnerJournal.OwnerPlayer is not EntityPlayerLocal player)
        {
            return;
        }

        if (__instance.CurrentPhase != __instance.QuestClass.HighestPhase)
        {
            return;
        }

        Quest? nextActiveQuest = __instance.OwnerJournal.FindNextQuestToTrack(player);

        if (nextActiveQuest is not null)
        {
            Utility.DebugLog("Found another quest to track!");
            __instance.OwnerJournal.ToggleActiveQuest(nextActiveQuest, player);
            return;
        }

        Utility.DebugLog("Didn't find another quest to track!");
    }
}