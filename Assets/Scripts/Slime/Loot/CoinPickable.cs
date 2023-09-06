using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player;
using Zenject;

namespace SlimeBounce.Slime.Loot
{
    public class CoinPickable : PickableLoot
    {
        [Header("Coin Parameters")]
        [SerializeField] private int _coinReward;
        [Inject]
        ICoinActor _coinActor;

        protected override void ApplyLootEffects()
        {
            _coinActor.ChangeCoins(_coinReward, transform.position);
        }
    }
}