using Ace;
using Godot;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleScene : Control
{
    private TurnBar _uiTurnBar;
    private BattlerController _battlerController;

    public override void _Ready()
    {
        _uiTurnBar = Utils.GetFirstChildOfType<TurnBar>(this);
        _battlerController = Utils.GetFirstChildOfType<BattlerController>(this);
        var battlers = _battlerController.GetBattlers();
        _uiTurnBar.Setup(battlers);
    }

    public override void _Process(double delta)
    {

    }
}
