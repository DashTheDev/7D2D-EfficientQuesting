namespace EfficientQuesting;

public static class QuestExtensions
{
    public static BaseObjective? GetActiveObjective(this Quest quest)
    {
        if (quest.Objectives is null | quest.Objectives.Count <= 0)
        {
            return null;
        }

        for (int i = 0; i < quest.Objectives.Count; i++)
        {
            BaseObjective objective = quest.Objectives[i];

            if (objective.Phase == quest.CurrentPhase)
            {
                return objective;
            }
        }

        return null;
    }

    public static bool IsAtHighestPhase(this Quest quest)
    {
        if (quest.QuestClass is null)
        {
            return false;
        }

        return quest.CurrentPhase == quest.QuestClass.HighestPhase;
    }

    public static bool IsCompletedOrFailed(this Quest quest)
    {
        return quest.CurrentState == Quest.QuestState.Completed || quest.CurrentState == Quest.QuestState.Failed;
    }
}