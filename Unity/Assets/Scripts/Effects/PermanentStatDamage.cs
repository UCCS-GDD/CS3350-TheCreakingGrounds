using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Effects
{
    public class PermanentStatDamage : Effect
    {
        public PlayerStat Stat;
        public sbyte ModAmount;

        public string DescriptionOverride;

        public override void OnAdd(Player player)
        {
            switch (Stat)
            {
                case PlayerStat.Brawn:
                    player.Brawn.DamageStat(ModAmount);
                    break;
                case PlayerStat.Speed:
                    player.Speed.DamageStat(ModAmount);
                    break;
                case PlayerStat.Intellect:
                    player.Intellect.DamageStat(ModAmount);
                    break;
                case PlayerStat.Willpower:
                    player.Willpower.DamageStat(ModAmount);
                    break;
                case PlayerStat.Wounds:
                    player.Wounds.DamageStat(ModAmount);
                    break;
                case PlayerStat.Traumas:
                    player.Traumas.DamageStat(ModAmount);
                    break;
                default:
                    break;
            }
        }

        public override void OnRemove(Player player)
        {
            return;
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
