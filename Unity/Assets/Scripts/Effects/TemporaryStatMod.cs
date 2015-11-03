using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Effects
{
    public class TemporaryStatMod : Effect
    {
        public PlayerStat Stat;
        public sbyte ModAmount;

        public string DescriptionOverride;

        public override void OnAdd(Player player)
        {
            switch (Stat)
            {
                case PlayerStat.Brawn:
                    player.Brawn.ApplyModifier(ModAmount);
                    break;
                case PlayerStat.Speed:
                    player.Speed.ApplyModifier(ModAmount);
                    break;
                case PlayerStat.Intellect:
                    player.Intellect.ApplyModifier(ModAmount);
                    break;
                case PlayerStat.Willpower:
                    player.Willpower.ApplyModifier(ModAmount);
                    break;
                case PlayerStat.Wounds:
                    player.Wounds.ApplyModifier(ModAmount);
                    break;
                case PlayerStat.Traumas:
                    player.Traumas.ApplyModifier(ModAmount);
                    break;
                default:
                    break;
            }
        }

        public override void OnRemove(Player player)
        {
            switch (Stat)
            {
                case PlayerStat.Brawn:
                    player.Brawn.ApplyModifier((sbyte)-ModAmount);
                    break;
                case PlayerStat.Speed:
                    player.Speed.ApplyModifier((sbyte)-ModAmount);
                    break;
                case PlayerStat.Intellect:
                    player.Intellect.ApplyModifier((sbyte)-ModAmount);
                    break;
                case PlayerStat.Willpower:
                    player.Willpower.ApplyModifier((sbyte)-ModAmount);
                    break;
                case PlayerStat.Wounds:
                    player.Wounds.ApplyModifier((sbyte)-ModAmount);
                    break;
                case PlayerStat.Traumas:
                    player.Traumas.ApplyModifier((sbyte)-ModAmount);
                    break;
                default:
                    break;
            }
        }

        public override string EffectDescription()
        {
            if (DescriptionOverride != null && DescriptionOverride != "")
                return DescriptionOverride;

            if (ModAmount >= 0)
                return String.Format("+{0} {1}", ModAmount, Stat.ToString());
            else
                return String.Format("{0} {1}", ModAmount, Stat.ToString());
        }
    }
}
