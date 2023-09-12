using Godot;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class MenuSelectArrow : Marker2D
{
    public override void _Ready()
    {
        TopLevel = true;
    }

    public MenuSelectArrow MoveTo(Vector2 target)
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "position", Position, 0.2f);
        tween.TweenProperty(this, "position", target, 0.2f);
        return this;
    }
}