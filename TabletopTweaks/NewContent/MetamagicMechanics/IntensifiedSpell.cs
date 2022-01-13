﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;
using TabletopTweaks.Extensions;
using TabletopTweaks.NewComponents;
using TabletopTweaks.Utilities;
using static TabletopTweaks.NewContent.MetamagicMechanics.MetamagicExtention;
using static TabletopTweaks.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TabletopTweaks.NewContent.MetamagicMechanics {
    static class IntensifiedSpell {
        public static void AddIntensifiedSpell() {
            var FavoriteMetamagicSelection = Resources.GetBlueprint<BlueprintFeatureSelection>("503fb196aa222b24cb6cfdc9a284e838");
            var Icon_IntensifiedSpellFeat = AssetLoader.LoadInternal("Feats", "Icon_IntensifiedSpellFeat.png");
            var Icon_IntensifiedSpellMetamagic = AssetLoader.LoadInternal("Metamagic", "Icon_IntensifiedSpellMetamagic.png", 128);

            var IntensifiedSpellFeat = Helpers.CreateBlueprint<BlueprintFeature>("IntensifiedSpellFeat", bp => {
                bp.SetName("Metamagic (Intensified Spell)");
                bp.SetDescription("Your spells can go beyond several normal limitations.\n" +
                    "An intensified spell increases the maximum number of damage dice by 5 levels. " +
                    "You must actually have sufficient caster levels to surpass the maximum in order " +
                    "to benefit from this feat. No other variables of the spell are affected, and spells " +
                    "that inflict damage that is not modified by caster level are not affected by this feat.\n" +
                    "Level Increase: +1 (an intensified spell uses up a spell slot one level higher than the spell’s actual level.)");
                bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.WizardFeat };
                bp.AddComponent<AddMetamagicFeat>(c => {
                    c.Metamagic = (Metamagic)CustomMetamagic.Intensified;
                });
                bp.AddComponent(Helpers.Create<FeatureTagsComponent>(c => {
                    c.FeatureTags = FeatureTag.Magic | FeatureTag.Metamagic;
                }));
            });

            var FavoriteMetamagicIntensified = Helpers.CreateBlueprint<BlueprintFeature>("FavoriteMetamagicIntensified", bp => {
                bp.SetName("Favorite Metamagic — Intensified");
                bp.m_Description = FavoriteMetamagicSelection.m_Description;
                //bp.m_Icon = Icon_IntensifiedSpellFeat;
                bp.Ranks = 1;
                bp.ReapplyOnLevelUp = true;
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] { };
                bp.AddComponent<AddCustomMechanicsFeature>(c => {
                    c.Feature = CustomMechanicsFeature.FavoriteMetamagicIntensified;
                });
                bp.AddPrerequisiteFeature(IntensifiedSpellFeat);
            });

            var spells = SpellTools.SpellList.AllSpellLists
                .Where(list => !list.IsMythic)
                .SelectMany(list => list.SpellsByLevel)
                .Where(spellList => spellList.SpellLevel != 0)
                .SelectMany(level => level.Spells)
                .Distinct()
                .OrderBy(spell => spell.Name)
                .ToArray();
            foreach (var spell in spells) {
                bool dealsDamage = spell.FlattenAllActions()
                    .OfType<ContextActionDealDamage>().Any(a => a.Value.DiceCountValue.ValueType == ContextValueType.Rank)
                    || (spell?.GetComponent<AbilityEffectStickyTouch>()?
                    .TouchDeliveryAbility?
                    .FlattenAllActions()?
                    .OfType<ContextActionDealDamage>()?
                    .Any(a => a.Value.DiceCountValue.ValueType == ContextValueType.Rank) ?? false)
                    || spell.GetComponent<AbilityShadowSpell>();
                if (dealsDamage) {
                    if (!spell.AvailableMetamagic.HasMetamagic((Metamagic)CustomMetamagic.Intensified)) {
                        spell.AvailableMetamagic |= (Metamagic)CustomMetamagic.Intensified;
                        Main.LogPatch("Enabled Intensified Metamagic", spell);
                    }
                };
            }
            MetamagicExtention.RegisterNewMetamagic(
                metamagic: CustomMetamagic.Intensified,
                name: "Intensified", 
                icon: Icon_IntensifiedSpellMetamagic, 
                defaultCost: 1,
                CustomMechanicsFeature.FavoriteMetamagicIntensified
            );
            //if (ModSettings.AddedContent.Feats.IsDisabled("QuickChannel")) { return; }
            FeatTools.AddAsFeat(IntensifiedSpellFeat);
            FavoriteMetamagicSelection.AddFeatures(FavoriteMetamagicIntensified);
        }
    }
}
