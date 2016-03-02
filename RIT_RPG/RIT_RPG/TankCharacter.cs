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
    class TankCharacter : Character
    {
        //constructor
        public TankCharacter(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, 
                              int nrg, int wi, int experienceGain) : base(sb, gd, sprite, nm, eg, hmr, intell, nrg, wi, experienceGain)
        {

        }

        //methods and attacks
        public int ConserveBullets() //main attack other than the normal attack
        {
            int attack = rgen.Next(10, humorInBattle) + 20;
            attackName = "I don't need to waste ammo on you!";
            return attack;
        }

        public int Kaboom() //hits All rows
        {
            attackName = "Kaboom!";
            int attack = 35;
            return attack;
        }

        public void ImInATank() //boosts own stats
        {
            attackName = "I'm in a friggin' tank!";
            witInBattle += 10;
        }

        public int PlayChicken() //int based attack
        {
            int attack = rgen.Next(5, intelligenceInBattle) + 35;
            attackName = "I'll run you over!";
            return attack;
        }
        public override void Draw(Color colour, int pos)
        {
            Vector2 drawPosition = new Vector2(372, 171); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 128, 128), colour); //Draw the character
        }

        #region Do NOT USE
        public override void ExpGain(int expGained)
        {
            
        }

        public override void LevelUp()
        {
            
        }

     

        public override void Draw()
        {

        }
        #endregion
    }
}
