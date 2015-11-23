using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    /// <summary>
    /// A wrapper class to handle base stats, current stats, penalties and bonuses
    /// </summary>
    public struct Stat : IEquatable<Stat>, IEquatable<int>
    {
        public sbyte baseVal;
        private float damage;
        private sbyte modifiers;

        /// <summary>
        /// The current modified value, including all bonuses, penalties and damage
        /// </summary>
        public short CurrentValue
        {
            get
            {
                return (short)Mathf.Clamp(baseVal - damage + modifiers, short.MinValue, short.MaxValue);
            }
        }

        /// <summary>
        /// The base/starting value, before any damage/bonuses/penalties
        /// </summary>
        public short BaseValue
        {
            get
            {
                return baseVal;
            }
        }

        /// <summary>
        /// The absolute max value that this stat can have, base value plus any modifier
        /// </summary>
        public short MaxValue
        {
            get
            {
                return (short)(baseVal + modifiers);
            }
        }

        /// <summary>
        /// How much damage this stat has already taken
        /// </summary>
        public float Damage
        {
            get
            {
                return damage;
            }
        }

        /// <summary>
        /// The cumulative bonuses and penalties applied to this stat
        /// </summary>
        public short Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        public Stat(sbyte baseVal, float damage = 0, sbyte modifiers = 0)
        {
            this.baseVal = baseVal;
            this.damage = damage;
            this.modifiers = modifiers;
        }

        /// <summary>
        /// Subtracts restoreAmount from this stat's damage, clamping to 0
        /// </summary>
        /// <param name="restoreAmount">The amount of damage to restore</param>
        public void RestoreStat(float restoreAmount)
        {
            damage = (float)Mathf.Clamp(damage - restoreAmount, 0, float.MaxValue);
        }

        /// <summary>
        /// Adds damageAmount damage to the stat, clamping to prevent overflow
        /// </summary>
        /// <param name="damageAmount">The amount of damage to add</param>
        public void DamageStat(float damageAmount)
        {
            damage = (float)Mathf.Clamp(damage + damageAmount, byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// Applies a semi-permanent modifier (modAmount) to a stat
        /// </summary>
        /// <param name="modAmount">The amount to modify by</param>
        public void ApplyModifier(sbyte modAmount)
        {
            modifiers = (sbyte)Mathf.Clamp(modifiers + modAmount, sbyte.MinValue, sbyte.MaxValue);
        }

        public static Stat Parse(string data)
        {
            string[] values = data.Split('_');

            return new Stat(sbyte.Parse(values[0]), float.Parse(values[1]), sbyte.Parse(values[2]));
        }

        public string ToDataString()
        {
            return string.Format("{0}_{1}_{2}", baseVal, damage, modifiers);
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Stat return false.
            if (!(obj is Stat))
            {
                return false;
            }

            // Return true if the fields match:
            return (CurrentValue == ((Stat)obj).CurrentValue);
        }

        public override int GetHashCode()
        {
            return baseVal;
        }

        public override string ToString()
        {
            return CurrentValue.ToString();
        }

        public bool Equals(Stat other)
        {
            return this.CurrentValue == other.CurrentValue;
        }

        public static bool operator ==(Stat c1, Stat c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Stat c1, Stat c2)
        {
            return !c1.Equals(c2);
        }

        public static bool operator >(Stat c1, Stat c2)
        {
            return c1.CurrentValue > c2.CurrentValue;
        }

        public static bool operator <(Stat c1, Stat c2)
        {
            return c1.CurrentValue < c2.CurrentValue;
        }

        public bool Equals(int other)
        {
            return this.CurrentValue == other;
        }

        public static bool operator ==(Stat c1, int c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Stat c1, int c2)
        {
            return !c1.Equals(c2);
        }

        public static bool operator >(Stat c1, int c2)
        {
            return c1.CurrentValue > c2;
        }

        public static bool operator <(Stat c1, int c2)
        {
            return c1.CurrentValue < c2;
        }

        public static bool operator >=(Stat c1, int c2)
        {
            return !(c1.CurrentValue < c2);
        }

        public static bool operator <=(Stat c1, int c2)
        {
            return !(c1.CurrentValue > c2);
        }
    }
}
