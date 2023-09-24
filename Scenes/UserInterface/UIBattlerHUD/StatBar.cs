using Ace.Util;
using Godot;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class StatBar : TextureProgressBar 
{
    protected Tween Tween;
    protected float FillRate = 0.2f;
    protected AnimationPlayer AnimationPlayer;
   
    public override void _Ready()
    {
    }
    
    public StatBar Setup(float currentValue, float maxValue)
    {
        Value = Mathf.RoundToInt(currentValue/maxValue * 100);
        MaxValue = 100;
        AnimationPlayer = this.GetFirstChildOfType<AnimationPlayer>();
        return this;
    }
   
    public StatBar SetTargetValue(float newValue)
    {
        var targetValue = Mathf.Max(newValue, 0);

        if (IsInsideTree())
        {
            Tween = CreateTween();
            Tween.Connect(Tween.SignalName.Finished, new Callable(this, nameof(OnTweenCompleted)));
            Tween.TweenProperty(this, "value", Value, FillRate);
            Tween.TweenProperty(this, "value", targetValue, FillRate);
        }
        Value = targetValue;
        return this;
    }
    
    public virtual void OnTweenCompleted()
    {
    }
}