using System.Collections.Generic;

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

    public static bool IsAlreadyTrackingQuest(this QuestJournal questJournal, Quest quest)
    {
        if (questJournal?.TrackedQuest == null || quest == null)
        {
            return false;
        }

        return questJournal.TrackedQuest.QuestCode == quest.QuestCode;
    }
}