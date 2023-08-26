using LanguageExt;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Ace.Models.Stats;

public record AllStats : IEnumerable<Stat>
{
  public PrimaryStats PrimaryStats { get; set; } = new();
  public SecondaryStats SecondaryStats { get; set; } = new();

  public IEnumerator<Stat> GetEnumerator() =>
    (PrimaryStats as IEnumerable<Stat>).ConcatFast(SecondaryStats).GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public override string ToString() =>
    JsonConvert.SerializeObject(this);
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
  public Health Health = new();
  public Shield Shield = new();
  public Stamina Stamina = new();

  public IEnumerator<SecondaryStat> GetEnumerator()
  {
    yield return Speed;
    yield return Critical;
    yield return Evasion;
    yield return Health;
    yield return Shield;
    yield return Stamina;
  }
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

