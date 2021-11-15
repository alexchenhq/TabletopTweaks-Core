﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Config;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewActions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.NewComponents.AbilitySpecific;
using TabletopTweaks.NewComponents.Properties;
using TabletopTweaks.Utilities;
using UnityEngine;
using static TabletopTweaks.NewUnitParts.CustomStatTypes;

namespace TabletopTweaks.NewContent.Archetypes {
    static class BladeBound {
        public static void AddBladeBound() {
            var MagusClass = Resources.GetBlueprint<BlueprintCharacterClass>("45a4607686d96a1498891b3286121780");
            var MagusArcanaSelection = Resources.GetBlueprint<BlueprintFeature>("e9dc4dfc73eaaf94aae27e0ed6cc9ada");
            var ArcanePoolResourse = Resources.GetBlueprint<BlueprintAbilityResource>("effc3e386331f864e9e06d19dc218b37");
            var Alertness = Resources.GetBlueprint<BlueprintFeature>("1c04fe9a13a22bc499ffac03e6f79153");
            var Unholy = Resources.GetBlueprint<BlueprintWeaponEnchantment>("d05753b8df780fc4bb55b318f06af453");
            var SpellResistanceBuff = Resources.GetBlueprint<BlueprintBuff>("50a77710a7c4914499d0254e76a808e5");
            var Fatigued = Resources.GetBlueprint<BlueprintBuff>("e6f2fc5d73d88064583cb828801212f4");
            var Exhausted = Resources.GetBlueprint<BlueprintBuff>("46d1b9cc3d0fd36469a471b047d773a2");
            //var SpellResistanceBuff = Resources.GetBlueprint<BlueprintBuff>("50a77710a7c4914499d0254e76a808e5");

            var BastardSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("91a4b3f6b4b53ae4fb3095cba86a38ca");
            var BattleAxPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("bf20773f9c880144d989e4a6f41071c7");
            var DuelingSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("c23265c7960b5c144a200eafda0e7cf1");
            var DwarvenWarAx = Resources.GetBlueprint<BlueprintItemWeapon>("c3b25150bbf1bea42887176bbe2306b2");
            var FalcataPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("3e14db6284db73d40b4a4b99943e2018");
            var HandAxePlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("506a2d43fbbbe7041ad57f05900478db");
            var KamaPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("5b8394d717f0789418146692d561cd36");
            var KukuriPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("86cae2531ed5df641aa57e5fb24a88c0");
            var LongSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("7453fb8aa1cd7f3428a14eeadc2022d7");
            var RapierPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("0e2b2a13f286c10499921633a557388c");
            var ScimitarPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("af2a9b2b3a6905f49a44e4676a39cea8");
            var ShortSwordPlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("b5f6e218fb193a24cb00bdec435732ff");
            var SicklePlus5 = Resources.GetBlueprint<BlueprintItemWeapon>("5733378292a9fd547aeb7eccb7e79c60");

            var Icon_SpellResistance = AssetLoader.LoadInternal("Abilities", "Icon_SpellResistance.png");
            var Icon_TransferArcana = AssetLoader.LoadInternal("Abilities", "Icon_TransferArcana.png");
            var Icon_BlackBladeStrike = AssetLoader.LoadInternal("Abilities", "Icon_BlackBladeStrike.png");
            var Icon_ElementalAttunment = AssetLoader.LoadInternal("Abilities", "Icon_ElementalAttunment.png");
            var Icon_WarriorSpirit_Bane = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_Bane.png");
            var Icon_WarriorSpirit_Flaming = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_Flaming.png");
            var Icon_WarriorSpirit_Frost = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_Frost.png");
            var Icon_WarriorSpirit_Shock = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_Shock.png");
            var Icon_WarriorSpirit_Thundering = AssetLoader.LoadInternal("Abilities", "Icon_WarriorSpirit_Thundering.png");

            var BlackBladeEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnchantment", bp => {
                bp.SetName("Black Blade");
                bp.SetDescription("A black blade's enhancement bonus scales with a Bladebound's level. It is +1 at level 3 and increases by 1 every 4 levels thereafter.");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.m_IdentifyDC = 0;
                bp.AddComponent<WeaponBlackBladeEnhancementBonus>();
            });

            var BlackBladeBastardSword = CreateBlackBlade(BastardSwordPlus5, BlackBladeEnchantment);
            var BlackBladeBattleAx = CreateBlackBlade(BattleAxPlus5, BlackBladeEnchantment);
            var BlackBladeDuelingSword = CreateBlackBlade(DuelingSwordPlus5, BlackBladeEnchantment);
            var BlackBladeDwarvenWarAx = CreateBlackBlade(DwarvenWarAx, BlackBladeEnchantment);
            var BlackBladeFalcata = CreateBlackBlade(FalcataPlus5, BlackBladeEnchantment);
            var BlackBladeHandAxe = CreateBlackBlade(HandAxePlus5, BlackBladeEnchantment);
            var BlackBladeKama = CreateBlackBlade(KamaPlus5, BlackBladeEnchantment);
            var BlackBladeKukuri = CreateBlackBlade(KukuriPlus5, BlackBladeEnchantment);
            var BlackBladeLongSword = CreateBlackBlade(LongSwordPlus5, BlackBladeEnchantment);
            var BlackBladeRapier = CreateBlackBlade(RapierPlus5, BlackBladeEnchantment);
            var BlackBladeScimitar = CreateBlackBlade(ScimitarPlus5, BlackBladeEnchantment);
            var BlackBladeShortSword = CreateBlackBlade(ShortSwordPlus5, BlackBladeEnchantment);
            var BlackBladeSickle = CreateBlackBlade(SicklePlus5, BlackBladeEnchantment);

            var BladeBoundArchetype = Helpers.CreateBlueprint<BlueprintArchetype>("BladeBoundArchetype", bp => {
                    bp.SetName("Bladebound");
                    bp.SetDescription("A select group of magi are called to carry a black blade, a sentient " +
                        "weapon of often unknown and possibly unknowable purpose. These weapons become " +
                        "valuable tools and allies, as both the magus and weapon typically crave arcane power, " +
                        "but as a black blade becomes more aware, its true motivations manifest, and as does its " +
                        "ability to influence its wielder with its ever-increasing ego.");
            });
            
            var BlackBladeArcanePool = Helpers.CreateBlueprint<BlueprintAbilityResource>("BlackBladeArcanePool", bp => {
                bp.m_Min = 1;
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount {
                    BaseValue = 1,
                    IncreasedByStat = true,
                    ResourceBonusStat = CustomStatType.BlackBladeIntelligence.Stat(),
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    IncreasedByLevelStartPlusDivStep = false,
                    StartingLevel = 1,
                    LevelStep = 1,
                    PerStepIncrease = 1,
                    StartingIncrease = 1
                };
            });
            var BlackBladeEgoProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>("BlackBladeEgoProperty", bp => {
                bp.AddComponent<StatValueGetter>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                });
            });

            var BlackBladeStrikeEnchantment = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeStrikeEnchantment", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeStrike>();
            });
            var BlackBladeStrikeBuff = Helpers.CreateBuff("BlackBladeStrikeBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Black Blade Strike");
                bp.SetDescription("As a free action, the magus can spend a point from the black blade’s arcane pool " +
                    "to grant the black blade a +1 bonus on damage rolls for 1 minute. For every four levels beyond 1st, " +
                    "this ability gives the black blade another +1 on damage rolls.");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_BlackBladeStrike;
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.DamageBonus;
                    c.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
                    c.m_Progression = ContextRankProgression.StartPlusDivStep;
                    c.m_StartLevel = 1;
                    c.m_StepLevel = 4;
                    c.m_Max = 5;
                    c.m_Min = 1;
                    c.m_UseMin = true;
                    c.m_UseMax = true;
                    c.m_Class = new BlueprintCharacterClassReference[] { MagusClass.ToReference<BlueprintCharacterClassReference>() };
                    c.Archetype = BladeBoundArchetype.ToReference<BlueprintArchetypeReference>();
                    c.m_AdditionalArchetypes = new BlueprintArchetypeReference[0];
                });
            });
            var BlackBladeStrikeAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeStrikeAbility", bp => {
                bp.SetName(BlackBladeStrikeBuff.m_DisplayName);
                bp.SetDescription(BlackBladeStrikeBuff.Description);
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration","1 minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = BlackBladeStrikeBuff.Icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = BlackBladeStrikeBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        }),
                        Helpers.Create<ContextActionApplyWeaponEnchant>(a => {
                            a.Enchantments = new BlueprintItemEnchantmentReference[]{
                                BlackBladeStrikeEnchantment.ToReference<BlueprintItemEnchantmentReference>()
                            };
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Minutes,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            var BlackBladeStrike = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeStrike", bp => {
                bp.SetName(BlackBladeStrikeAbility.m_DisplayName);
                bp.SetDescription(BlackBladeStrikeAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeStrikeAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeStrikeBuff.Icon;
            });

            var BlackBladeEnergyAttunementCold = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementCold", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Cold
                    };
                });
            });
            var BlackBladeEnergyAttunementElectricity = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementElectricity", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Electricity
                    };
                });
            });
            var BlackBladeEnergyAttunementFire = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementFire", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Fire
                    };
                });
            });
            var BlackBladeEnergyAttunementSonic = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementSonic", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Energy,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData(),
                        Energy = DamageEnergyType.Sonic
                    };
                });
            });
            var BlackBladeEnergyAttunementForce = Helpers.CreateBlueprint<BlueprintWeaponEnchantment>($"BlackBladeEnergyAttunementForce", bp => {
                bp.SetName("");
                bp.SetDescription("");
                bp.SetPrefix("");
                bp.SetSuffix("");
                bp.WeaponFxPrefab = Unholy.WeaponFxPrefab;
                bp.m_EnchantmentCost = 0;
                bp.AddComponent<WeaponBlackBladeElementalAttunement>(c => {
                    c.Type = new DamageTypeDescription() {
                        Type = DamageType.Force,
                        Common = new DamageTypeDescription.CommomData(),
                        Physical = new DamageTypeDescription.PhysicalData()
                    };
                });
            });
            var BlackBladeEnergyAttunementBuff = Helpers.CreateBuff("BlackBladeEnergyAttunementBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Energy Attunement");
                bp.SetDescription("At 5th level, as a free action, a magus can spend a point of his black blade’s arcane pool to have it deal " +
                    "one of the following types of damage instead of weapon damage: cold, electricity, or fire. He can spend 2 points from the " +
                    "black blade’s arcane pool to deal sonic or force damage instead of weapon damage. This effect lasts until the start of the magus’s next turn.");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_ElementalAttunment;
            });
            var BlackBladeEnergyAttunementColdAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementColdAbility", 
                "Energy Attunement — Cold", 
                BlackBladeArcanePool, 1, 
                BlackBladeEnergyAttunementCold, 
                BlackBladeEnergyAttunementBuff,
                Icon_WarriorSpirit_Frost
            );
            var BlackBladeEnergyAttunementElectricityAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementElectricityAbility",
                "Energy Attunement — Electricity",
                BlackBladeArcanePool, 1,
                BlackBladeEnergyAttunementElectricity,
                BlackBladeEnergyAttunementBuff,
                Icon_WarriorSpirit_Shock
            );
            var BlackBladeEnergyAttunementFireAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementFireAbility",
                "Energy Attunement — Fire",
                BlackBladeArcanePool, 1,
                BlackBladeEnergyAttunementFire,
                BlackBladeEnergyAttunementBuff,
                Icon_WarriorSpirit_Flaming
            );
            var BlackBladeEnergyAttunementSonicAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementSonicAbility",
                "Energy Attunement — Sonic",
                BlackBladeArcanePool, 2,
                BlackBladeEnergyAttunementSonic,
                BlackBladeEnergyAttunementBuff,
                Icon_WarriorSpirit_Thundering
            );
            var BlackBladeEnergyAttunementForceAbility = CreateEnergyAttunement(
                "BlackBladeEnergyAttunementForceAbility",
                "Energy Attunement — Force",
                BlackBladeArcanePool, 2,
                BlackBladeEnergyAttunementForce,
                BlackBladeEnergyAttunementBuff,
                Icon_WarriorSpirit_Bane
            );
            var BlackBladeEnergyAttunementBaseAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeEnergyAttunementBaseAbility", bp => {
                bp.SetName("Energy Attunement");
                bp.SetDescription("At 5th level, as a free action, a magus can spend a point of his black blade’s arcane pool to have it deal one " +
                    "of the following types of damage instead of weapon damage: cold, electricity, or fire. He can spend 2 points from the black " +
                    "blade’s arcane pool to deal sonic or force damage instead of weapon damage. This effect lasts until the start of the magus’s next turn.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.LocalizedDuration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = Icon_ElementalAttunment;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.Personal;
                bp.CanTargetFriends = true;
                bp.CanTargetSelf = true;
                bp.NeedEquipWeapons = true;
                bp.AddComponent<AbilityVariants>(c => {
                    c.m_Variants = new BlueprintAbilityReference[] {
                        BlackBladeEnergyAttunementColdAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementElectricityAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementFireAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementSonicAbility.ToReference<BlueprintAbilityReference>(),
                        BlackBladeEnergyAttunementForceAbility.ToReference<BlueprintAbilityReference>(),
                    };
                });
            });
            var BlackBladeEnergyAttunement = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEnergyAttunement", bp => {
                bp.SetName(BlackBladeEnergyAttunementBaseAbility.m_DisplayName);
                bp.SetDescription(BlackBladeEnergyAttunementBaseAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeEnergyAttunementBaseAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeEnergyAttunementBaseAbility.Icon;
            });

            var BlackBladeTransferArcanaAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeTransferArcanaAbility", bp => {
                bp.SetName("Transfer Arcana");
                bp.SetDescription("At 13th level a magus can attempt to siphon points from his black blade’s arcane pool " +
                    "into his own arcane pool. Doing so takes a full-round action and the magus must succeed at a Will saving throw with " +
                    "a DC equal to the black blade’s ego. If the magus succeeds, he regains 1 point to his arcane pool for every 2 points " +
                    "he saps from his black blade. If he fails the saving throw, the magus becomes fatigued. If he is " +
                    "fatigued, he becomes exhausted instead. He cannot use this ability if he is exhausted.");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.m_Icon = Icon_TransferArcana;
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.m_IsFullRoundAction = true;
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = Helpers.CreateActionList(
                        Helpers.Create<ContextActionTransferArcana>(a => {
                            a.m_sourceResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                            a.m_sourceAmount = 2;
                            a.m_destinationResource = ArcanePoolResourse.ToReference<BlueprintAbilityResourceReference>();
                            a.m_destinationAmount = 1;
                            a.SaveDC = new ContextValue() {
                                ValueType = ContextValueType.CasterCustomProperty,
                                m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                            };
                            a.FailedActions = Helpers.CreateActionList(
                                new Conditional {
                                    ConditionsChecker = new ConditionsChecker {
                                        Conditions = new Condition[] {
                                            Helpers.Create<ContextConditionHasFact>(c => {
                                                c.m_Fact = Fatigued.ToReference<BlueprintUnitFactReference>();
                                                c.Not = true;
                                            })
                                        }
                                    },
                                    IfTrue = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Fatigued.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    ),
                                    IfFalse = Helpers.CreateActionList(
                                        Helpers.Create<ContextActionApplyBuff>(c => {
                                            c.m_Buff = Exhausted.ToReference<BlueprintBuffReference>();
                                            c.Permanent = true;
                                            c.DurationValue = new ContextDurationValue();
                                        })
                                    )
                                }
                            );
                        })
                    );
                });
            });
            var BlackBladeTransferArcana = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeTransferArcana", bp => {
                bp.SetName(BlackBladeTransferArcanaAbility.m_DisplayName);
                bp.SetDescription(BlackBladeTransferArcanaAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeTransferArcanaAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeTransferArcanaAbility.Icon;
            });

            var BlackBladeSpellDefenseBuff = Helpers.CreateBuff("BlackBladeSpellDefenseBuff", bp => {
                bp.Ranks = 1;
                bp.SetName("Spell Defense");
                bp.SetDescription("A magus of 17th level or higher can expend an arcane point from his weapon’s arcane pool as a free action; " +
                    "he then gains SR equal to his black blade’s ego until the start of his next turn.");
                bp.IsClassFeature = true;
                bp.m_Icon = Icon_SpellResistance;
                bp.AddComponent<AddSpellResistance>(c => {
                    c.Value = new ContextValue() {
                        ValueType = ContextValueType.CasterCustomProperty,
                        m_CustomProperty = BlackBladeEgoProperty.ToReference<BlueprintUnitPropertyReference>()
                    };
                });
            });
            var BlackBladeSpellDefenseAbility = Helpers.CreateBlueprint<BlueprintAbility>("BlackBladeSpellDefenseAbility", bp => {
                bp.SetName(BlackBladeSpellDefenseBuff.m_DisplayName);
                bp.SetDescription(BlackBladeSpellDefenseBuff.Description);
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = BlackBladeSpellDefenseBuff.Icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = 1;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = BlackBladeSpellDefenseBuff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            var BlackBladeSpellDefense = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeSpellDefense", bp => {
                bp.SetName(BlackBladeSpellDefenseAbility.m_DisplayName);
                bp.SetDescription(BlackBladeSpellDefenseAbility.m_Description);
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        BlackBladeSpellDefenseAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.m_Icon = BlackBladeSpellDefenseAbility.Icon;
            });

            var BlackBladeLifeDrinker = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeLifeDrinker", bp => {
                bp.SetName("Life Drinker");
                bp.SetDescription("At 19th level, each time the magus kills a living creature with the black blade, he can pick one " +
                    "of the following effects: the black blade restores 2 points to its arcane pool; the black blade restores 1 point " +
                    "to its arcane pool and the magus restores 1 point to his arcane pool; the magus gains a number of temporary hit " +
                    "points equal to the black blade’s ego (these temporary hit points last until spent or 1 minute, whichever is shorter). " +
                    "The creature killed must have a number of Hit Dice equal to half the magus’s character level for this to occur.");
                bp.IsClassFeature = true;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        //BlackBladeTransferArcanaAbility.ToReference<BlueprintUnitFactReference>(),
                    };
                });
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                //bp.m_Icon = BlackBladeSpellDefenseBuff.Icon;
            });

            var BlackBladeBaseStats = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeBaseStats", bp => {
                bp.SetName("Black Blade Base Stats");
                bp.SetDescription("");
                bp.Ranks = 1;
                //bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 5;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeIntelligence.Stat();
                    c.Value = 11;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeCharisma.Stat();
                    c.Value = 7;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeWisdom.Stat();
                    c.Value = 7;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeStatIncrease = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeMentalIncrease", bp => {
                bp.SetName("Black Blade Stat Increase");
                bp.SetDescription("The Black Blade's mental stats increases by 1.");
                bp.Ranks = 8;
                //bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeIntelligence.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeCharisma.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeWisdom.Stat();
                    c.Value = 1;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease2 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease2", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 2.");
                bp.Ranks = 6;
                //bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 2;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease3 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease3", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 3.");
                bp.Ranks = 1;
                //bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 3;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeEgoIncrease4 = Helpers.CreateBlueprint<BlueprintFeature>("BlackBladeEgoIncrease4", bp => {
                bp.SetName("Black Blade Ego Increase");
                bp.SetDescription("The Black Blade's ego increases by 4.");
                bp.Ranks = 1;
                //bp.m_Icon = SlayerSwiftStudyTargetFeature.Icon;
                bp.IsClassFeature = true;
                bp.AddComponent<AddStatBonus>(c => {
                    c.Stat = CustomStatType.BlackBladeEgo.Stat();
                    c.Value = 4;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                });
            });
            var BlackBladeProgression = Helpers.CreateBlueprint<BlueprintProgression>("BlackBladeProgression", bp => {
                bp.SetName("Black Blade");
                bp.SetDescription("At level 3 the Bladebound will gain a black blade that will grow stronger along side them.");
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[] {
                    new BlueprintProgression.ArchetypeWithLevel(){
                        m_Archetype = BladeBoundArchetype.ToReference<BlueprintArchetypeReference>()
                    }
                };
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeatureRankIncrease = new BlueprintFeatureReference();
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(3, BlackBladeBaseStats, Alertness, BlackBladeStrike),
                    Helpers.CreateLevelEntry(5, BlackBladeStatIncrease, BlackBladeEgoIncrease3, BlackBladeEnergyAttunement),
                    Helpers.CreateLevelEntry(7, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                    Helpers.CreateLevelEntry(9, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                    Helpers.CreateLevelEntry(11, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                    Helpers.CreateLevelEntry(13, BlackBladeStatIncrease, BlackBladeEgoIncrease2, BlackBladeTransferArcana),
                    Helpers.CreateLevelEntry(15, BlackBladeStatIncrease, BlackBladeEgoIncrease2),
                    Helpers.CreateLevelEntry(17, BlackBladeStatIncrease, BlackBladeEgoIncrease4, BlackBladeSpellDefense),
                    Helpers.CreateLevelEntry(19, BlackBladeStatIncrease, BlackBladeEgoIncrease2, BlackBladeLifeDrinker),
                };
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = BlackBladeArcanePool.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
                bp.UIGroups = new UIGroup[] { 
                    Helpers.CreateUIGroup(BlackBladeEgoIncrease2, BlackBladeEgoIncrease3, BlackBladeEgoIncrease4),
                    Helpers.CreateUIGroup(BlackBladeBaseStats, BlackBladeStatIncrease),
                    Helpers.CreateUIGroup(BlackBladeStrike, BlackBladeEnergyAttunement, BlackBladeTransferArcana, BlackBladeSpellDefense, BlackBladeLifeDrinker)
                };
            });
            BladeBoundArchetype.RemoveFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(3, MagusArcanaSelection)
            };
            BladeBoundArchetype.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, BlackBladeProgression),
            };
            if (ModSettings.AddedContent.Archetypes.IsDisabled("BladeBound")) { return; }

            MagusClass.m_Archetypes = MagusClass.m_Archetypes.AppendToArray(BladeBoundArchetype.ToReference<BlueprintArchetypeReference>());
            /*
            MagusClass.Progression.UIGroups = WarpriestClass.Progression.UIGroups.AppendToArray(
                Helpers.CreateUIGroup(DivineCommanderCompanionSelection, DivineCommanderBlessedMount),
                Helpers.CreateUIGroup(DivineCommanderBattleTacticianFeature, DivineCommanderBattleTacticianGreaterFeature),
                Helpers.CreateUIGroup(DivineCommanderBattleTacticianSelection)
            );
            */
            Main.LogPatch("Added", BladeBoundArchetype);
        }

        private static BlueprintAbility CreateEnergyAttunement(
            string name, 
            string DisplayName, 
            BlueprintAbilityResource resource, int cost, 
            BlueprintWeaponEnchantment enchant, 
            BlueprintBuff buff,
            Sprite icon = null) {

            var EnergyAttunement = Helpers.CreateBlueprint<BlueprintAbility>(name, bp => {
                bp.SetName(DisplayName);
                bp.SetDescription("");
                bp.LocalizedDuration = Helpers.CreateString($"{bp.name}.Duration", "1 minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.Range = AbilityRange.Personal;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Immediate;
                bp.ActionType = UnitCommand.CommandType.Free;
                bp.m_Icon = icon;
                bp.AddComponent<AbilityResourceLogic>(c => {
                    c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_IsSpendResource = true;
                    c.Amount = cost;
                });
                bp.AddComponent<AbilityRequirementHasBuff>(c => {
                    c.RequiredBuff = buff.ToReference<BlueprintBuffReference>();
                    c.Not = true;
                });
                bp.AddComponent<AbilityEffectRunAction>(c => {
                    c.Actions = new ActionList();
                    c.Actions.Actions = new GameAction[] {
                        Helpers.Create<ContextActionApplyBuff>(a => {
                            a.m_Buff = buff.ToReference<BlueprintBuffReference>();
                            a.IsNotDispelable = true;
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        }),
                        Helpers.Create<ContextActionApplyWeaponEnchant>(a => {
                            a.Enchantments = new BlueprintItemEnchantmentReference[]{
                                enchant.ToReference<BlueprintItemEnchantmentReference>()
                            };
                            a.DurationValue = new ContextDurationValue() {
                                Rate = DurationRate.Rounds,
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = 1
                            };
                        })
                    };
                });
            });
            return EnergyAttunement;
        }

        private static BlueprintItemWeapon CreateBlackBlade(BlueprintItemWeapon baseWeapon, BlueprintWeaponEnchantment enchant) {
            var LexiconAssemble_BE = Resources.GetBlueprint<BlueprintDialog>("9df5b313d792a424392ae64647e36969");
            var BlackBlade = Helpers.CreateCopy(baseWeapon, bp => {
                bp.name = $"BlackBlade{baseWeapon.Category}";
                bp.AssetGuid = ModSettings.Blueprints.GetGUID(bp.name);
                bp.m_DisplayNameText = Helpers.CreateString($"{bp.name}.Name", "Black Blade");
                bp.m_DescriptionText = Helpers.CreateString($"{bp.name}.Description", "A black blade has special abilities (or imparts abilities to its wielder) " +
                    "depending on the wielder’s magus level. The abilities are cumulative. A black blade cannot be wielded by anyone other than its magus.");
                bp.m_Enchantments = new BlueprintWeaponEnchantmentReference[] { enchant.ToReference<BlueprintWeaponEnchantmentReference>() };
                bp.m_Destructible = false;
                bp.m_Weight = 0;
                bp.AddComponent<ItemDialog>(c => {
                    c.m_Conditions = new ConditionsChecker() {
                        Conditions = new Condition[0]
                    };
                    c.m_ItemName = new Kingmaker.Localization.LocalizedString();
                    c.m_DialogReference = LexiconAssemble_BE.ToReference<BlueprintDialogReference>();
                });
            });
            Resources.AddBlueprint(BlackBlade);
            return BlackBlade;
        }
    }
}
