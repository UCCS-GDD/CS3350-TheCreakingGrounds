using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Perks
{
    public class StatMod : Perk
    {
        public sbyte BrawnMod;
        public sbyte SpeedMod;
        public sbyte IntellectMod;
        public sbyte WillpowerMod;
        public sbyte WoundCapacityMod;
        public sbyte TraumaCapacityMod;

        public override void OnAdd(Player player)
        {
            player.Brawn.ApplyModifier(BrawnMod);
            player.Speed.ApplyModifier(SpeedMod);
            player.Intellect.ApplyModifier(IntellectMod);
            player.Willpower.ApplyModifier(WillpowerMod);
            player.Wounds.ApplyModifier(WoundCapacityMod);
            player.Traumas.ApplyModifier(TraumaCapacityMod);
        }

        public override void OnRemove(Player player)
        {
            player.Brawn.ApplyModifier((sbyte)-BrawnMod);
            player.Speed.ApplyModifier((sbyte)-SpeedMod);
            player.Intellect.ApplyModifier((sbyte)-IntellectMod);
            player.Willpower.ApplyModifier((sbyte)-WillpowerMod);
            player.Wounds.ApplyModifier((sbyte)-WoundCapacityMod);
            player.Traumas.ApplyModifier((sbyte)-TraumaCapacityMod);
        }
    }
}
