using Ace;
using Godot;
using LanguageExt;
using System;

namespace Ace.Models;

public partial class Enemy : Battler
{
    public override bool InParty => false;
}