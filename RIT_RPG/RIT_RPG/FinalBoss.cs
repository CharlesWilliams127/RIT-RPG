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
    class FinalBoss : Character
    {
        //Constructor
        public FinalBoss(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, int nrg, int wi, int experienceGain)
                       :base(sb, gd, sprite, nm, eg, hmr, intell, nrg, wi, experienceGain)
        {

        }

        //Methods and attacks
        public int ComplaintsPaper() //intelligence attack
        {
            int attack = rgen.Next(0, intelligenceInBattle);
            attackName = "I've written a 28 page paper on why you suck!";
            return attack;
        }

        public void FourMilestones(CoffeeGuy cg, LowJokesGuy ljk, SmartassGuy sg, WittyGuy wg) //Lowers stats
        {
            attackName = "Now do the 4 milestones.";
            cg.WitInBattle -= cg.WitInBattle / 5;
            ljk.WitInBattle -= ljk.WitInBattle / 5;
            sg.WitInBattle -= sg.WitInBattle / 5;
            wg.WitInBattle -= wg.WitInBattle / 5; 
        }

        public int BreakYourGame() //unblockable hit party
        {
            attackName = "I just broke your game!";
            int damage = rgen.Next(10, humorInBattle);
            return damage;
        }

        public void LaughAtPain() //healing move
        {
            int heal = rgen.Next(10, 51);
            attackName = "I laugh at your struggles.";
            egoInBattle += heal;
        }
        public void SinceDayOne() //boost move
        {
            attackName = "I've been here since day 1!";
            intelligenceInBattle += 10;
        }

        public override void Draw(Color colour, int pos)
        {
            Vector2 drawPosition = new Vector2(517, 171); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 128), colour); //Draw the character
        }

        #region Do Not Call
        public override void ExpGain(int expGained)
        {
            throw new NotImplementedException();
        }

        public override void LevelUp()
        {
            throw new NotImplementedException();
        }

        

        public override void Draw()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
