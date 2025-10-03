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
        struct PlayerStat
        {
            public string Name;
            public float Value;
            public float MaxValue;

            public PlayerStat(string name, float value, float maxValue)
            {
                Name = name;
                Value = value;
                MaxValue = maxValue;
            }
        }
        private PlayerStat health;
        private PlayerStat stamina;
        private PlayerStat mana;
        private void Start()
        {
            health = new PlayerStat("Health", 100, 100);
            stamina = new PlayerStat("Stamina", 100, 100);
            mana = new PlayerStat("Mana", 100, 100);
        }
    }
}
