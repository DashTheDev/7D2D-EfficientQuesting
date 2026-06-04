using System.Collections.Generic;
using UnityEngine;

namespace EfficientQuesting;

public static class QuestJournalExtensions
{
    public static Dictionary<int, Quest> ToActiveQuestNavObjectKeyDict(this QuestJournal journal)
    {
        Dictionary<int, Quest> dict = [];

        for (int i = 0; i < journal.quests.Count; i++)
        {
            Quest quest = journal.quests[i];

            if (!quest.Active || quest.NavObject is null)
            {
                continue;
            }

            if (dict.ContainsKey(quest.NavObject.Key))
            {
                continue;
            }

            dict.Add(quest.NavObject.Key, quest);
        }

        return dict;
    }

    public static Dictionary<Quest.QuestState, Quest> ToFirstQuestStateOccurenceDict(this QuestJournal journal, Quest? questToExclude = null)
    {
        Dictionary<Quest.QuestState, Quest> dict = [];

        for (int i = 0; i < journal.quests.Count; i++)
        {
            Quest quest = journal.quests[i];

            if (!quest.Active || quest == questToExclude)
            {
                continue;
            }

            if (dict.ContainsKey(quest.CurrentState))
            {
                continue;
            }

            dict.Add(quest.CurrentState, quest);
        }

        return dict;
    }

    public static Quest? FindNextQuestToTrack(this QuestJournal journal, EntityPlayerLocal player)
    {
        Quest? closestInProgressQuest = null;
        float closestObjectiveDistance = float.MaxValue;

        void FindDistanceToNavObjectAndUpdateClosestQuest(Quest quest, NavObject navObject)
        {
            float distance = Vector3.SqrMagnitude(player.position - navObject.GetPosition());

            if (distance < closestObjectiveDistance)
            {
                closestInProgressQuest = quest;
                closestObjectiveDistance = distance;
            }
        }

        for (int i = 0; i < journal.quests.Count; i++)
        {
            Quest quest = journal.quests[i];

            if (!quest.Active || quest.QuestClass is null || quest.CurrentPhase == quest.QuestClass.HighestPhase)
            {
                continue;
            }

            if (quest.GetActiveObjective() is BaseObjective activeObjective && activeObjective.NavObject is not null)
            {
                FindDistanceToNavObjectAndUpdateClosestQuest(quest, activeObjective.NavObject);
            }
            else if (quest.NavObject is not null)
            {
                FindDistanceToNavObjectAndUpdateClosestQuest(quest, quest.NavObject);
            }
        }

        return closestInProgressQuest;
    }

    public static void ToggleActiveQuest(this QuestJournal journal, Quest quest, EntityPlayerLocal? player = null)
    {
        if (journal.TrackedQuest == quest)
        {
            quest.Tracked = false;
            journal.TrackedQuest = null;
            player.ShowTooltip($"No longer tracking: {quest.QuestClass.Name}");
        }
        else
        {
            journal.TrackedQuest?.Tracked = false;
            journal.TrackedQuest = quest;
            player.ShowTooltip($"Now tracking: {quest.QuestClass.Name}");
        }
    }
}