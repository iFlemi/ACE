using Godot;

namespace Ace.Interfaces;

public interface IDamageAllocator
{
  DamageAllocation AllocateDamage(int damage, int shield, int stamina, int health, float damageResistance);
}

public record DamageAllocation(int RemainingShield, int RemainingStamina, int RemainingHealth);

public class StandardDamageAllocator : IDamageAllocator
{
    public DamageAllocation AllocateDamage(int damage, int shield, int stamina, int health, float damageResistance) =>
     Mathf.RoundToInt(damage * (1-damageResistance)) switch
    {
        var d when d >= shield + stamina + health => new DamageAllocation(0, 0, 0),
        var d when d >= shield + stamina => new DamageAllocation(0, 0, d - shield - stamina >= health ? 0 : health - (d - shield - stamina)),
        var d when d  >= shield => new DamageAllocation(0, d - shield >= stamina ? 0 : stamina - (d - shield), d - shield >= stamina ? 0 : health),
        _ => new DamageAllocation(shield - damage, stamina, health)
    };
}

public class SoftenedShieldDamageAllocator : IDamageAllocator
{
    private readonly float _shieldHardness = 0.5f;
    public SoftenedShieldDamageAllocator(float shieldHardness)
    {
        _shieldHardness = shieldHardness;
    }

    public DamageAllocation AllocateDamage(int dmg, int shield, int stamina, int health, float damageResistance)
    {
        var damage = Mathf.RoundToInt(dmg * (1 - damageResistance));
        var shieldDamage = (int)(damage * _shieldHardness);
        var remainingDamage = damage - shieldDamage;

        var remainingShield = Mathf.Max(0, shield - shieldDamage);
        var remainingStamina = stamina - Mathf.Min(stamina, remainingDamage);
        var damageAfterStaminaDepleted = Mathf.Max(0, remainingDamage - stamina);
        var remainingHealth = health;
        if (damageAfterStaminaDepleted > 0) { 
            remainingHealth = Mathf.Max(0, health - damageAfterStaminaDepleted);
        }
        return new DamageAllocation(remainingShield, remainingStamina, remainingHealth);
    }
}
