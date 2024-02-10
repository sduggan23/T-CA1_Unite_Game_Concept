﻿using System.Collections.Generic;
using Unite.Enemies;
using Unite.EventSystem;
using Unite.InteractionSystem;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using UnityEngine.Analytics;

namespace Unite.Managers
{
    public class AnalyticsManager : MonoBehaviour
    {
        async void Start()
        {
            try
            {
                // Initialize Unity Services asynchronously
                await UnityServices.InitializeAsync();
                GiveConsent(); // Get user consent according to various legislations
                //OnLevelCompleted();
            }
            catch (ConsentCheckException e)
            {
                Debug.Log(e.ToString());
            }
        }

        public void PlayerDied(PlayerDiedInfo info, Enemy enemy)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            //data.Add("Death_Position_x", info.DeathPosition.x);
            //data.Add("Death_Position_y", info.DeathPosition.y);
            //data.Add("Death_Position_z", info.DeathPosition.z);
            data.Add("Killed_By_Enemy", info.KilledByAttacker);
            data.Add("Killed_By_Attack", info.KilledByAttack);

            // Add more data related to player death
            // Example: data.Add("Player_Health", info.PlayerHealth);

            // Send analytics event
            SendAnalyticsEvent("PlayerDied", data);
        }

        public void EnemyDefeated(Enemy enemy)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Name", enemy.DisplayName);

            SendAnalyticsEvent("EnemyDefeated", data);
        }

        public void TimeStopUsed()
        {
            //Add more data related to time stop usage
            Dictionary<string, object> data = new Dictionary<string, object>();

           //Send analytics event
           SendAnalyticsEvent("TimeStopUsed", data);
        }

    public void OnInteractWithInteractible(InteractibleObject interactible)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Name", interactible.DisplayName);

            // Send analytics event
            SendAnalyticsEvent("InteractWithInteractible", data);
        }

        public void PlayerReachedCheckpoint(Vector3 checkpointPosition)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Checkpoint_Position_x", checkpointPosition.x);
            data.Add("Checkpoint_Position_y", checkpointPosition.y);
            data.Add("Checkpoint_Position_z", checkpointPosition.z);

            // Add more data related to checkpoint reached event
            // Example: data.Add("Player_Health", player.Health);

            // Send analytics event
            SendAnalyticsEvent("PlayerReachedCheckpoint", data);
        }

        public void PlayerUsedPowerup(string powerupType)
        {
            // Add more data related to powerup usage
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Name", powerupType);

            // Send analytics event
            SendAnalyticsEvent("PlayerUsedPowerup", data);
        }

        // Add more events and data as needed

        private void OnLevelCompleted()
        {
            int currentLevel = Random.Range(1, 4); // Gets a random number from 1-3

            // Create a dictionary of custom parameters with the key "levelName"
            // and a value of the form "levelX," where X is the randomly generated level number.
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "levelName", "level" + currentLevel.ToString()}
            };

            // The ‘levelCompleted’ event will get cached locally
            // and sent during the next scheduled upload, within 1 minute
            SendAnalyticsEvent("levelCompleted", parameters, true);
        }

        private void OnDestroy()
        {
            Analytics.FlushEvents();
        }

        public void GiveConsent()
        {
            // Call if consent has been given by the user
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log($"Consent has been provided. The SDK is now collecting data!");
        }

        private void SendAnalyticsEvent(string eventName, Dictionary<string, object> data, bool flushImmediately = false)
        {
            // Send analytics event
            AnalyticsService.Instance.CustomData(eventName, data);

            // Flush events immediately if specified
            if (flushImmediately)
            {
                AnalyticsService.Instance.Flush();
            }
        }
    }
}
