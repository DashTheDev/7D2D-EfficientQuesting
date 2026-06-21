using HarmonyLib;
using UnityEngine;

namespace EfficientQuesting;

[HarmonyPatch(typeof(Quest), $"set_{nameof(Quest.Position)}")]
public class QuestSetPositionPatch
{
    static void Postfix(Quest __instance, Vector3 value)
    {
        GeneralUtility.LogLine("Quest set position!");
        QuestUtility.FindAndTrackNextQuest(__instance.OwnerJournal, __instance.OwnerJournal?.OwnerPlayer);
    }
}