using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New Magic Ability", menuName = "Abilities/Magic Ability", order = 14)]
    /// <summary>
    /// This Ability is for Magic related abilities.
    /// </summary>
    public class MagicAbilityObject : AbilityObject
    {
        [SerializeField]
        protected CombatStyle combatStyle;
        [SerializeField]
        protected bool useWeaponCombatStyle;
        [SerializeField]
        protected bool maxDamageAlways;
        [SerializeField]
        protected bool ignoreDefense;
        [SerializeField]
        protected bool dealDamage;
        [SerializeField]
        protected bool restoreHp;
        [SerializeField]
        protected bool restoreMp;
        [SerializeField]
        protected bool restoreTp;
        [SerializeField]
        protected int restoreHpAmount;
        [SerializeField]
        protected int restoreMpAmount;
        [SerializeField]
        protected int restoreTpAmount;
        [SerializeField]
        protected StatusCalcType restoreType;
        [SerializeField]
        protected bool stealHp;
        [SerializeField]
        protected bool stealMp;
        [SerializeField]
        protected bool stealTp;
        //[SerializeField]
        //protected List<StatusEffectObject> statusEffects = new();
        [SerializeField]
        protected bool onlyApplyStatusEffectOnDamage;
        [SerializeField]
        protected List<string> removeStatusEffects;

        //private List<IBattleActor> targets = new List<IBattleActor>();
        //private IBattleActor source;
        private int sourceRoundToWait;
        private int targetRoundToWait;
        /// <summary>
        /// Type of <see cref="TBS.CombatStyle"/> style to use for the ability.
        /// </summary>
        public CombatStyle CombatStyle { get => combatStyle; protected set => combatStyle = value; }

        /// <summary>
        /// When true applies weapon stats to damage calculation.
        /// </summary>
        public bool UseWeaponCombatStyle { get => useWeaponCombatStyle; protected set => useWeaponCombatStyle = value; }

        /// <summary>
        /// When true causes the damage of the ability to always be the max calculated output not randomly generated.
        /// </summary>
        public bool MaxDamageAlways { get => maxDamageAlways; protected set => maxDamageAlways = value; }

        /// <summary>
        /// When true causes the damage calculation to ignore the opposing targets defense. 
        /// </summary>
        public bool IgnoreDefense { get => ignoreDefense; protected set => ignoreDefense = value; }

        /// <summary>
        /// When true causes damage calculation on respective targets.
        /// </summary>
        public bool DealDamage { get => dealDamage; protected set => dealDamage = value; }

        /// <summary>
        /// When true causes restoring of HP for the respective targets.
        /// </summary>
        public bool RestoreHp { get => restoreHp; protected set => restoreHp = value; }

        /// <summary>
        /// When true causes restoring of MP for the respective targets.
        /// </summary>
        public bool RestoreMp { get => restoreMp; protected set => restoreMp = value; }

        /// <summary>
        /// When true causes restoring of TP for the respective targets.
        /// </summary>
        public bool RestoreTp { get => restoreTp; protected set => restoreTp = value; }

        /// <summary>
        /// When <see cref="RestoreHp"/> is true this will restore the respective amount of HP.
        /// </summary>
        public int RestoreHpAmount { get => restoreHpAmount; protected set => restoreHpAmount = value; }

        /// <summary>
        /// When <see cref="RestoreMp"/> is true this will restore the respective amount of MP.
        /// </summary>
        public int RestoreMpAmount { get => restoreMpAmount; protected set => restoreMpAmount = value; }

        /// <summary>
        /// When <see cref="RestoreTp"/> is true this will restore the respective amount of TP.
        /// </summary>
        public int RestoreTpAmount { get => restoreTpAmount; protected set => restoreTpAmount = value; }

        /// <summary>
        /// The RestoreType to be applied to the restoring.
        /// </summary>
        public StatusCalcType RestoreType { get => restoreType; protected set => restoreType = value; }


        /// <summary>
        /// When true causes the damage to steal HP from respective targets.
        /// </summary>
        public bool StealHp { get => stealHp; protected set => stealHp = value; }

        /// <summary>
        /// When true causes the damage to steal MP from respective targets.
        /// </summary>
        public bool StealMp { get => stealMp; protected set => stealMp = value; }

        /// <summary>
        /// When true causes the damage to steal TP from respective targets.
        /// </summary>
        public bool StealTp { get => stealTp; protected set => stealTp = value; }

        /// <summary>
        /// The list of status effects to inflict on target/s
        /// </summary>
        //public List<StatusEffectObject> StatusEffects { get => statusEffects; protected set => statusEffects = value; }


        /// <summary>
        /// When true status effects will only apply when more than 0 damage is dealt.
        /// </summary>
        public bool OnlyApplyStatusEffectOnDamage { get => onlyApplyStatusEffectOnDamage; protected set => onlyApplyStatusEffectOnDamage = value; }
        /// <summary>
        /// A list of strings that reflect which status effects to remove.
        /// </summary>
        public List<string> RemoveStatusEffects { get => removeStatusEffects; set => removeStatusEffects = value; }
       
        /// <summary>
        /// Called when the gameobject is enabled. This OnEnable is responsible for assigning events to respective methods.
        /// </summary>
        //private void OnEnable()
       // {
        //    EventManager.onEncounterEnd += EventManager_onEncounterEnd;
        //    EventManager.onRoundEnd += EventManager_onRoundEnd;
        //}

        /// <summary>
        /// Called when the gameobject is disabled. This OnDisable is responsible for unassinging events from their respective methods.
        /// </summary>
       /* private void OnDisable()
        {
            EventManager.onEncounterEnd -= EventManager_onEncounterEnd;
            EventManager.onRoundEnd -= EventManager_onRoundEnd;
        }

        public override void Activate(IBattleActor actorSource, params IBattleActor[] actorTargets)
        {
            targets.AddRange(actorTargets);
            source = actorSource;

            bool canActivate = false;
            if (costs.Length <= 0) { canActivate = true; }

            for (int i = 0; i < costs.Length; i++)
            {
                var cost = costs[i];
                if (cost.Type == CostType.MP)
                {
                    if (actorSource.Mana.CurrentValue >= cost.Amount)
                    {
                        canActivate = true;
                        actorSource.Mana.CurrentValue -= cost.Amount;
                    }
                }
                if (cost.Type == CostType.TP)
                {
                    if (actorSource.Tp.CurrentValue >= cost.Amount)
                    {
                        canActivate = true;
                        actorSource.Tp.CurrentValue -= cost.Amount;
                    }
                }
                if (cost.Type == CostType.HP)
                {
                    if (actorSource.Health.CurrentValue > cost.Amount)
                    {
                        canActivate = true;
                        actorSource.Health.CurrentValue -= cost.Amount;
                    }
                }
            }
            if (canActivate)
            {
                if (!string.IsNullOrWhiteSpace(sfxNameToPlay))
                {
                    AudioManager.instance.PlaySound(sfxNameToPlay);
                }
                if (sourceAnimPrefab != null)
                {
                    Instantiate(sourceAnimPrefab, actorSource.gameObject.transform);
                }
                sourceRoundToWait = Random.Range(sourceMinModRoundsToLast, sourceMaxModRoundsToLast + 1) + actorSource.CurrentRound;
                targetRoundToWait = Random.Range(targetMinModRoundsToLast, targetMaxModRoundsToLast + 1) + actorSource.CurrentRound;
                if (applySourceStats)
                {
                    if (actorSource.Stats.CheckAnyModifiersOnObject(this) && stackSourceStats || !actorSource.Stats.CheckAnyModifiersOnObject(this))
                    {
                        actorSource.Stats.AddAllModifiers(sourceStats, sourceStatModType, this);
                    }
                }
                for (int i = 0; i < actorTargets.Length; i++)
                {
                    if(targetAnimPrefab != null)
                    {
                        Instantiate(targetAnimPrefab, actorTargets[i].gameObject.transform);
                    }
                    if (applyTargetStats)
                    {
                        if (actorTargets[i].Stats.CheckAnyModifiersOnObject(this) && stackTargetStats || !actorTargets[i].Stats.CheckAnyModifiersOnObject(this))
                        {
                            actorTargets[i].Stats.AddAllModifiers(targetStats, targetStatModType, this);
                        }
                    }
                    if (dealDamage)
                    {
                        int dmg = 0;
                        int actorDefense = 0;
                        if (actorSource.EquipmentSlots[0] != null && useWeaponCombatStyle)
                        {
                            switch (actorSource.EquipmentSlots[0].CombatStyle)
                            {
                                case CombatStyle.Melee:
                                    dmg = actorSource.Stats.MeleeAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.MeleeDefense.Value;
                                    break;
                                case CombatStyle.Range:
                                    dmg = actorSource.Stats.RangeAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.RangeDefense.Value;
                                    break;
                                case CombatStyle.Magic:
                                    dmg = actorSource.Stats.MagicAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.MagicDefense.Value;
                                    break;
                            }
                        }
                        else
                        {
                            switch (combatStyle)
                            {
                                case CombatStyle.Melee:
                                    dmg = actorSource.Stats.MeleeAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.MeleeDefense.Value;
                                    break;
                                case CombatStyle.Range:
                                    dmg = actorSource.Stats.RangeAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.RangeDefense.Value;
                                    break;
                                case CombatStyle.Magic:
                                    dmg = actorSource.Stats.MagicAttack.Value + actorSource.Stats.BaseDamage.Value;
                                    actorDefense = actorTargets[i].Stats.BaseDefense.Value + actorTargets[i].Stats.MagicDefense.Value;
                                    break;
                            }
                        }
                        var dmgCalc = 0;
                        if (maxDamageAlways && !ignoreDefense)
                        {
                            dmgCalc = dmg - Random.Range(0, actorDefense);
                        }
                        else if (!maxDamageAlways && !ignoreDefense)
                        {
                            dmgCalc = Random.Range(0, dmg + 1) - Random.Range(0, actorDefense);
                        }
                        else if (maxDamageAlways && ignoreDefense)
                        {
                            dmgCalc = dmg;
                        }
                        else if (!maxDamageAlways && ignoreDefense)
                        {
                            dmgCalc = Random.Range(0, dmg + 1);
                        }
                        if (dmgCalc > 0)
                        {
                            if (onlyApplyStatusEffectOnDamage)
                            {
                                for (int s = 0; s < statusEffects.Count; s++)
                                {
                                    var statusEffect = statusEffects[s];
                                    if (statusEffect != null)
                                    {
                                        statusEffect.Activate(actorSource, actorTargets[i]);
                                    }
                                }
                            }
                            actorTargets[i].Health.CurrentValue -= dmgCalc;
                            if (stealHp)
                            {
                                if (actorTargets[i].Health.CurrentValue > 0)
                                {
                                    int diff = 0;

                                    if (actorTargets[i].Health.CurrentValue >= dmgCalc)
                                    {
                                        diff = dmgCalc;
                                    }
                                    else
                                    {
                                        diff = dmgCalc - actorTargets[i].Health.CurrentValue;
                                    }
                                    actorTargets[i].Health.CurrentValue -= diff;
                                    actorSource.Health.CurrentValue += diff;
                                    EventManager.ActorHitPopup(actorSource, diff, Color.red);
                                }
                            }
                            if (stealMp)
                            {
                                if (actorTargets[i].Mana.CurrentValue > 0)
                                {
                                    int diff = 0;

                                    if (actorTargets[i].Mana.CurrentValue >= dmgCalc)
                                    {
                                        diff = dmgCalc;
                                    }
                                    else
                                    {
                                        diff = dmgCalc - actorTargets[i].Mana.CurrentValue;
                                    }
                                    actorTargets[i].Mana.CurrentValue -= diff;
                                    actorSource.Mana.CurrentValue += diff;
                                    EventManager.ActorHitPopup(actorSource, diff, Color.blue);
                                }

                            }
                            if (stealTp)
                            {
                                if (actorTargets[i].Tp.CurrentValue > 0)
                                {
                                    int diff = 0;

                                    if (actorTargets[i].Mana.CurrentValue >= dmgCalc)
                                    {
                                        diff = dmgCalc;
                                    }
                                    else
                                    {
                                        diff = dmgCalc - actorTargets[i].Mana.CurrentValue;
                                    }
                                    actorTargets[i].Tp.CurrentValue -= diff;
                                    actorSource.Tp.CurrentValue += diff;
                                    EventManager.ActorHitPopup(actorSource, diff, Color.green);
                                }
                            }
#if UNITY_EDITOR
                            Debug.Log($"{actorSource.ActorName} Uses Magic: {abilityName} {actorTargets[i].ActorName} for {dmgCalc} damage. {actorSource.ActorName} MaxDamage: {dmg - actorDefense} {actorTargets[i].ActorName} Defense: {actorDefense}");
#endif
                        }
#if UNITY_EDITOR
                        else { Debug.Log($"{actorSource.ActorName} Uses Magic: {abilityName} {actorTargets[i].ActorName} and Misses! {actorSource.ActorName} MaxDamage: {dmg - actorDefense} {actorTargets[i].ActorName} Defense: {actorDefense}"); }
#endif
                        EventManager.ActorHitPopup(actorTargets[i], dmgCalc, Color.white);
                    }
                    if (!onlyApplyStatusEffectOnDamage)
                    {
                        for (int s = 0; s < statusEffects.Count; s++)
                        {
                            var statusEffect = statusEffects[s];
                            if (statusEffect != null)
                            {
                                statusEffect.Activate(actorSource, actorTargets[i]);
                            }
                        }
                    }

                    float actualRestoreHp = 0;
                    float actualRestoreMp = 0;
                    float actualRestoreTp = 0;
                    if (restoreType == StatusCalcType.Flat)
                    {
                        actualRestoreHp = restoreHpAmount;
                        actualRestoreMp = restoreMpAmount;
                        actualRestoreTp = restoreTpAmount;
                    }

                    if (restoreType == StatusCalcType.Percentage)
                    {
                        actualRestoreHp = (float)actorTargets[i].Health.MaxValue * ((float)restoreHpAmount / 100f);
                        actualRestoreMp = (float)actorTargets[i].Mana.MaxValue * ((float)restoreMpAmount / 100f);
                        actualRestoreTp = (float)actorTargets[i].Tp.MaxValue * ((float)restoreTpAmount / 100f);
                    }

                    if (restoreHp)
                    {
                        actorTargets[i].Health.CurrentValue += (int)actualRestoreHp;
                        EventManager.ActorHitPopup(actorTargets[i], (int)actualRestoreHp, Color.red);

                    }
                    if (restoreMp)
                    {
                        actorTargets[i].Mana.CurrentValue += (int)actualRestoreMp;
                        EventManager.ActorHitPopup(actorTargets[i], (int)actualRestoreMp, Color.blue);
                    }
                    if (restoreTp)
                    {
                        actorTargets[i].Tp.CurrentValue += (int)actualRestoreTp;
                        EventManager.ActorHitPopup(actorTargets[i], (int)actualRestoreTp, Color.green);
                    }
                    if (!targetModLastTillXRounds && !targetModLastTillEndEncounter)
                    {
                        actorTargets[i].Stats.RemoveAllModifiersFromObject(this);
                    }
                    for (int s = 0; s < removeStatusEffects.Count; s++)
                    {
                        var effect = actorTargets[i].StatusEffects.Find(x => x.EffectName == removeStatusEffects[s]);
                        if (effect != null)
                        {
                            effect.Deactivate(removeStatusEffects[s], actorTargets[i]);
                        }
                    }
                }
                if (!sourceModLastTillXRounds && !sourceModLastTillEndEncounter && applySourceStats)
                {
                    actorSource.Stats.RemoveAllModifiersFromObject(this);
                }
            }
        }
        /// <summary>
        /// This method is invoked from the event <see cref="EventManager.onEncounterEnd"/> handles renoving stats from targets.
        /// </summary>
        private void EventManager_onEncounterEnd()
        {
            if(source != null)
            {
                source.Stats.RemoveAllModifiersFromObject(this);
            }
            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target != null)
                {
                    target.Stats.RemoveAllModifiersFromObject(this);
                }
            }
        }

        /// <summary>
        /// This method is invoked from the event <see cref="EventManager.onRoundEnd"/> handles removing stats from targets/source.
        /// </summary>
        private void EventManager_onRoundEnd()
        {
            if (targetModLastTillXRounds && applyTargetStats)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    var target = targets[i];
                    if (target != null)
                    {
                        if (target.CurrentRound >= targetRoundToWait)
                        {
                            target.Stats.RemoveAllModifiersFromObject(this);
                        }
                    }
                }
            }
            if (sourceModLastTillXRounds && applySourceStats)
            {
                if (source.CurrentRound >= sourceRoundToWait)
                {
                    source.Stats.RemoveAllModifiersFromObject(this);

                }
            }
        }
       */
    }