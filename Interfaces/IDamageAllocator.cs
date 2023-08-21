using System;

namespace Ace.Interfaces;

public interface IDamageAllocator
{
    (int RemainingShield, int RemainingStamina, int RemainingHealth) AllocateDamage(int damage, int shield, int stamina, int health);
}

public class StandardDamageAllocator : IDamageAllocator
{
    public (int RemainingShield, int RemainingStamina, int RemainingHealth) AllocateDamage(int damage, int shield, int stamina, int health) =>
    damage switch
    {
        var d when d >= shield + stamina + health => (0, 0, 0),
        var d when d >= shield + stamina => (0, 0, d - shield - stamina >= health ? 0 : health - (d - shield - stamina)),
        var d when d >= shield => (0, d - shield >= stamina ? 0 : stamina - (d - shield), d - shield >= stamina ? 0 : health),
        _ => (shield - damage, stamina, health)
    };
}

public class SoftenedShieldDamageAllocator : IDamageAllocator
{
    private readonly float _shieldHardness = 0.5f;
    public SoftenedShieldDamageAllocator(float shieldHardness)
    {
        _shieldHardness = shieldHardness;
    }

    public (int RemainingShield, int RemainingStamina, int RemainingHealth) AllocateDamage(int damage, int shield, int stamina, int health)
    {
        var shieldDamage = (int)(damage * _shieldHardness);
        var remainingDamage = damage - shieldDamage;

        var remainingShield = Math.Max(0, shield - shieldDamage);
        var remainingStamina = stamina - Math.Min(stamina, remainingDamage);
        var damageAfterStaminaDepleted = Math.Max(0, remainingDamage - stamina);
        var remainingHealth = health;
        if (damageAfterStaminaDepleted > 0) { 
            remainingHealth = Math.Max(0, health - damageAfterStaminaDepleted);
        }
        return (remainingShield, remainingStamina, remainingHealth);
    }
}
