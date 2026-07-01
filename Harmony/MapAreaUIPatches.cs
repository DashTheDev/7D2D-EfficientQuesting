using System.Collections.Generic;
using HarmonyLib;

namespace EfficientQuesting;

[HarmonyPatch(typeof(XUiC_MapArea))]
public class MapAreaUIPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(XUiC_MapArea.onMapPressedLeft))]
    static bool LeftClickPrefix(XUiC_MapArea __instance, XUiController _sender, int _mouseButton)
    {
        EfficientQuestingMod.Instance.Logger.LogLine($"Map left clicked!");

        Quest? clickedQuest = GetQuestFromClickedNavObject(__instance);

        if (clickedQuest != null)
        {
            EntityPlayerLocal? player = __instance.xui?.playerUI?.entityPlayer;
            QuestUtility.ToggleActiveQuest(player?.QuestJournal, clickedQuest, player);
            __instance.closeAllPopups();
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(XUiC_MapArea.onMapPressed))]
    static bool RightClickPrefix(XUiC_MapArea __instance, XUiController _sender, int _mouseButton)
    {
        EfficientQuestingMod.Instance.Logger.LogLine($"Map right clicked!");

        Quest? clickedQuest = GetQuestFromClickedNavObject(__instance);

        if (clickedQuest != null)
        {
            __instance.closeAllPopups();
            SetupAndShowQuestPopup(__instance, clickedQuest);
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(XUiC_MapArea.closeAllPopups))]
    static void CloseAllPopupsPrefix(XUiC_MapArea __instance)
    {
        __instance.xui.HideMapAreaQuestContextWindow();
    }

    private static Quest? GetQuestFromClickedNavObject(XUiC_MapArea mapArea)
    {
        EntityPlayerLocal? player = mapArea.xui?.playerUI?.entityPlayer;
        QuestJournal? playerQuestJournal = player?.QuestJournal;
        NavObject? clickedNavObject = mapArea.closestMouseOverNavObject;

        if (player is null || playerQuestJournal is null || clickedNavObject is null)
        {
            return null;
        }

        Dictionary<int, Quest> activeQuestNavObjectKeyDict = playerQuestJournal.ToActiveQuestNavObjectKeyDict();
        return activeQuestNavObjectKeyDict.GetValueOrDefault(clickedNavObject.Key);
    }

    private static void SetupAndShowQuestPopup(XUiC_MapArea mapArea, Quest quest)
    {
        XUiV_Window window = mapArea.xui.GetMapAreaQuestContextWindow();
        window.Controller.TryGetChildController("", out XUiC_MapQuestPopupList questPopupList);
        questPopupList?.Quest = quest;
        window.Position = mapArea.xui.GetMouseXUiPosition();
        window.IsVisible = true;
    }
}