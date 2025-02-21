﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Optimization;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TabletopTweaks.Core.NewComponents.OwlcatReplacements {
    [TypeId("39daf11ada364bbab00b4ff8a92dba1d")]
    public class AzataZippyMagicTTT : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCastSpell>,
        IRulebookHandler<RuleCastSpell>,
        ISubscriber,
        IInitiatorRulebookSubscriber {

        public void OnEventAboutToTrigger(RuleCastSpell evt) {
        }

        private bool isValidTrigger(RuleCastSpell evt) {
            return evt.Success
                && evt.Spell.Blueprint.IsSpell
                && !evt.IsDuplicateSpellApplied
                && !evt.Spell.IsAOE
                && !evt.Spell.Blueprint.GetComponents<AbilityEffectStickyTouch>().Any()
                && !evt.Spell.Blueprint.GetComponents<BlockSpellDuplicationComponent>().Any();
        }

        public void OnEventDidTrigger(RuleCastSpell evt) {
            if (!isValidTrigger(evt)) {
                return;
            }
            AbilityData spell = evt.Spell;
            UnitEntityData newTarget = this.GetNewTarget(spell, evt.SpellTarget.Unit);
            if (newTarget == null) {
                return;
            }
            Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(spell, newTarget) {
                IsDuplicateSpellApplied = true
            });
        }

        private UnitEntityData GetNewTarget(AbilityData data, UnitEntityData baseTarget) {
            List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(baseTarget.Position, m_FeetsRadius.Feet().Meters);
            list.Remove(baseTarget);
            list.Remove(base.Owner);
            list.RemoveAll((UnitEntityData x) => x.Faction != baseTarget.Faction || !data.CanTarget(x));
            if (list.Count <= 0) {
                return null;
            }
            return list.GetRandomElements(1, new System.Random())[0];
        }

        [SerializeField]
        private int m_FeetsRadius = 30;
    }
}
