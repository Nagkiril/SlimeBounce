using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities;

namespace SlimeBounce.Slime.Loot
{
    public interface ILootEnvironmentProvider
    {
        public LootEnvironment GetEnvironment();
    }

    //We use the LootEnvironment to check whether or not a loot prefab can be spawned, without actually instnatiating\using factory for it if there's no need for that
    //It's a bit of a crutch - if more loot types would need other things in the environment (current gold count, what level is Player, etc), we WILL need to modify the environment and essentially violate open-closed principle
    //However, alternatives are either using static\singleton data for it (as it worked prior to DI) - which is usually frowned upon, or having to create, inject, and destroy unneeded object which sounds much worse
    //We could also use a Prototype pattern: create and inject just one disabled object, check and clone it - but I don't want to keep instances of loot that can't ever spawn, and neither want to make a smart tracker that would optimize it
    public class LootEnvironment
    {
        public IAbilityCooldownHub CooldownHub;
    }
}