using System;
using Ace.Util;
using Godot;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class StatBar : TextureProgressBar 
{
    protected Tween Tween;
    protected float FillRate = 0.2f;
    protected AnimationPlayer AnimationPlayer;
    
    public StatBar Setup(float currentValue, float maxValue)
    {
        Value = currentValue;
        MaxValue = maxValue;
        AnimationPlayer = this.GetFirstChildOfType<AnimationPlayer>();
        Tween.Connect(Tween.SignalName.Finished, new Callable(this, nameof(OnTweenCompleted))); 
        return this;
    }
   
    public StatBar SetTargetValue(float newValue)
    {
        var targetValue = Mathf.Max(newValue, 0);
        Tween.TweenProperty(this, "value", Value, FillRate);
        Tween.TweenProperty(this, "value", targetValue, FillRate);
        Value = targetValue;
        return this;
    }
    
    public virtual void OnTweenCompleted()
    {
    }
}