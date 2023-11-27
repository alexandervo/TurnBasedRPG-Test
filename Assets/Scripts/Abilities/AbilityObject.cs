using Kryz.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

    public abstract class AbilityObject : ScriptableObject
{
    [SerializeField]
    protected string abilityName;
    [SerializeField]
    protected string description;
    [SerializeField]
    protected bool applySourceStats;
    [SerializeField]
    protected Stats sourceStats;
    [SerializeField]
    protected bool stackSourceStats;
    [SerializeField]
    protected bool applyTargetStats;
    [SerializeField]
    protected Stats targetStats;
    [SerializeField]
    protected bool stackTargetStats;
    [SerializeField]
    protected StatModType sourceStatModType;
    [SerializeField]
    protected StatModType targetStatModType;
    [SerializeField]
    protected SelectionType selectionType;
    [SerializeField]
    protected SelectionMode selectionMode;
    [SerializeField]
    protected bool canTargetDeadActors;
    [SerializeField]
    protected bool canUseOutsideBattle;
    [SerializeField]
    protected Cost[] costs;
    [SerializeField]
    protected bool sourceModLastTillXRounds;
    [SerializeField]
    protected bool sourceModLastTillEndEncounter;
    [SerializeField]
    protected int sourceMinModRoundsToLast;
    [SerializeField]
    protected int sourceMaxModRoundsToLast;
    [SerializeField]
    protected bool targetModLastTillXRounds;
    [SerializeField]
    protected bool targetModLastTillEndEncounter;
    [SerializeField]
    protected int targetMinModRoundsToLast;
    [SerializeField]
    protected int targetMaxModRoundsToLast;
    [SerializeField]
    protected GameObject sourceAnimPrefab;
    [SerializeField]
    protected GameObject targetAnimPrefab;
    [SerializeField]
    protected string sfxNameToPlay;

    /// <summary>
    /// Name of the ability.
    /// </summary>
    public string Name { get => abilityName; protected set => abilityName = value; }

    /// <summary>
    /// Description of the ability.
    /// </summary>
    public string Description { get => description; protected set => description = value; }

    /// <summary>
    /// When true <see cref="sourceStats"/> will be applied.
    /// </summary>
    public bool ApplySourceStats { get => applySourceStats; protected set => applySourceStats = value; }

    /// <summary>
    /// Source Stats for the ability.
    /// </summary>
    public Stats SourceStats { get => sourceStats; protected set => sourceStats = value; }

    /// <summary>
    /// When true source stats will be able to be stacked.
    /// </summary>
    public bool StackSourceStats { get => stackSourceStats; protected set => stackSourceStats = value; }

    /// <summary>
    /// When true <see cref="targetStats"/> will be applied.
    /// </summary>
    public bool ApplyTargetStats { get => applyTargetStats; protected set => applyTargetStats = value; }

    /// <summary>
    /// Target Stats for the ability.
    /// </summary>
    public Stats TargetStats { get => targetStats; protected set => targetStats = value; }

    /// <summary>
    /// When true target stats will be able to be stacked.
    /// </summary>
    public bool StackTargetStats { get => stackTargetStats; protected set => stackTargetStats = value; }

    /// <summary>
    /// Type of mod type to apply for stat increases see <see cref="sourceStatModType"/>
    /// </summary>
    public StatModType SourceStatModType { get => sourceStatModType; protected set => sourceStatModType = value; }

    /// <summary>
    /// Type of mod type to apply for stat increases see <see cref="targetStatModType"/>
    /// </summary>
    public StatModType TargetStatModType { get => targetStatModType; protected set => targetStatModType = value; }

    /// <summary>
    /// Type of selection for the ability <see cref="SelectionType"/>
    /// </summary>
    public SelectionType SelectionType { get => selectionType; protected set => selectionType = value; }

    /// <summary>
    /// Type of selection mode for targeting for the ability <see cref="SelectionMode"/>
    /// </summary>
    public SelectionMode SelectionMode { get => selectionMode; protected set => selectionMode = value; }

    /// <summary>
    /// When true the ability can target actors that are marked as <see cref="IBattleActor.IsDead"/>
    /// </summary>
    public bool TargetDeadActors { get => canTargetDeadActors; protected set => canTargetDeadActors = value; }

    /// <summary>
    /// When true the ability can be used in the character menu outside of battle.
    /// </summary>
    public bool CanUseOutsideBattle { get => canUseOutsideBattle; protected set => canUseOutsideBattle = value; }

    /// <summary>
    /// An array of Costs for the ability can have multiple costs or no costs based on the ability.
    /// </summary>
    public Cost[] Costs { get => costs; protected set => costs = value; }

    /// <summary>
    /// When true causes stats to be applied for <see cref="sourceMinModRoundsToLast"/>
    /// </summary>
    public bool SourceModLastTillXRounds { get => sourceModLastTillXRounds; protected set => sourceModLastTillXRounds = value; }

    /// <summary>
    /// When true causes stats to last until the enc of the encounter.
    /// </summary>
    public bool SourceModLastTillEndEncounter { get => sourceModLastTillEndEncounter; protected set => sourceModLastTillEndEncounter = value; }

    /// <summary>
    /// Minumum number of rounds for stat buff/debuff to last before being removed on the source.
    /// </summary>
    public int SourceMinModRoundsToLast { get => sourceMinModRoundsToLast; protected set => sourceMinModRoundsToLast = value; }

    /// <summary>
    /// Minumum number of rounds for stat buff/debuff to last before being removed on the source.
    /// </summary>
    public int SourceMaxModRoundsToLast { get => sourceMaxModRoundsToLast; protected set => sourceMaxModRoundsToLast = value; }

    /// <summary>
    /// When true causes stats to be applied for <see cref="targetMinModRoundsToLast"/>
    /// </summary>
    public bool TargetModLastTillXRounds { get => TargetModLastTillXRounds; protected set => TargetModLastTillXRounds = value; }

    /// <summary>
    /// When true causes stats to last until the enc of the encounter.
    /// </summary>
    public bool TargetModLastTillEndEncounter { get => targetModLastTillEndEncounter; protected set => targetModLastTillEndEncounter = value; }

    /// <summary>
    /// Minimum number of rounds for stat buff/debuff to last before being removed on the target/s.
    /// </summary>
    public int TargetMinModRoundsToLast { get => targetMinModRoundsToLast; protected set => targetMinModRoundsToLast = value; }

    /// <summary>
    /// Maximum number of rounds for stat buff/debuff to last before being removed on the target/s.
    /// </summary>
    public int MaxTargetModRoundsToLast { get => targetMaxModRoundsToLast; protected set => targetMaxModRoundsToLast = value; }

    /// <summary>
    /// The GameObject to spawn on the source GameObject for an animation.
    /// </summary>
    public GameObject SourceAnimPrefab { get => sourceAnimPrefab; protected set => sourceAnimPrefab = value; }

    /// <summary>
    /// The GameObject to spawn on the target GameObject for an animation.
    /// </summary>
    public GameObject TargetAnimPrefab { get => targetAnimPrefab; protected set => targetAnimPrefab = value; }

    /// <summary>
    /// The SFX to playwhen ability is used.
    /// </summary>
    public string SfxNameToPlay { get => sfxNameToPlay; protected set => sfxNameToPlay = value; }

    /// <summary>
    /// This Method is called to activate the ability and apply the respective effects of the ability.
    /// </summary>
    /// <param name="actorSource"></param>
    /// <param name="actorTargets"></param>
    /// <returns></returns>
    /// 
    //public abstract void Activate(IBattleActor actorSource, params IBattleActor[] actorTargets);
}
