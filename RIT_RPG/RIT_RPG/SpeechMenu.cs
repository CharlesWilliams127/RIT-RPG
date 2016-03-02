using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace RIT_RPG
{

    class SpeechMenu:SmallMenu
    {
        SpriteFont font;
        string text = null;
        DemoBattle db;
        DestlerBossBattle destBattle;
        FinalBossBattle fBB;
        Vector2 line1;
        Vector2 line2;

        public SpeechMenu(GraphicsDeviceManager gr, SpriteBatch sp, SpriteFont ft, Texture2D bak, Vector2 pos, int ht, int wd, DemoBattle d):base(gr,sp,bak,pos,ht,wd)
        {
            font = ft;
            db = d;
            line1 = new Vector2(position.X + 20, position.Y + 60);
            line2 = new Vector2(position.X + 20, position.Y + 90);
        }

        public SpeechMenu(GraphicsDeviceManager gr, SpriteBatch sp, SpriteFont ft, Texture2D bak, Vector2 pos, int ht, int wd, DestlerBossBattle dbb)
            : base(gr, sp, bak, pos, ht, wd)
        {
            font = ft;
            destBattle = dbb;
            line1 = new Vector2(position.X + 20, position.Y + 60);
            line2 = new Vector2(position.X + 20, position.Y + 90);
        }

        public SpeechMenu(GraphicsDeviceManager gr, SpriteBatch sp, SpriteFont ft, Texture2D bak, Vector2 pos, int ht, int wd, FinalBossBattle fb)
            : base(gr, sp, bak, pos, ht, wd)
        {
            font = ft;
            fBB = fb;
            line1 = new Vector2(position.X + 20, position.Y + 60);
            line2 = new Vector2(position.X + 20, position.Y + 90);
        }

        public void Talk(Character attacker) //If the character switches rows
        {
            base.DrawMenu(); //Draws the background

            if (attacker != null)
            {
                text = attacker.Name + " switched rows!"; // put the character's name and quote into one text string

                // this command may need to be improved upon to write multiple lines of the text attribute
                sprite.DrawString(font, text, line1, Color.White); // use the spritefont to write out the completed text
                //sprite.DrawString(font, attacker.Name + " is in row " + attacker.Row, new Vector2(10, 30), Color.Black); // use the spritefont to write out the completed text
            }
            else
            {
                return;
            }
        }

        public void Talk(Character attacker, Character defender) //Base speech
        {
            base.DrawMenu(); //Draws the background

            if (attacker != null && defender != null)
            {
                if (defender.IsDead == false) //If the character is not dead
                {
                    text = attacker.Name + ": " + attacker.AttackName; // put the character's name and quote into one text string

                    // this command may need to be improved upon to write multiple lines of the text attribute
                    sprite.DrawString(font, text, line1, Color.White); // use the spritefont to write out the completed text
                    if (defender is Destler || attacker is Destler) //If the defender is Destler
                    {
                        sprite.DrawString(font, defender.Name + "'s ego took " + (defender.PreviousEgoInBattle - defender.EgoInBattle) + " damage!", line2, Color.White); // use the spritefont to write out the completed text
                    }
                    else if (defender is TankCharacter || defender is FinalBoss || attacker is TankCharacter || attacker is FinalBoss) //If the defender is the tank or the final boss
                    {
                        sprite.DrawString(font, defender.Name + "'s ego took " + fBB.Damage + " damage!", line2, Color.White); // use the spritefont to write out the completed text
                    }
                    else //DemoBattle
                    {
                        sprite.DrawString(font, defender.Name + "'s ego took " + (defender.PreviousEgoInBattle - defender.EgoInBattle) + " damage!", line2, Color.White); // use the spritefont to write out the completed text
                    }
                }
                else if (defender.IsDead == true && defender.PreviousEgoInBattle > 0) //If the character just died
                {
                    text = attacker.Name + ": " + attacker.AttackName; // put the character's name and quote into one text string

                    // this command may need to be improved upon to write multiple lines of the text attribute
                    sprite.DrawString(font, text, line1, Color.White); // use the spritefont to write out the completed text
                    sprite.DrawString(font, defender.Name + " was defeated!", line2, Color.White); // use the spritefont to write out the completed text
                }
                else if (defender.IsDead == true && defender is TankCharacter) //If the character is the tank
                {
                    // this command may need to be improved upon to write multiple lines of the text attribute
                    sprite.DrawString(font, attacker.Name + " punched the tank!", line1, Color.White); // use the spritefont to write out the completed text
                    sprite.DrawString(font, attacker.Name + " broke his hand!", line2, Color.White); // use the spritefont to write out the completed text
                }
                else //If the character has been dead
                {
                    // this command may need to be improved upon to write multiple lines of the text attribute
                    sprite.DrawString(font, attacker.Name + " beats on the lifeless body of " + defender.Name + "!", line1, Color.White); // use the spritefont to write out the completed text
                }
            }
            else
            {
                return;
            }
        }

        public void Talk(Character attacker, string defender) //Special enemy case
        {
            base.DrawMenu(); //Draws the background

            if (attacker != null && defender != null)
            {
                text = attacker.Name + ": " + attacker.AttackName; // put the character's name and quote into one text string

                // this command may need to be improved upon to write multiple lines of the text attribute
                sprite.DrawString(font, text, line1, Color.White); // use the spritefont to write out the completed text
                if (defender == "The party")
                {
                    sprite.DrawString(font, defender + " has lowered stats!", line2, Color.White); // use the spritefont to write out the completed text
                }
                else if (defender == "All")
                {
                    sprite.DrawString(font, defender + " members of the party take damage!", line2, Color.White); // use the spritefont to write out the completed text
                }
                else
                {
                    sprite.DrawString(font, defender + " has buffed stats!", line2, Color.White); // use the spritefont to write out the completed text
                }
            }
            else
            {
                return;
            }
        }

        public void Talk(Character attacker, Item item) //When an item is used
        {
            base.DrawMenu(); //Draws the background

            if (attacker != null && item != null)
            {
                text = attacker.Name + " used " + item.Name + "!"; // put the character's name and quote into one text string

                // this command may need to be improved upon to write multiple lines of the text attribute
                sprite.DrawString(font, text, line1, Color.White); // use the spritefont to write out the completed text
                if (item is HealthItem)
                {
                    sprite.DrawString(font, attacker.Name + " feels restored.", line2, Color.White); // use the spritefont to write out the completed text
                }
                else if (item is DamageItem)
                {
                    sprite.DrawString(font, attacker.Name + " damages the enemies.", line2, Color.White); // use the spritefont to write out the completed text
                }
                else if (item is ReviveItem)
                {
                    sprite.DrawString(font, attacker.Name + " empowers the party.", line2, Color.White); // use the spritefont to write out the completed text
                }
            }
            else
            {
                return;
            }
        }
    }
}
