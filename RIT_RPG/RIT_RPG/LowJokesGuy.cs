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
using System.IO; //Using file input and output

namespace RIT_RPG
{
    class LowJokesGuy:Character //Handles files for the Low Jokes Guy
    {
        public LowJokesGuy(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string fileName):base(sb, gd, sprite, fileName) //Constructor
        {

        }

        public LowJokesGuy(Character ch):base(ch) //Cloning constructor
        {

        }

        public override void ExpGain(int expGained) //Handles exp gain
        {    
            if (level < 100) //While the character's level is less than 100
            {
                #region Handles exp gain, calls the LevelUp method if needed
                exp += expGained; //Calculate the new experience

                if (exp >= 100) //If the player has more than 100 experience points
                {
                    LevelUp(); //Call the LevelUp method
                }
                #endregion

                #region Write the new exp to the character file
                else //If the player did not level up
                {
                    StreamWriter fileWriter = new StreamWriter("lowjokesGuy.bin"); //Create the StreamWriter object
                    fileWriter.Write(name + ","); //Write the character's name
                    fileWriter.Write(characterClass + ","); //Write the character's class
                    fileWriter.Write(level + ","); //Add the character's level
                    fileWriter.Write(exp + ","); //Add the character's experience points
                    fileWriter.Write(ego + ","); //Add the character's ego stat
                    fileWriter.Write(humor + ","); //Add the character's humor stat
                    fileWriter.Write(intelligence + ","); //Add the character's intelligence stat
                    fileWriter.Write(energy + ","); //Add the character's energy stat
                    fileWriter.Write(wit); //Add the character's wit stat
                    fileWriter.Close(); //Close the StreamWriter
                }
                #endregion
            }  
        }

        public override void LevelUp() //Handles character levelup
        {
            #region Handles stat distribution and recalculates exp
            exp -= 100; //Calculate the new experience
            level += 1; //Give the character a new level
            ego += ((ego % 5) + 2); //Calculates the new ego stat
            humor += ((humor % 4) + 1); //Calculates the new humor stat
            intelligence += ((intelligence % 4) + 1); //Calculates the new intelligence stat
            energy += ((energy % 3) + 1); //Calculates the new energy stat
            wit += ((wit % 5) + 1); //Calculates the new wit stat
            #endregion

            #region Writes the stat changes to the character file
            StreamWriter fileWriter = new StreamWriter("lowjokesGuy.bin"); //Create the StreamWriter object
            fileWriter.Write(name + ","); //Write the character's name
            fileWriter.Write(characterClass + ","); //Write the character's class
            fileWriter.Write(level + ","); //Add the character's level
            fileWriter.Write(exp + ","); //Add the character's experience points
            fileWriter.Write(ego + ","); //Add the character's ego stat
            fileWriter.Write(humor + ","); //Add the character's humor stat
            fileWriter.Write(intelligence + ","); //Add the character's intelligence stat
            fileWriter.Write(energy + ","); //Add the character's energy stat
            fileWriter.Write(wit); //Add the character's wit stat
            fileWriter.Close(); //Close the StreamWriter
            #endregion
        }

        public override void Draw(Color colour, int pos) //Draws the character to the screen
        {
            Vector2 drawPosition = new Vector2(19 + (83 * pos), 161); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 128), colour); //Draw the character
        }

        public override void Draw() //Draws the character for game over
        {
            Vector2 drawPosition = new Vector2(268, 180); //Where to draw the character
            sBatch.Draw(characterSprite, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, 64, 128), Color.White); //Draw the character
        }
    }
}
