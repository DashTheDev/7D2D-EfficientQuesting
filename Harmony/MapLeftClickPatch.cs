using System.Collections.Generic;
using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(XUiC_MapArea), nameof(XUiC_MapArea.onMapPressedLeft))]
public class MapLeftClickPatch
{
    [HarmonyPriority(Priority.Last)]
    static bool Prefix(XUiC_MapArea __instance, XUiController _sender, int _mouseButton)
    {
        GeneralUtility.LogLine($"Map left clicked!");

        EntityPlayerLocal? player = __instance.xui?.playerUI?.entityPlayer;
        QuestJournal? playerQuestJournal = player?.QuestJournal;
        NavObject? clickedNavObject = __instance.closestMouseOverNavObject;

        if (player is null || playerQuestJournal is null || clickedNavObject is null)
        {
            return true;
        }

        Dictionary<int, Quest> activeQuestNavObjectKeyDict = playerQuestJournal.ToActiveQuestNavObjectKeyDict();

        if (activeQuestNavObjectKeyDict.GetValueSafe(clickedNavObject.Key) is Quest foundQuest)
        {
            QuestUtility.ToggleActiveQuest(playerQuestJournal, foundQuest, player);
            __instance.closeAllPopups();
            return false;
        }

        return true;
    }
}