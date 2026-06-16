using UnityEngine;

namespace EfficientQuesting;

public static class QuestUtility
{
    public static Quest? FindNextQuestToTrack(QuestJournal journal, EntityPlayerLocal player)
    {
        Quest? closestInProgressQuest = null;
        float closestObjectiveDistance = float.MaxValue;

        void FindAndUpdateClosestQuest(Quest quest)
        {
            if (quest.Position == Vector3.zero)
            {
                return;
            }

            float distance = Vector3.SqrMagnitude(player.position - quest.Position);

            if (distance < closestObjectiveDistance)
            {
                closestInProgressQuest = quest;
                closestObjectiveDistance = distance;
            }
        }

        for (int i = 0; i < journal.quests.Count; i++)
        {
            Quest quest = journal.quests[i];

            if (quest.IsCompletedOrFailed())
            {
                continue;
            }

            FindAndUpdateClosestQuest(quest);
        }

        return closestInProgressQuest;
    }

    public static void FindAndTrackNextQuest(QuestJournal journal, EntityPlayerLocal player)
    {
        if (journal == null || player == null)
        {
            return;
        }

        Quest? nextActiveQuest = FindNextQuestToTrack(journal, player);

        if (journal.IsAlreadyTrackingQuest(nextActiveQuest))
        {
            GeneralUtility.DebugLog("Already tracking quest!");
            return;
        }
        else if (nextActiveQuest is not null)
        {
            GeneralUtility.DebugLog("Found another quest to track!");
            ToggleActiveQuest(journal, nextActiveQuest, player);
            return;
        }

        GeneralUtility.DebugLog("Didn't find another quest to track!");
    }

    public static void ToggleActiveQuest(QuestJournal journal, Quest quest, EntityPlayerLocal? player = null)
    {
        if (journal.TrackedQuest == quest)
        {
            quest.Tracked = false;
            journal.TrackedQuest = null;
            player.ShowTooltip(LocalisationUtility.GetNoLongerTrackingMessage(quest.QuestClass.Name));
        }
        else
        {
            journal.TrackedQuest?.Tracked = false;
            journal.TrackedQuest = quest;
            player.ShowTooltip(LocalisationUtility.GetNowTrackingMessage(quest.QuestClass.Name));
        }
    }
}