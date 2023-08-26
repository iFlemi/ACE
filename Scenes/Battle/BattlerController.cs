using Godot;
using LanguageExt;
using static LanguageExt.Prelude;
using Ace.Models;

public partial class BattlerController : Node2D
{
	Seq<Battler> _battlers;

	public override void _Ready()
	{
		_battlers = Utils.GetAllChildrenOfType<Battler>(this).ToSeq();
	}

	public override void _Process(double delta)
	{
	}

	public Seq<Battler> GetBattlers() => 
		Seq1(_battlers.Head());
		//_battlers;
}
