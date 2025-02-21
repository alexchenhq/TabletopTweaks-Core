﻿using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;

namespace TabletopTweaks.Core.MechanicsChanges {

    public class AdditionalModifierDescriptors {
        public enum NaturalArmor : int {
            Bonus = ModifierDescriptor.NaturalArmor,
            Size = 1717,
            Stackable = 1718
        }
        public enum Dodge : int {
            Strength = 2121,
            Dexterity = 2122,
            Constitution = 2123,
            Intelligence = 2124,
            Wisdom = 2125,
            Charisma = 2126
        }
        public enum Untyped : int {
            Strength = 3121,
            Dexterity = 3122,
            Constitution = 3123,
            Intelligence = 3124,
            Wisdom = 3125,
            Charisma = 3126,
            WeaponTraining = 3127,
            WeaponFocus = 3128,
            WeaponFocusGreater = 3129,
            SpellFocus = 3130,
            SpellFocusGreater = 3131,
            SchoolMastery = 3132,
            VarisianTattoo = 3133,
            Monk = 3134,
            Age = 3135
        }
        public enum Enhancement : int {
            Weapon = 4121
        }

        private static class FilterAdjustments {
            private static readonly Func<ModifiableValue.Modifier, bool> FilterIsDodgeOriginal = ModifiableValueArmorClass.FilterIsDodge;
            private static readonly Func<ModifiableValue.Modifier, bool> FilterIsArmorOriginal = ModifiableValueArmorClass.FilterIsArmor;

            [PostPatchInitialize]
            static void Update_ModifiableValueArmorClass_FilterIsArmor() {
                Func<ModifiableValue.Modifier, bool> newFilterIsArmor = delegate (ModifiableValue.Modifier m) {
                    ModifierDescriptor modDescriptor = m.ModDescriptor;
                    return
                        FilterIsArmorOriginal(m)
                        || modDescriptor == ModifierDescriptor.NaturalArmorForm
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Bonus
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Size
                        || modDescriptor == (ModifierDescriptor)NaturalArmor.Stackable;
                };
                var FilterIsArmor = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsArmor");
                FilterIsArmor.SetValue(null, newFilterIsArmor);
            }

            [PostPatchInitialize]
            static void Update_ModifiableValueArmorClass_FilterIsDodge() {
                Func<ModifiableValue.Modifier, bool> newFilterIsDodge = delegate (ModifiableValue.Modifier m) {
                    ModifierDescriptor modDescriptor = m.ModDescriptor;
                    return
                        FilterIsDodgeOriginal(m)
                            || modDescriptor == (ModifierDescriptor)Dodge.Strength
                            || modDescriptor == (ModifierDescriptor)Dodge.Dexterity
                            || modDescriptor == (ModifierDescriptor)Dodge.Constitution
                            || modDescriptor == (ModifierDescriptor)Dodge.Intelligence
                            || modDescriptor == (ModifierDescriptor)Dodge.Wisdom
                            || modDescriptor == (ModifierDescriptor)Dodge.Charisma;
                };
                var FilterIsDodge = AccessTools.Field(typeof(ModifiableValueArmorClass), "FilterIsDodge");
                FilterIsDodge.SetValue(null, newFilterIsDodge);
            }
        }

        [PostPatchInitialize]
        static void Update_ModifierDescriptorComparer_SortedValues() {
            InsertAfter(NaturalArmor.Size, (ModifierDescriptor)NaturalArmor.Bonus);
            InsertBefore(NaturalArmor.Stackable, (ModifierDescriptor)NaturalArmor.Bonus);
            InsertBefore(Dodge.Strength, ModifierDescriptor.Dodge);
            InsertBefore(Dodge.Dexterity, ModifierDescriptor.Dodge);
            InsertBefore(Dodge.Constitution, ModifierDescriptor.Dodge);
            InsertBefore(Dodge.Intelligence, ModifierDescriptor.Dodge);
            InsertBefore(Dodge.Wisdom, ModifierDescriptor.Dodge);
            InsertBefore(Dodge.Charisma, ModifierDescriptor.Dodge);
            InsertBefore(Untyped.Age, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Strength, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Dexterity, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Constitution, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Intelligence, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Wisdom, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Charisma, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.Monk, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.WeaponTraining, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.WeaponFocus, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.WeaponFocusGreater, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.SpellFocus, ModifierDescriptor.UntypedStackable);
            InsertAfter(Untyped.SpellFocusGreater, ModifierDescriptor.UntypedStackable);
            InsertBefore(Enhancement.Weapon, ModifierDescriptor.Enhancement);

            void InsertBefore(Enum value, ModifierDescriptor before) {
                ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
                    .SortedValues.InsertBeforeElement((ModifierDescriptor)value, before);
            };
            void InsertAfter(Enum value, ModifierDescriptor after) {
                ModifierDescriptorComparer.SortedValues = ModifierDescriptorComparer
                    .SortedValues.InsertAfterElement((ModifierDescriptor)value, after);
            };
        }

        [HarmonyPatch(typeof(ModifierDescriptorHelper), "IsStackable", new[] { typeof(ModifierDescriptor) })]
        static class ModifierDescriptorHelper_IsStackable_Patch {

            static void Postfix(ref bool __result, ModifierDescriptor descriptor) {
                if (descriptor == (ModifierDescriptor)NaturalArmor.Stackable) {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(ModifierDescriptorComparer), "Compare", new Type[] { typeof(ModifierDescriptor), typeof(ModifierDescriptor) })]
        static class ModifierDescriptorComparer_Compare_Patch {
            static SortedDictionary<ModifierDescriptor, int> order;

            static bool Prefix(ModifierDescriptorComparer __instance, ModifierDescriptor x, ModifierDescriptor y, ref int __result) {
                if (IsTTTDescriptor(x) || IsTTTDescriptor(y)) {
                    if (order == null) {
                        order = new SortedDictionary<ModifierDescriptor, int>();
                        int i = 0;
                        for (i = 0; i < ModifierDescriptorComparer.SortedValues.Length; i++) {
                            order[ModifierDescriptorComparer.SortedValues[i]] = i;
                        }
                    }
                    __result = order.Get(x).CompareTo(order.Get(y));
                    return false;
                }
                return true;
            }

            private static bool IsTTTDescriptor(ModifierDescriptor desc) {
                return Enum.IsDefined(typeof(NaturalArmor), (int)desc)
                    || Enum.IsDefined(typeof(Dodge), (int)desc)
                    || Enum.IsDefined(typeof(Untyped), (int)desc)
                    || Enum.IsDefined(typeof(Enhancement), (int)desc);
            }
        }

        [HarmonyPatch(typeof(AbilityModifiersStrings), "GetName", new Type[] { typeof(ModifierDescriptor) })]
        static class AbilityModifierStrings_GetName_Patch {
            static void Postfix(AbilityModifiersStrings __instance, ModifierDescriptor descriptor, ref string __result) {
                if (__result != __instance.DefaultName) { return; }
                switch (descriptor) {
                    case (ModifierDescriptor)NaturalArmor.Bonus:
                        __result = "Natural armor bonus";
                        break;
                    case (ModifierDescriptor)NaturalArmor.Size:
                        __result = "Natural armor size";
                        break;
                    case (ModifierDescriptor)NaturalArmor.Stackable:
                        __result = "Natural armor stackable";
                        break;
                    case (ModifierDescriptor)Dodge.Strength:
                    case (ModifierDescriptor)Dodge.Dexterity:
                    case (ModifierDescriptor)Dodge.Constitution:
                    case (ModifierDescriptor)Dodge.Intelligence:
                    case (ModifierDescriptor)Dodge.Wisdom:
                    case (ModifierDescriptor)Dodge.Charisma:
                        __result = "Dodge";
                        break;
                    case (ModifierDescriptor)Enhancement.Weapon:
                        __result = "Enhancement";
                        break;
                    case (ModifierDescriptor)Untyped.Strength:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Strength;
                        break;
                    case (ModifierDescriptor)Untyped.Dexterity:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Dexterity;
                        break;
                    case (ModifierDescriptor)Untyped.Constitution:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Constitution;
                        break;
                    case (ModifierDescriptor)Untyped.Intelligence:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Intelegence;
                        break;
                    case (ModifierDescriptor)Untyped.Wisdom:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Wisdom;
                        break;
                    case (ModifierDescriptor)Untyped.Charisma:
                        __result = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.CharacterSheet.Charisma;
                        break;
                    case (ModifierDescriptor)Untyped.Age:
                        __result = "Age";
                        break;
                    case (ModifierDescriptor)Untyped.WeaponTraining:
                    case (ModifierDescriptor)Untyped.WeaponFocus:
                    case (ModifierDescriptor)Untyped.WeaponFocusGreater:
                    case (ModifierDescriptor)Untyped.SpellFocus:
                    case (ModifierDescriptor)Untyped.SpellFocusGreater:
                    case (ModifierDescriptor)Untyped.SchoolMastery:
                    case (ModifierDescriptor)Untyped.VarisianTattoo:
                    case (ModifierDescriptor)Untyped.Monk:
                        __result = "Other";
                        break;
                }
            }
        }
    }
}
