﻿using UnityEngine;

namespace Unite.BuffSystem
{
    [CreateAssetMenu(fileName = "HealthRegenBuff", menuName = "Buffs/Health Regen")]
    public class HealthRegenerationBuff : BuffScriptableObject
    {
        [SerializeField]
        private float regenerationPercentage;

        [SerializeField]
        private float intervalInSeconds;
        
        public override void ApplyBuff(Player.Player player)
        {
            base.ApplyBuff(player);
            
            player.HealthHandler.ApplyRegeneration(regenerationPercentage, intervalInSeconds);
        }
    }
}