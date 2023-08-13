using Ace;
using Godot;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static Constants;

public partial class TurnBar : Control
{
    TextureRect _turnBarBackground;
    PackedScene _battlerIconResource;
    Seq<Battler> _battlers;

    [Export]
    public int BattlerIconPositionRatio = 4;

    public override void _Ready()
    {

        _turnBarBackground = Utils.GetFirstChildOfType<TextureRect>(this);
        const string battlerIconPath = "res://Scenes/Battle/TurnBar/BattlerIcon.tscn";
        _battlerIconResource = ResourceLoader.Load<PackedScene>(battlerIconPath);
    }

    public override void _Process(double delta)
    {
        UpdateAPForBattlers(delta);
    }

    public TurnBar Setup(Seq<Battler> battlers)
    {
        _battlers = battlers.Map(AddIconToBattler);
        _battlers.ToList().ForEach(battler =>
        {
            battler.Connect(Battler.SignalName.ReadyForInput, new Callable(this, nameof(HandlePlayerInput)));
            _turnBarBackground.AddChild(battler.TurnBarIcon);
        });
        return this;
    }

    private Unit UpdateAPForBattlers(double delta)
    {
        _battlers = _battlers.Map(battler => battler.UpdateAP(delta));
        _battlers.ToList()
            .ForEach(battler => battler.TurnBarIcon
                .Snap(battler.CurrentAP));

        bool areAnyPlayersWaiting = _battlers.Any(b => 
            b is PartyMember p
            && p.CurrentAPState == APState.WaitingForInput);

        if (areAnyPlayersWaiting) Engine.TimeScale = INPUT_WAIT_TIMESCALE;

        return new Unit();
    }
    
    public Battler HandlePlayerInput(Battler battler)
    {
        GD.Print($"HandlePlayerInput has been reached for {battler?.id.ToString() ?? "null"}");
        return battler;
    }
    
    public Battler AddIconToBattler(Battler battler)
    {
        var turnBarIcon = _battlerIconResource.Instantiate() as TurnBarIcon;
        turnBarIcon.Texture = battler.BattleIconBorder;
        turnBarIcon._battlerId = battler.id;
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
