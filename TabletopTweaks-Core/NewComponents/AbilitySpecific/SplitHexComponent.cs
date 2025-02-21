﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Units;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;
using TabletopTweaks.Core.NewEvents;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.NewComponents.AbilitySpecific {
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("00d2d2c9def641d3844a98a5a95f238e")]
    public class SplitHexComponent : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        IAbilityGetCommandTypeHandler,
        ITickEachRound {

        private BlueprintAbility[] m_MajorHexes;
        private BlueprintAbility[] MajorHexes {
            get {
                if (m_MajorHexes == null) {
                    m_MajorHexes = this.m_MajorHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_MajorHexes;
            }
        }
        private BlueprintAbility[] m_GrandHexes;
        private BlueprintAbility[] GrandHexes {
            get {
                if (m_GrandHexes == null) {
                    m_GrandHexes = this.m_GrandHex?.Get()?.IsPrerequisiteFor
                        .Select(f => f.Get())
                        .SelectMany(c => c.GetComponents<AddFacts>())
                        .Where(c => c is not null)
                        .SelectMany(c => c.Facts)
                        .OfType<BlueprintAbility>()
                        .SelectMany(hex => hex.AbilityAndVariants())
                        .SelectMany(hex => hex.AbilityAndStickyTouch())
                        .Distinct()
                        .ToArray();
                }
                return m_GrandHexes;
            }
        }
        private BlueprintFeature SplitMajorHex => m_SplitMajorHex?.Get();

        public ReferenceArrayProxy<BlueprintFeature, BlueprintFeatureReference> Features {
            get {
                return this.m_MajorHex?.Get()?.IsPrerequisiteFor.ToArray();
            }
        }

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            var SplitHexPart = Owner.Ensure<UnitPartSplitHex>();
            if (!isValidTrigger(evt)) { return; }
            if (SplitHexPart.Data.HasStoredHex) {
                if (SplitHexPart.Data.StoredHex == evt.Spell.Blueprint) {
                    SplitHexPart.Data.Clear();
                }
            } else if (evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex) && !evt.Spell.IsAOE) {
                SplitHexPart.Data.Store(evt.Spell.Blueprint, evt.SpellTarget.Unit);
            }
        }

        public void HandleGetCommandType(AbilityData ability, ref UnitCommand.CommandType commandType) {
            var SplitHexPart = Owner.Ensure<UnitPartSplitHex>();
            if (SplitHexPart.Data.HasStoredHex && ability.Blueprint.AssetGuid == SplitHexPart.Data.StoredHex.AssetGuid) {
                commandType = UnitCommand.CommandType.Free;
            }
        }

        public void OnNewRound() {
            var SplitHexPart = Owner.Ensure<UnitPartSplitHex>();
            SplitHexPart.Data.Clear();
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && evt.Spell.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.Hex)
                && !evt.IsDuplicateSpellApplied
                && !evt.Spell.IsAOE
                && !GrandHexes.Any(hex => hex.AssetGuid == evt.Spell.Blueprint.AssetGuid)
                && (evt.Initiator.HasFact(SplitMajorHex) || !MajorHexes.Any(hex => hex.AssetGuid == evt.Spell.Blueprint.AssetGuid));
        }

        public BlueprintFeatureReference m_MajorHex;
        public BlueprintFeatureReference m_GrandHex;
        public BlueprintFeatureReference m_SplitMajorHex;
        public BlueprintBuffReference m_SplitHexCooldown;

        public class SplitHexData {
            private BlueprintAbilityReference m_StoredHex;

            public bool HasStoredHex => !m_StoredHex?.IsEmpty() ?? false;
            public BlueprintAbility StoredHex => m_StoredHex?.Get();


            public void Store(BlueprintAbility hex) {
                m_StoredHex = hex.ToReference<BlueprintAbilityReference>();
            }

            public void Clear() {
                m_StoredHex = null;
            }
        }
    }
}
