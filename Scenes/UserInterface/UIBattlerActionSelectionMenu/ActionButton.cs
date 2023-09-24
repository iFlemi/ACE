using Ace.Models.Abilities;
using Ace.Util;
using Godot;
using Vector2 = Godot.Vector2;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class ActionButton : TextureButton
{
    private TextureRect _icon;
    private Label _label;
    public ActiveAbility Ability { get; private set; }
    
    public void OnPressed() =>
        ReleaseFocus();
    
    public ActionButton Setup(ActiveAbility ability)
    {
        Ability = ability;
        _icon = this.GetFirstChildOfType<TextureRect>();
        _label = this.GetFirstChildOfType<Label>();
        _icon.Texture = ability.MenuIcon;
        _label.Text = ability.Name;
        _label.TooltipText = ability.Description;
        return this;
    }

    public Vector2 GetMenuArrowPosition() =>
         Position + new Vector2(Size.X, Size.Y/2);
}