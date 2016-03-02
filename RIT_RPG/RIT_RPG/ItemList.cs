using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RIT_RPG
{
    #region Health Items
    class CuppaJoe : HealthItem
    {
        public CuppaJoe(int amt)
            : base("Cuppa Joe", "Coffee from Java Wally's to boost your ego", amt)
        {

        }

        public override void Use(Character ch)
        {
            ch.EgoInBattle += 30;
            uses--;
        }
    }
    #endregion

    #region Damage Items
    class LogicBomb : DamageItem
    {
        public LogicBomb(int amt)
            : base("Logic Bomb", "Confuse your enemies and damage their ego", amt)
        {

        }

        public override void Use(List<Character> enem)
        {
            foreach (Character en in enem)
            {
                en.EgoInBattle -= 20;
            }
            uses--;
        }
    }
    #endregion

    #region Revive Items
    class SummonRitchie : ReviveItem
    {
        public SummonRitchie(int amt)
            : base("Summon RITchie", "Call upon your school mascot to empower your team!", amt)
        {

        }

        public override void Use(CharacterRows list)
        {
            Character curr = list.Head;

            while(curr != null)
            {
                curr.EgoInBattle += 20;
                curr.WitInBattle += 15;
                curr.HumorInBattle += 20;
                curr.IntelligenceInBattle += 15;
                curr = curr.Next;
            }
            uses--;
        }
    }
    #endregion
}
