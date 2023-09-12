using Ace.Models;
using Ace.Util;
using Godot;

namespace Ace.Scenes.Battle.TurnBar;

public partial class TurnBar : Control
{
    TextureRect _turnBarBackground;
    PackedScene _battlerIconResource;
    Seq<Battler> _battlers;

    [Export]
    public int BattlerIconPositionRatio = 4;

    public override void _Ready()
    {

        _turnBarBackground = this.GetFirstChildOfType<TextureRect>();
        _battlerIconResource = ResourceLoader.Load<PackedScene>(BattlerIconPath);
    }

    public override void _Process(double delta)
    {
        UpdateApForBattlers(delta);
    }

    public TurnBar Setup(Seq<Battler> battlers)
    {
        _battlers = battlers.Map(AddIconToBattler);
        _battlers.Iter(battler =>
        {
            battler.Connect(Battler.SignalName.ReadyForInput, new Callable(this, nameof(HandlePlayerInput)));
            _turnBarBackground.AddChild(battler.TurnBarIcon);
        });
        return this;
    }

    private Unit UpdateApForBattlers(double delta)
    {
        _battlers = _battlers.Map(battler => 
                battler.UpdateAp(delta))
            .Do(battler => battler.TurnBarIcon
                .Snap(battler.CurrentAp));

        var areAnyPlayersWaiting = _battlers.Any(b => 
            b is PartyMember p
            && p.CurrentApState == ApState.WaitingForInput);

        if (areAnyPlayersWaiting) Engine.TimeScale = InputWaitTimescale;

        return new Unit();
    }
    
    public Battler HandlePlayerInput(Battler battler)
    {
        this.Log($"HandlePlayerInput has been reached for {battler?.Id.ToString() ?? "null"}");
        return battler;
    }
    
    public Battler AddIconToBattler(Battler battler)
    {
        var turnBarIcon = _battlerIconResource.Instantiate() as TurnBarIcon;
        turnBarIcon.Texture = battler.BattleIconBorder;
        turnBarIcon.BattlerId = battler.Id;
        turnBarIcon = turnBarIcon.SetPositionRange(_turnBarBackground.Size);
        turnBarIcon.SetPosition(new Vector2(turnBarIcon.Position.X, battler.InParty 
            ? Size.Y/ BattlerIconPositionRatio 
            : (BattlerIconPositionRatio -1) * Size.Y/BattlerIconPositionRatio));
        turnBarIcon = turnBarIcon.SetBattlerIcon(battler.BattleIcon);
        //GD.Print($"battlerIcon: {GD.VarToStr(battlerIcon)}");
        battler.TurnBarIcon = turnBarIcon;
        return battler;
    }
}