using Ace.Util;
using Godot;

namespace Ace.Scenes.Battle;

public partial class BattleScene : Control
{
    private TurnBar.TurnBar _uiTurnBar;
    private BattlerController _battlerController;
    private UserInterface.UIBattlerHUD.BattlerHUDList _battlerHUDList;

    public override void _Ready()
    {
        _uiTurnBar = this.GetFirstChildOfType<TurnBar.TurnBar>();
        _battlerController = this.GetFirstChildOfType<BattlerController>();
        var battlers = _battlerController.GetBattlers();
        _uiTurnBar.Setup(battlers);
        _battlerHUDList = this.GetFirstChildOfType<UserInterface.UIBattlerHUD.BattlerHUDList>();
        _battlerHUDList = _battlerHUDList.Setup(battlers.Filter(b => b.InParty));
    }

    public override void _Process(double delta)
    {

    }
}