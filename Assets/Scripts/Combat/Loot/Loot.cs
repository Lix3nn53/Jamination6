using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;

public class Loot : MonoBehaviour
{
    [HideInInspector] public LootPool LootPool; // WindPool is set in WindPool.OnTakeFromPool
    [SerializeField] private LootType lootType;

    private Player _player;

    private ZombieTypeSelector zombieTypeSelector;

    private Slider virusProgressBar;

    private void Start()
    {
        _player = ServiceLocator.Get<Player>();
        zombieTypeSelector = ServiceLocator.Get<ZombieTypeSelector>();
        virusProgressBar = zombieTypeSelector.GetComponentInChildren<Slider>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        CombatUnit combatUnit = collision.gameObject.GetComponent<CombatUnit>();
        if (combatUnit != null)
        {
            //switch case
            switch (lootType)
            {
                case LootType.AttackerVirus:
                    _player.attackerVirus++;
                    if (zombieTypeSelector.activeType == ZombieType.Attacker) { virusProgressBar.value = _player.attackerVirus; }
                    break;
                case LootType.HealerVirus:
                    _player.healerVirus++;
                    if (zombieTypeSelector.activeType == ZombieType.Healer) { virusProgressBar.value = _player.healerVirus; }
                    break;
                case LootType.TankVirus:
                    _player.tankVirus++;
                    if (zombieTypeSelector.activeType == ZombieType.Tank) { virusProgressBar.value = _player.tankVirus; }
                    break;
                case LootType.CollectorVirus:
                    _player.collectorVirus++;
                    if (zombieTypeSelector.activeType == ZombieType.Collector) { virusProgressBar.value = _player.collectorVirus; }
                    break;
                case LootType.ScoreCollectable:
                    _player.scoreCollectable++;
                    break;
                case LootType.DamageBuff:
                    _player.damageBuff++;
                    break;
                case LootType.DefenseBuff:
                    _player.defenseBuff++;
                    break;
                case LootType.MovementBuff:
                    _player.movementBuff++;
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }

}