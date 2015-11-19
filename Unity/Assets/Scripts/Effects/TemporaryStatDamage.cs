using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Effects
{
    public class TemporaryStatDamage : Effect
    {
        public PlayerStat Stat;
        public byte ModAmount;

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
            Debug.Assert(Duration > 0);

            if (DescriptionOverride != null && DescriptionOverride != "")
                return DescriptionOverride;

            return String.Format("-{0} {1} for {2} seconds", ModAmount, Stat.ToString(), Duration);
        }

        public override void OnUpdate(Player player)
        {
            return;
        }
    }
}
