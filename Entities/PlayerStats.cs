using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerStats : MonoBehaviour
    {
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float BaseSpeed { get; private set; } = 7f;
        [field: SerializeField] public float AirAcceleration { get; private set; } = 50f;
        [field: SerializeField] public float AirDeceleration { get; private set; } = 20f;
        [field: SerializeField] public int PlayerHealth { get; set; } = 100;
        [field: SerializeField] public float DashForce { get; private set; } = 20f;

        private void Start()
        {
            AkUnitySoundEngine.SetState("PlayerLife", "Alive");
            AkUnitySoundEngine.PostEvent("Play_Heartbeat", gameObject);

            AkUnitySoundEngine.SetRTPCValue("PlayerHealth", PlayerHealth, gameObject);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TakeDamage(21);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Heal(21);
            }
        }
        private void UpdateHeartbeatRTPC()
        {
            AkUnitySoundEngine.SetRTPCValue("PlayerHealth", PlayerHealth, gameObject);
        }
        public void TakeDamage(int damageAmount)
        {
            PlayerHealth -= damageAmount;
            UpdateHeartbeatRTPC();
            Die();
        }
        public void Heal(int healAmount)
        {
            PlayerHealth += healAmount;
            UpdateHeartbeatRTPC();
            if (PlayerHealth > 100)
                PlayerHealth = 100;
        }
        private void Die()
        {
            if (PlayerHealth <= 0)
            {
                AkUnitySoundEngine.SetState("PlayerLife", "Dead");
                Debug.Log("Player Died");
                // Add death logic here (e.g., respawn, game over screen, etc.)
            }
        }
        //struct PlayerStat
        //{
        //    public string Name;
        //    public float Value;
        //    public float MaxValue;

        //    public PlayerStat(string name, float value, float maxValue)
        //    {
        //        Name = name;
        //        Value = value;
        //        MaxValue = maxValue;
        //    }
        //}
        //private PlayerStat health;
        //private PlayerStat stamina;
        //private PlayerStat mana;
        //private void Start()
        //{
        //    health = new PlayerStat("Health", 100, 100);
        //    stamina = new PlayerStat("Stamina", 100, 100);
        //    mana = new PlayerStat("Mana", 100, 100);
        //}
    }
}
