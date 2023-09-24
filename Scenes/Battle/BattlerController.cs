using Ace.Util;
using Godot;
using Battler = Ace.Models.Character.Battler;

namespace Ace.Scenes.Battle;

public partial class BattlerController : Node2D
{
	public Seq<Battler> Battlers { get; private set; }
	public ulong Id { get; private set; }

	public override void _Ready()
	{
		Id = GetInstanceId();
		Battlers = this.GetAllChildrenOfType<Battler>().ToSeq();
	}

	public override void _Process(double delta)
	{
	}

	public Seq<Battler> GetBattlers() => Battlers;
}