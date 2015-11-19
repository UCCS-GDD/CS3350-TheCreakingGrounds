using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Effects
{
    public class StatRestore: Effect
    {
        public PlayerStat Stat;
        public byte ModAmount;

        public override void OnAdd(Player player)
        {
            switch (Stat)
            {
                case PlayerStat.Brawn:
                    player.Brawn.RestoreStat(ModAmount);
                    break;
                case PlayerStat.Speed:
                    player.Speed.RestoreStat(ModAmount);
                    break;
                case PlayerStat.Intellect:
                    player.Intellect.RestoreStat(ModAmount);
                    break;
                case PlayerStat.Willpower:
                    player.Willpower.RestoreStat(ModAmount);
                    break;
                case PlayerStat.Wounds:
                    player.Wounds.RestoreStat(ModAmount);
                    break;
                case PlayerStat.Traumas:
                    player.Traumas.RestoreStat(ModAmount);
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

            return String.Format("Heal {0} {1}", ModAmount, Stat.ToString());
        }

        public override void OnUpdate(Player player)
        {
            return;
        }
    }
}
