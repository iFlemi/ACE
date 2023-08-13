using Ace;
using Godot;
using LanguageExt;
using System;
using Ace.Models;

public partial class TurnBarIcon : TextureRect
{
    public Vector2 _positionRange = Vector2.Zero;
    
    public TextureRect _battlerIcon;
    public Guid _battlerId;

    private float halfX => Size.X / 2.0f;

    public override void _Ready()
    {
        _battlerIcon = Utils.GetFirstChildOfType<TextureRect>(this);
    }

    public TurnBarIcon Snap(float ratio, bool isEnemy = false)
    {
        var newPositionX = Mathf.Lerp(_positionRange.X, _positionRange.Y, ratio);
        SetPosition(new Vector2(newPositionX, Position.Y));
        return this;
    }

    public TurnBarIcon SetBattlerIcon(Texture2D battlerIconTexture)
    {
        //GD.Print($"SetBattlerIcon: {GD.VarToStr(_battlerIcon)}");
        _battlerIcon ??= Utils.GetFirstChildOfType<TextureRect>(this);
        _battlerIcon.Texture = battlerIconTexture;
        return this;
    }

    public TurnBarIcon SetPositionRange(Vector2 parentSize)
    {
        _positionRange = new Vector2(-halfX, -halfX + parentSize.X);
        return this;
    }
}

