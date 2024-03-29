﻿using System.Collections;
using System.Collections.Generic;

namespace Ace.Models.Stats;

public record AllStats : IEnumerable<Stat>
{
  public PrimaryStats PrimaryStats { get; set; } = new();
  public SecondaryStats SecondaryStats { get; set; } = new();
  public VitalStats VitalStats { get; set; } = new();

  public IEnumerator<Stat> GetEnumerator() =>
    (PrimaryStats as IEnumerable<Stat>).ConcatFast(SecondaryStats).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public record PrimaryStats : IEnumerable<PrimaryStat>
{
  public Strength Strength = new();
  public Agility Agility = new();
  public Intelligence Intelligence = new();
  public Power Power = new();
  public Willpower Willpower = new();
  public Endurance Endurance = new();

  public IEnumerator<PrimaryStat> GetEnumerator()
  {
    yield return Strength;
    yield return Agility;
    yield return Intelligence;
    yield return Power;
    yield return Willpower;
    yield return Endurance;
  }
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  
}

public record SecondaryStats : IEnumerable<SecondaryStat>
{
  public Speed Speed = new();
  public Critical Critical = new();
  public Evasion Evasion = new();
  public DamageMulti DamageMulti = new();
  public SpellDamageMulti SpellDamageMulti = new();
  public DamageResistance DamageResistance = new();

  public IEnumerator<SecondaryStat> GetEnumerator()
  {
    yield return Speed;
    yield return Critical;
    yield return Evasion;
    yield return SpellDamageMulti;
    yield return DamageMulti;
    yield return DamageResistance;
  }
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public record VitalStats : IEnumerable<VitalStat>
{
  public Health Health = new();
  public Stamina Stamina = new();
  public Shield Shield = new();
  
  public VitalStats InitialiseCurrent()
  {
    Health.InitialiseCurrent();
    Stamina.InitialiseCurrent();
    Shield.InitialiseCurrent();
    return this;
  }
  public IEnumerator<VitalStat> GetEnumerator()
  {
    yield return Health;
    yield return Stamina;
    yield return Shield;
  }
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public record CurrentVitals(float CurrentHealth, float CurrentStamina, float CurrentShield);