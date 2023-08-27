namespace Ace.Util;

public enum ApState
{
    Unknown,
    Waiting,
    ReadyForInput,
    WaitingForInput,
    Activating,
    ReadyToActivate
}

public enum EnhancementLayer
{
    Material,
    Expertise,
    Enchantment,
    Divine,
    Enigmatic,
    Absolute
}

public enum StatType
{
    Unknown = -1,
    Strength,
    Agility,
    Intelligence,
    Power,
    Willpower,
    Endurance,
    Speed,
    Critical,
    Evasion,
    Health,
    Shield,
    Stamina,
    DamageMulti,
    SpellDamageMulti,
    DamageResistance,
}