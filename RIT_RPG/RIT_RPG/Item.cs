using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RIT_RPG
{
    // generic item, more specific variations will inherit from this
    abstract class Item
    {
        //attributes
        protected string name; // name of item
        protected string description; // explanation of what the item does
        protected string itemType; // can be either "Heal Item", "Damage Item", or "Revive Item"
        protected int uses; // "amount" of item the team as a whole have

        //properties
        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        public string ItemType
        {
            get { return itemType; }
        }

        public int Uses
        {
            get { return uses; }
            set { uses = value; }
        }

        //constructor
        public Item(string nm, string des, string typ, int amt)
        {
            name = nm;
            description = des;
            itemType = typ;
            uses = amt;
        }
    }

    abstract class HealthItem : Item
    {
        public HealthItem(string nm, string des, int amt)
            : base(nm, des, "Health Item", amt)
        {

        }

        public abstract void Use(Character ch);
    }

    abstract class DamageItem : Item
    {
        public DamageItem(string nm, string des, int amt)
            : base(nm, des, "Damage Item", amt)
        {

        }

        public abstract void Use(List<Character> enem);
    }

    abstract class ReviveItem : Item
    {
        public ReviveItem(string nm, string des, int amt)
            : base(nm, des, "Revive Item", amt)
        {

        }

        public abstract void Use(CharacterRows list);
    }
}
