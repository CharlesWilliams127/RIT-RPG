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
    class Destler : Character
    {
        //Constructor
        public Destler(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, int nrg, int wi, int experienceGain):base(sb, gd, sprite, nm, eg, hmr, intell, nrg, wi, experienceGain)
        {

        }

        //Methods and attacks
        public int Superiority() //int based attacks
        {
            int attack = rgen.Next(10, intelligenceInBattle);
            attackName = "Superiority!";
            return attack;
        }

        public void TuneBanjo() //main healing move
        {
            int heal = rgen.Next(10, 51);
            attackName = "Tune Banjo!";
            egoInBattle += heal;
        }

        public void Tuition() //Boosts own stats
        {
            attackName = "Tuition!";
            energyInBattle += 10;
        }

        public int BanjoShreddin() //main attack
        {
            attackName = "Banjo Shreddin!";
            int attack = rgen.Next(5, humorInBattle);
            return attack;
        }

        public int MADdamage()  //special attack.
        {
            attackName = "MAD Damage!";
            if(egoInBattle <= egoInBattle / 2)
            {
                int attack = rgen.Next(10, 31);
                return attack;
            }
            else
            {
                int attack = 20;
                return attack;
            }
        }

        public override void Draw(Color colour, int pos)
        {
            Vector2 drawPosition = new Vector2(434, 171); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 128), colour); //Draw the character
        }

        #region Do Not Call
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
