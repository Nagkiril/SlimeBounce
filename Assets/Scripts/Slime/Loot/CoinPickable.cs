using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player;

namespace SlimeBounce.Slime.Loot
{
    public class CoinPickable : PickableLoot
    {
        [Header("Coin Parameters")]
        [SerializeField] int coinReward;

        protected override void ApplyLootEffects()
        {
            CoinController.ChangeCoins(coinReward, transform.position);
        }
    }
}