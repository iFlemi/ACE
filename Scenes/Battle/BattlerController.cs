using Ace.Models;
using Ace.Util;
using Godot;

namespace Ace.Scenes.Battle;

public partial class BattlerController : Node2D
{
	public Seq<Battler> Battlers { get; private set; }
	public ulong Id { get; private set; }

	public override void _Ready()
	{
		Id = GetInstanceId();
		Battlers = Utils.GetAllChildrenOfType<Battler>(this).ToSeq();
	}

	public override void _Process(double delta)
	{
	}

	public Seq<Battler> GetBattlers() => Battlers;
}