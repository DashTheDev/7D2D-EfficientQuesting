using System.Collections.Generic;
using UnityEngine;

namespace EfficientQuesting;

public class XUiC_MapQuestPopupList : XUiController
{
    #region Fields

    private XUiC_MapPopupEntry? trackButton;
    private XUiC_MapPopupEntry? untrackButton;
    private XUiC_MapPopupEntry? removeButton;
    private XUiC_MapPopupEntry? detailsButton;
    private XUiC_MapPopupEntry? shareButton;
    private XUiC_MapPopupEntry? teleportToButton;

    #endregion

    #region Properties

    public Quest? Quest { get; set; }
    public EntityPlayerLocal? Player => xui?.playerUI?.entityPlayer;
    public QuestJournal? QuestJournal => Player?.QuestJournal;
    public bool QuestIsTracked => Quest?.Tracked ?? false;
    public bool PlayerCanTeleport => GameStats.GetBool(EnumGameStats.IsTeleportEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
    public bool PlayerIsInParty => Player?.IsInParty() ?? false;

    #endregion

    #region Life Cycle

    public override void Init()
    {
        base.Init();

        trackButton = GetButton(0);
        untrackButton = GetButton(1);
        removeButton = GetButton(2);
        detailsButton = GetButton(3);
        shareButton = GetButton(4);
        teleportToButton = GetButton(5);

        OnVisiblity += OnVisiblityChanged;
        SubscribeToButtonPresses();
    }

#if V2_6_0
    private void OnVisiblityChanged(XUiController _sender, bool _visible)
#else
    private void OnVisiblityChanged(XUiController _sender, bool _visible, bool _visibleInScene)
#endif
    {
        if (!_visible)
        {
            return;
        }

        trackButton?.ViewComponent?.IsVisible = !QuestIsTracked;
        untrackButton?.ViewComponent?.IsVisible = QuestIsTracked;
        SetButtonEnabled(shareButton, PlayerIsInParty);
        SetButtonEnabled(teleportToButton, PlayerCanTeleport);

        XUiView? navigationViewComponent = (trackButton?.ViewComponent?.IsVisible ?? false) ? trackButton?.ViewComponent : untrackButton?.ViewComponent;

        if (navigationViewComponent != null)
        {
            xui.playerUI.CursorController.SetNavigationTargetLater(navigationViewComponent);
        }
    }

#endregion

    #region Event Handling

    private void OnTrackButtonPress(XUiController _sender, int _mouseButton)
    {
        QuestUtility.ToggleActiveQuest(QuestJournal, Quest, Player);
        ClosePopup();
    }

    private void OnRemoveButtonPress(XUiController _sender, int _mouseButton)
    {
        QuestUtility.RemoveQuest(QuestJournal, Quest);
        ClosePopup();
    }

    private void OnDetailsButtonPress(XUiController _sender, int _mouseButton)
    {
        XUiC_WindowSelector.OpenSelectorAndWindow(xui.playerUI.entityPlayer, "quests");
        XUiC_QuestListWindow? questListWindow = xui.GetWindow("windowQuestList")?.Controller as XUiC_QuestListWindow;
        List<XUiC_QuestEntry>? questEntries = questListWindow?.questList?.entryList;

        if (questEntries != null || Quest == null)
        {
            foreach (XUiC_QuestEntry questEntry in questEntries)
            {
                if (questEntry.Quest == null || questEntry.Quest.QuestCode != Quest.QuestCode)
                {
                    continue;
                }

                questListWindow.questList.SelectedEntry = questEntry;
            }
        }

        ClosePopup();
    }

    private void OnShareButtonPress(XUiController _sender, int _mouseButton)
    {
        if (Quest == null && Player == null || !PlayerIsInParty)
        {
            return;
        }

        PartyQuests.ShareQuestWithParty(Quest, Player, _showTooltips: true);
        ClosePopup();
    }

    private void OnTeleportToButtonPress(XUiController _sender, int _mouseButton)
    {
        if (Quest != null && Player != null)
        {
            Player.Teleport(Quest.Position);
        }

        ClosePopup();
    }

    #endregion

    #region Supporting Methods

    private XUiC_MapPopupEntry? GetButton(int index)
    {
        if (Children.Count <= index)
        {
            return null;
        }

        return Children[index] as XUiC_MapPopupEntry;
    }

    private void SubscribeToButtonPresses()
    {
        trackButton?.OnPress += OnTrackButtonPress;
        untrackButton?.OnPress += OnTrackButtonPress;
        removeButton?.OnPress += OnRemoveButtonPress;
        detailsButton?.OnPress += OnDetailsButtonPress;
        shareButton?.OnPress += OnShareButtonPress;
        teleportToButton?.OnPress += OnTeleportToButtonPress;
    }

    private void ClosePopup()
    {
        xui.HideMapAreaQuestContextWindow();
    }

    private void SetButtonEnabled(XUiC_MapPopupEntry? button, bool isEnabled)
    {
        if (button == null || button.ViewComponent == null)
        {
            return;
        }

        Color color = isEnabled ? Color.white : Color.grey;
        button.ViewComponent.Enabled = isEnabled;
        button.TryGetChildView("Icon", out XUiV_Sprite sprite);
        button.TryGetChildView("Name", out XUiV_Label label);
        sprite?.Color = color;
        label?.Color = color;
    }

    #endregion
}