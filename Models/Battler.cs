using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Constants;

namespace Ace
{
    public abstract partial class Battler : Sprite2D
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public float Strength { get; set; } = 10;
        public float Willpower { get; set; } = 10;
        public float Endurance { get; set; } = 10;
        public float Power { get; set; } = 10;
        [Export]
        public float Agility { get; set; } = 10;
        [Export]
        public float Intelligence { get; set; } = 10;
        [Export]
        public Texture2D BattleIcon { get; set; }
        [Export]
        public Texture2D BattleIconBorder { get; set; }

        public float CurrentAP { get; set; } = 0;
        public APState CurrentAPState { get; set; }

        public TurnBarIcon TurnBarIcon { get; set; }

        public abstract bool InParty { get; }
        public virtual float GetSpeed() => Agility;

        [Signal]
        public delegate void ReadyForInputEventHandler(Battler battler);
        [Signal]
        public delegate void ReadyToActEventHandler(Battler battler);

        public Battler UpdateAP(double delta)
        {
            (CurrentAPState, CurrentAP) = GetNewAPAndAPState((float)delta);
            EmitSignalsWhereRequired();
            return this;
        }

        private void EmitSignalsWhereRequired()
        {
            switch (CurrentAPState)
            {
                case APState.ReadyForInput:
                    EmitSignal(SignalName.ReadyForInput, this);
                    break;
                case APState.ReadyToActivate:
                    EmitSignal(SignalName.ReadyToAct, this);
                    break;
                case APState.Unknown:
                    GD.PrintErr($"Battler {id} - hit {nameof(APState)}: {APState.Unknown}\r\nnewAP: {CurrentAP}");
                    break;
                default:
                    break;
            }
        }

        protected virtual float CalculateNewAP(float delta) => 
            (float)(CurrentAP + (GetSpeed() * delta) / 100);

        private (APState newState, float newAP) GetNewAPAndAPState(float delta)
        {
            var oldAP = CurrentAP;
            var adjustedAP = CalculateNewAP(delta);

            return adjustedAP switch
            {
                var ap when ap <= 0 => (APState.Waiting, 0),
                var ap when ap < AP_BAR_ACTION_POINT => (APState.Waiting, ap),
                var ap when ap >= AP_BAR_ACTION_POINT && oldAP < AP_BAR_ACTION_POINT => (APState.ReadyForInput, AP_BAR_ACTION_POINT),
                var ap when ap >= AP_BAR_ACTION_POINT && (CurrentAPState == APState.ReadyForInput || CurrentAPState == APState.WaitingForInput) => (APState.WaitingForInput, AP_BAR_ACTION_POINT),
                var ap when ap >= AP_BAR_ACTION_POINT && oldAP < 1.00f => (APState.Activating, ap),
                var ap when ap >= 1.00f && oldAP < 1.00f => (APState.ReadyToActivate, 1.00f),
                var ap when ap >= 1.00f => (APState.ReadyToActivate, 1.00f),
                _ => (APState.Unknown, CurrentAP)
            };
        }

        public override int GetHashCode() => id.GetHashCode();

        public override bool Equals(object obj) => obj is Battler other && other.id == id;

    }

}
