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
    class CodeTA:Character
    {
        public CodeTA(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, int nrg, int wi, int experienceGain):base(sb, gd, sprite, nm, eg, hmr, intell, nrg, wi, experienceGain) //Enemy stats
        {
            
        }

        // the TA's intelligence based attack
        public int ImSmarterThanYou()
        {
            int attack = rgen.Next(0, intelligenceInBattle); // uses a die roll with the TA's max intelligence as the limiting attribute
            attackName = "I'm smarter than you!";
            return attack; // this value will be passed into the DefendIntelligence method for whatever character is being attacked
        }

        public override void Draw(Color colour, int pos) //Draws the charater to the screen
        {
            Vector2 drawPosition = new Vector2(351 + (83 * pos), 171); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 128), colour); //Draw the character
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
