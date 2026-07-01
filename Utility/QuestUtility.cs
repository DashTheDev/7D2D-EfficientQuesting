using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            EfficientQuestingMod.Instance.Logger.LogLine("Already tracking quest!");
            return;
        }
        else if (nextActiveQuest is not null)
        {
            EfficientQuestingMod.Instance.Logger.LogLine("Found another quest to track!");
            ToggleActiveQuest(journal, nextActiveQuest, player);
            return;
        }

        EfficientQuestingMod.Instance.Logger.LogLine("Didn't find another quest to track!");
    }

    public static void ToggleActiveQuest(QuestJournal journal, Quest quest, EntityPlayerLocal? player = null)
    {
        if (journal == null || quest == null)
        {
            return;
        }

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

    public static void RemoveQuest(QuestJournal journal, Quest quest)
    {
        if (journal == null || quest == null)
        {
            return;
        }

        journal.RemoveQuest(quest);
    }

    public static void AllowUnlimitedQuests()
    {
        if (!EfficientQuestingMod.Instance.Config.AllowMultipleActiveQuests)
        {
            return;
        }

        bool foundTraderDialog = Dialog.DialogList.TryGetValue("trader", out Dialog traderDialog);

        if (!foundTraderDialog)
        {
            return;
        }

        DialogResponse? jobsNoneDialogResponse = traderDialog.Responses.FirstOrDefault(r => r.ID == "jobsnone");

        if (jobsNoneDialogResponse != null)
        {
            traderDialog.Responses.Remove(jobsNoneDialogResponse);
            EfficientQuestingMod.Instance.Logger.LogLine("Removed jobsnone dialog response!");
        }

        IEnumerable<DialogResponse> jobsHaveDialogResponses = traderDialog.Responses.Where(r => IsJobsHaveID(r.ID));

        foreach (DialogResponse jobsHaveDialogResponse in jobsHaveDialogResponses)
        {
            BaseDialogRequirement? questStateRequirement = jobsHaveDialogResponse.RequirementList.FirstOrDefault(r => r.RequirementType == BaseDialogRequirement.RequirementTypes.QuestStatus);

            if (questStateRequirement == null)
            {
                continue;
            }

            jobsHaveDialogResponse.RequirementList.Remove(questStateRequirement);
            EfficientQuestingMod.Instance.Logger.LogLine("Removed dialog response quest state requirement!");
        }
    }

    private static bool IsJobsHaveID(string id) => Regex.IsMatch(id, @"^jobshave[1-9]$");
}