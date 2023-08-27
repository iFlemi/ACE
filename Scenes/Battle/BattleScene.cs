using Ace.Util;
using Godot;

namespace Ace.Scenes.Battle;

public partial class BattleScene : Control
{
    private TurnBar.TurnBar _uiTurnBar;
    private BattlerController _battlerController;

    public override void _Ready()
    {
        _uiTurnBar = Utils.GetFirstChildOfType<TurnBar.TurnBar>(this);
        _battlerController = Utils.GetFirstChildOfType<BattlerController>(this);
        var battlers = _battlerController.GetBattlers();
        _uiTurnBar.Setup(battlers);
    }

    public override void _Process(double delta)
    {

    }
}