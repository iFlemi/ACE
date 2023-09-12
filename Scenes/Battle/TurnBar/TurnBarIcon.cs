using Ace.Util;
using Godot;

namespace Ace.Scenes.Battle.TurnBar;

public partial class TurnBarIcon : TextureRect
{
    public Vector2 PositionRange = Vector2.Zero;
    
    public TextureRect BattlerIcon;
    public ulong BattlerId;

    private float HalfX => Size.X / 2.0f;

    public override void _Ready()
    {
        BattlerIcon = this.GetFirstChildOfType<TextureRect>();
    }

    public TurnBarIcon Snap(float ratio, bool isEnemy = false)
    {
        var newPositionX = Mathf.Lerp(PositionRange.X, PositionRange.Y, ratio);
        SetPosition(new Vector2(newPositionX, Position.Y));
        return this;
    }

    public TurnBarIcon SetBattlerIcon(Texture2D battlerIconTexture)
    {
        //GD.Print($"SetBattlerIcon: {GD.VarToStr(_battlerIcon)}");
        BattlerIcon ??= this.GetFirstChildOfType<TextureRect>();
        BattlerIcon.Texture = battlerIconTexture;
        return this;
    }

    public TurnBarIcon SetPositionRange(Vector2 parentSize)
    {
        PositionRange = new Vector2(-HalfX, -HalfX + parentSize.X);
        return this;
    }
}