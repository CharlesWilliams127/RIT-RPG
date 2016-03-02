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
    class CodeMonkey:Character
    {
        public CodeMonkey(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, int nrg, int wi, int experienceGain):base(sb, gd, sprite, nm, eg, hmr, intell, nrg, wi, experienceGain) //Enemy stats
        {
            
        }

        public int ThrowPoop()
        {
            int damage = rgen.Next(20, humorInBattle); // this attack does less damage overall but it ignores the defenders chance to reduce damage
            attackName = "Throw poop!";
            return damage; // this value is passed into the TakeDamage method
        }

        // temporarily distracts players, lowering their wit
        public void DoADance(CoffeeGuy cg, LowJokesGuy ljk, SmartassGuy sg, WittyGuy wg)
        {
            attackName = "Do a dance!";
            cg.WitInBattle -= cg.WitInBattle / 5;
            ljk.WitInBattle -= ljk.WitInBattle / 5;
            sg.WitInBattle -= sg.WitInBattle / 5;
            wg.WitInBattle -= wg.WitInBattle / 5; 
        }

        public override void Draw(Color colour, int pos) //Draws the charater to the screen
        {
            Vector2 drawPosition = new Vector2(351 + (83 * pos), 235); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 64), colour); //Draw the character
        }

        #region DO NOT CALL
        public override void Draw() //Never call this
        {

        }

        public override void ExpGain(int expGained) //Never call this
        {

        }

        public override void LevelUp() //Never call this
        {

        }
        #endregion
    }
}
