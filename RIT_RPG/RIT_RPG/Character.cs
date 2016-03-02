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
    abstract class Character //Handles reading of character files
    {
        #region Attributes
        #region Character Base Stats
        protected string name; //Character name
        protected string characterClass; //Character class
        protected int level; //Character level
        protected int exp; //Character experience points
        protected int ego; //Ego (health) stat
        protected int humor; //Humor (attack) stat
        protected int intelligence; //Intelligence (defense) stat
        protected int energy; //Energy (speed) stat
        protected int wit; //Wit (buff) stat
        #endregion

        #region Character Stats in Battle
        protected int previousEgoInBattle; //Ego of the character before damage is taken
        protected int egoInBattle; //Ego stat when the player is in battle
        protected int humorInBattle; //Humor stat when the player is in battle
        protected int intelligenceInBattle; //Intelligence when the player is in battle
        protected int energyInBattle; //Energy when the player is in battle
        protected int witInBattle; //Wit when the player is in battle
        #endregion

        #region Experience Given From Enemies
        protected int experienceFromEnemy; //Experience gained when defeating an enemy
        #endregion

        #region Attack File Names
        protected string attackFileName1; //A string containing the first filename
        protected string attackFileName2; //A string containing the second filename
        protected string attackFileName3; //A string containing the third filename
        protected string attackFileName4; //A string containing the fourth filename
        #endregion

        #region Attack Strings and Base Damage
        protected string attackString1; //A string containing the first insult
        protected string attackString2; //A string containing the second insult
        protected string attackString3; //A string containing the third insult
        protected string attackString4; //A string containing the fourth insult
        protected string attackName; //The attack name for some displaying
        protected int attackBaseDamage; //The base damage an attack does
        #endregion

        #region RNG and Drawing
        protected Random rgen; //A random number generator for rolling dice
        protected GraphicsDeviceManager gdm; //The GraphicsDeviceManager
        protected SpriteBatch sBatch; //The SpriteBatch
        protected Texture2D characterSprite; //The sprite of the character
        #endregion

        #region Character States
        protected int row; //What row the character or enemy is in
        protected bool isDead; //A boolean value to see if the character is dead
        protected bool isActive; //If the character is currently active
        protected bool hasAttacked; //If the character has taken their turn to attack
        protected bool isBeingAttacked; //The character being attcked
        protected bool isTalking; //The character's text talking
        protected bool usedItem;
        protected Character next; //The characters are now nodes in a linked list
        #endregion
        #endregion

        /// <summary>
        /// Player Charcter
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gd"></param>
        /// <param name="sprite"></param>
        /// <param name="fileName"></param>
        public Character(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string fileName) //Constructor for characters
        {
            #region Try to read in the character file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(fileName); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[9]; //An array to hold the nine pieces of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split(','); //Tells the array where each piece of data ends
                    name = characterData[0]; //Give the character the name specified in the file
                    characterClass = characterData[1]; //Give the character the class specified in the file
                    level = int.Parse(characterData[2]); //Give the character the level specified in the file
                    exp = int.Parse(characterData[3]); //Give the character the amount of experience specified in the file
                    ego = int.Parse(characterData[4]); //Give the character the ego stat specified in the file
                    humor = int.Parse(characterData[5]); //Give the character the humor stat specified in the file
                    intelligence = int.Parse(characterData[6]); //Give the character the intelligence stat specified in the file
                    energy = int.Parse(characterData[7]); //Give the character the energy stat specified in the file
                    wit = int.Parse(characterData[8]); //Give the character the wit stat specified in the file
                }
                fileReader.Close(); //Closes the fileReader
            }
            #endregion

            #region If an exception is thrown, create a new character file
            catch (Exception) //If the file cannot be read, default
            {
                StreamWriter fileWriter = null; //Create the StreamWriter object

                if (fileName == "smartassGuy.bin" || fileName == "smartassGuyDefault.bin") //If the file was for the Smartass Guy
                {
                    name = "Smartass Guy"; //Gives the character a default name
                    characterClass = "Smartass Guy"; //Sets the character to the default class
                    level = 1; //Sets the character to the default level
                    exp = 0; //Gives the character the default amount of experience
                    ego = 103; //Gives the character the default ego stat
                    humor = 5; //Gives the character the default humor stat
                    intelligence = 15; //Gives the character the default intelligence stat
                    energy = 10; //Gives the character the default energy stat
                    wit = 10; //Gives the character the default wit stat

                    fileWriter = new StreamWriter(fileName); //Filename, to create the character files
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
                if (fileName == "lowjokesGuy.bin" || fileName == "lowjokesGuyDefault.bin") //If the file was for the low jokes Guy
                {
                    name = "Low Jokes Guy"; //Gives the character a default name
                    characterClass = "Low Jokes Guy"; //Sets the character to the default class
                    level = 1; //Sets the character to the default level
                    exp = 0; //Gives the character the default amount of experience
                    ego = 100; //Gives the character the default ego stat
                    humor = 15; //Gives the character the default humor stat
                    intelligence = 5; //Gives the character the default intelligence stat
                    energy = 13; //Gives the character the default energy stat
                    wit = 10; //Gives the character the default wit stat

                    fileWriter = new StreamWriter(fileName); //Filename, to create the character files
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
                if (fileName == "wittyGuy.bin" || fileName == "wittyGuyDefault.bin") //If the file was for the Witty Guy
                {
                    name = "Witty Guy"; //Gives the character a default name
                    characterClass = "Witty Guy"; //Sets the character to the default class
                    level = 1; //Sets the character to the default level
                    exp = 0; //Gives the character the default amount of experience
                    ego = 100; //Gives the character the default ego stat
                    humor = 13; //Gives the character the default humor stat
                    intelligence = 10; //Gives the character the default intelligence stat
                    energy = 5; //Gives the character the default energy stat
                    wit = 15; //Gives the character the default wit stat

                    fileWriter = new StreamWriter(fileName); //Filename, to create the character files
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
                if (fileName == "coffeeGuy.bin" || fileName == "coffeeGuyDefault.bin") //If the file was for the Coffee Guy
                {
                    name = "Coffee Guy"; //Gives the character a default name
                    characterClass = "Coffee Guy"; //Sets the character to the default class
                    level = 1; //Sets the character to the default level
                    exp = 0; //Gives the character the default amount of experience
                    ego = 105; //Gives the character the default ego stat
                    humor = 5; //Gives the character the default humor stat
                    intelligence = 10; //Gives the character the default intelligence stat
                    energy = 13; //Gives the character the default energy stat
                    wit = 10; //Gives the character the default wit stat

                    fileWriter = new StreamWriter(fileName); //Filename, to create the character files
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
            }
            #endregion

            #region Set the drawing attributes and in battle stats
            sBatch = sb; //Sets the Spritebatch
            gdm = gd; //Sets the GraphicsDeviceManager
            characterSprite = sprite; //Set the sprite for the character
            egoInBattle = ego; //Set the ego for use in battles
            humorInBattle = humor; //Set the humor for use in battles
            intelligenceInBattle = intelligence; //Set the intelligence for use in battles
            energyInBattle = energy; //Set the energy for use in battles
            witInBattle = wit; //Set the wit for use in battles
            rgen = new Random(); //Creates a random number generator
            isDead = false;
            isTalking = false;
            usedItem = false;
            #endregion
        }
        /// <summary>
        /// Enemy Character
        /// </summary>
        /// <param name="sb">Sprite batch</param>
        /// <param name="gd">Graphics Device</param>
        /// <param name="sprite">Texture fo Sprite</param>
        /// <param name="nm">name</param>
        /// <param name="eg">Ego</param>
        /// <param name="hmr">humor</param>
        /// <param name="intell">intelligence</param>
        /// <param name="nrg">energy</param>
        /// <param name="wi">wit</param>
        /// <param name="experienceGain">EXP after defeat</param>
        public Character(SpriteBatch sb, GraphicsDeviceManager gd, Texture2D sprite, string nm, int eg, int hmr, int intell, int nrg, int wi, int experienceGain) //Constructor for enemies
        {
            #region Set the drawing attributes and in battle stats
            sBatch = sb; //Sets the Spritebatch
            gdm = gd; //Sets the GraphicsDeviceManager
            characterSprite = sprite; //Set the sprite for the character
            name = nm; //Set the enemy name
            egoInBattle = eg; //Set the enemy ego
            humorInBattle = hmr; //Set the enemy humor
            intelligenceInBattle = intell; //Set the enemy intelligence
            energyInBattle = nrg; //Set the enemy energy
            witInBattle = wi; //Set the enemy wit
            experienceFromEnemy = experienceGain; //Set the amount of experience the enemy will give out when defeated
            rgen = new Random(); //Creates a random number generator
            isDead = false;
            isTalking = false;
            #endregion
        }

        public Character(Character ch) //Overload for cloning a previous character
        {
            #region Character Base Stats
            name = ch.Name;
            characterClass = ch.CharacterClass;
            level = ch.Level;
            exp = ch.Exp;
            ego = ch.Ego;
            humor = ch.Humor;
            intelligence = ch.Intelligence;
            energy = ch.Energy;
            wit = ch.Wit;
            #endregion

            #region Character Stats in Battle
            egoInBattle = ch.EgoInBattle;
            humorInBattle = ch.HumorInBattle;
            intelligenceInBattle = ch.IntelligenceInBattle;
            witInBattle = ch.WitInBattle;
            energyInBattle = ch.EnergyInBattle;
            #endregion 

            #region Attack File Names
            attackFileName1 = ch.AttackFileName1;
            attackFileName2 = ch.AttackFileName2;
            attackFileName3 = ch.AttackFileName3;
            attackFileName4 = ch.AttackFileName4;
            #endregion

            #region Attack Strings and Base Damage
            attackString1 = ch.AttackString1;
            attackString2 = ch.AttackString2;
            attackString3 = ch.AttackString3;
            attackString4 = ch.AttackString4;
            attackBaseDamage = ch.AttackBaseDamage;
            #endregion

            #region Character Sprite
            characterSprite = ch.characterSprite;
            #endregion

            #region RNG and Drawing
            rgen = new Random();
            gdm = ch.Gdm;
            sBatch = ch.SBatch;
            #endregion

            #region Character States
            row = ch.Row;
            isDead = ch.IsDead;
            isActive = false;
            hasAttacked = false;
            next = null;
            isTalking = false;
            usedItem = false;
            #endregion
        }

        #region Properties
        public string Name //Name property
        {
            get { return name; }
        }
        public string CharacterClass //CharacterClass property
        {
            get { return characterClass; }
        }
        public int Level //Level property
        {
            get { return level; }
            set { level = value; }
        }
        public int Exp //Exp property
        {
            get { return exp; }
            set { exp = value; }
        }
        public int Ego //Ego property
        {
            get { return ego; }
            set { ego = value; }
        }
        public int Humor //Humor property
        {
            get { return humor; }
            set { humor = value; }
        }
        public int Intelligence //Intelligence property
        {
            get { return intelligence; }
            set { intelligence = value; }
        }
        public int Energy //Energy property
        {
            get { return energy; }
            set { energy = value; }
        }
        public int Wit //Wit property
        {
            get { return wit; }
            set { wit = value; }
        }
        public int PreviousEgoInBattle //PreviousEgoInBattle property
        {
            get { return previousEgoInBattle; }
            set { previousEgoInBattle = value; }
        }
        public int EgoInBattle //EgoInBattle property
        {
            get { return egoInBattle; }
            set { egoInBattle = value; }
        }
        public int HumorInBattle //HumorInBattle property
        {
            get { return humorInBattle; }
            set { humorInBattle = value; }
        }
        public int IntelligenceInBattle //IntelligenceInBattle property
        {
            get { return intelligenceInBattle; }
            set { intelligenceInBattle = value; }
        }
        public int EnergyInBattle //EnergyInBattle property
        {
            get { return energyInBattle; }
            set { energyInBattle = value; }
        }
        public int WitInBattle //WitInBattle property
        {
            get { return witInBattle; }
            set { witInBattle = value; }
        }
        public int ExperienceFromEnemy //ExperienceFromEnemy property
        {
            get { return experienceFromEnemy; }
            set { experienceFromEnemy = value; }
        }
        public string AttackFileName1 //AttackFileName1 property
        {
            get { return attackFileName1; }
            set { attackFileName1 = value; }
        }
        public string AttackFileName2 //AttackFileName2 property
        {
            get { return attackFileName2; }
            set { attackFileName2 = value; }
        }
        public string AttackFileName3 //AttackFileName3 property
        {
            get { return attackFileName3; }
            set { attackFileName3 = value; }
        }
        public string AttackFileName4 //AttackFileName4 property
        {
            get { return attackFileName4; }
            set { attackFileName1 = value; }
        }
        public string AttackString1 //AttackString1 property
        {
            get { return attackString1; }
            set { attackString1 = value; }
        }
        public string AttackString2 //AttackString2 property
        {
            get { return attackString2; }
            set { attackString2 = value; }
        }
        public string AttackString3 //AttackString3 property
        {
            get { return attackString3; }
            set { attackString3 = value; }
        }
        public string AttackString4 //AttackString4 property
        {
            get { return attackString4; }
            set { attackString4 = value; }
        }
        public string AttackName //AttackName property
        {
            get { return attackName; }
            set { attackName = value; }
        }
        public int AttackBaseDamage //AttackBaseDamage property
        {
            get { return attackBaseDamage; }
            set { attackBaseDamage = value; }
        }
        public Texture2D CharacterSprite //CharacterSprite property
        {
            get { return characterSprite; }
            set { characterSprite = value; }
        }
        public int Row //Row property
        {
            get { return row; }
            set { row = value; }
        }
        public bool IsDead //IsDead property
        {
            get { return isDead; }
            set { isDead = value; }
        }
        public bool IsActive //IsActive property
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public bool IsTalking //IsTalking property
        {
            get { return isTalking; }
            set { isTalking = value; }
        }
        public bool HasAttacked //HasAttacked property
        {
            get { return hasAttacked; }
            set { hasAttacked = value; }
        }
        public bool IsBeingAttacked //IsBeingAttacked property
        {
            get { return isBeingAttacked; }
            set { isBeingAttacked = value; }
        }

        public bool UsedItem //UsedItem property
        {
            get { return usedItem; }
            set { usedItem = value; }
        }
        public GraphicsDeviceManager Gdm //Gdm property
        {
            get { return gdm; }
            set { gdm = value; }
        }
        public SpriteBatch SBatch //SBatch property
        {
            get { return sBatch; }
            set { sBatch = value; }
        }
        public Character Next //Next property
        {
            get { return next; }
            set { next = value; }
        }
        #endregion

        #region Abstract Methods
        public abstract void ExpGain(int expGained); //Handles exp gain

        public abstract void LevelUp(); //Handles character levelup
        #endregion

        public void TakeDamage(int damageTaken) //Updates the character's health in battle
        {
            previousEgoInBattle = egoInBattle; //Set the previous ego
            egoInBattle -= damageTaken; //Give the player their current ego
        }

        public void BuffStats() //Controls stat buffs
        {
            humorInBattle += energyInBattle; //Calculate the humor stat buff
            intelligenceInBattle += energyInBattle; //Calculate the intelligence stat buff
            witInBattle += energyInBattle ; //Calculate the wit stat buff
        }

        public int Attack(string attackFile) //Takes an attack file from the Insults folder
        {
            #region Try to read the attack file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(attackFile); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[2]; //An array to hold the two pieces of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split('`'); //Tells the array where each piece of data ends
                    attackName = characterData[0]; //Read in the attack name
                    attackBaseDamage = int.Parse(characterData[1]); //Give the character the level specified in the file
                }
                fileReader.Close(); //Closes the fileReader

                return (attackBaseDamage + (humor / 8) + rgen.Next(0, 21)); //Returns the attack damage
            }
            #endregion

            #region If the file cannot be read, create a default damage value
            catch //If the file cannot be read
            {
                return attackBaseDamage = 10; //Attack base damage
            }
            #endregion
        }

        public int Defend(int baseDamage) //The defense method for humor based attacks, based on wit
        {
            int damageTaken = baseDamage - rgen.Next(wit / 8, wit / 2);

            if (damageTaken >= 0) //If the damage is positive
            {
                return damageTaken; //Calculate how much damage the character takes
            }
            else //If the damage is negative
            {
                return 0;
            }
        }

        public int DefendIntelligence(int baseDamage) //The defense method for intelligence based attacks, based on intelligence
        {
            int damageTaken = baseDamage - rgen.Next(intelligence / 8, intelligence / 2);

            if (damageTaken >= 0) //If the damage is positive
            {
                return damageTaken; //Calculate how much damage the character takes
            }
            else //If the damage is negative
            {
                return 0;
            }
        }

        public void ReadAttackFileName(string attackFile1, string attackFile2, string attackFile3, string attackFile4) //Reads in the attack file's name
        {
            #region Assign the filenames
            attackFileName1 = attackFile1; //Assign the first filename
            attackFileName2 = attackFile2; //Assign the second filename
            attackFileName3 = attackFile3; //Assign the third filename
            attackFileName4 = attackFile4; //Assign the fourth filename
            #endregion

            #region Try to read the first file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(attackFile1); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[1]; //An array to hold the one piece of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split('`'); //Tells the array where each piece of data ends
                    attackString1 = characterData[0]; //Give the character the level specified in the file
                }
                fileReader.Close(); //Closes the fileReader
            }
            #endregion

            #region If the first file cannot be read, create a default attack name value
            catch //If the file cannot be read
            {
                attackString1 = "DefaultAttackName1"; //Attack string
            }
            #endregion


            #region Try to read the second file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(attackFile2); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[1]; //An array to hold the one piece of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split('`'); //Tells the array where each piece of data ends
                    attackString2 = characterData[0]; //Give the character the level specified in the file
                }
                fileReader.Close(); //Closes the fileReader
            }
            #endregion

            #region If the second file cannot be read, create a default attack name value
            catch //If the file cannot be read
            {
                attackString2 = "DefaultAttackName2"; //Attack string
            }
            #endregion


            #region Try to read the third file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(attackFile3); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[1]; //An array to hold the one piece of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split('`'); //Tells the array where each piece of data ends
                    attackString3 = characterData[0]; //Give the character the level specified in the file
                }
                fileReader.Close(); //Closes the fileReader
            }
            #endregion

            #region If the third file cannot be read, create a default attack name value
            catch //If the file cannot be read
            {
                attackString3 = "DefaultAttackName3"; //Attack string
            }
            #endregion


            #region Try to read the fourth file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader(attackFile4); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] characterData = new string[1]; //An array to hold the one piece of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    characterData = line.Split('`'); //Tells the array where each piece of data ends
                    attackString4 = characterData[0]; //Give the character the level specified in the file
                }
                fileReader.Close(); //Closes the fileReader
            }
            #endregion

            #region If the fourth file cannot be read, create a default attack name value
            catch //If the file cannot be read
            {
                attackString4 = "DefaultAttackName4"; //Attack string
            }
            #endregion
        }

        public void CheckDead() //This method will check if the character is dead and modify the boolean isDead accordingly, this should be done in a loop or in the update method in the game class
        {
            if (egoInBattle <= 0) //If ego drops below zero
            {
                isDead = true; //Mistah Kurtz, he dead.
            }
        }

        public void Activate() //This method will set the active statistic to true
        {
            isActive = true; //Call this when it is currently the character's turn
        }

        public void Deactivate() //This method will deactivate the character
        {
            isActive = false; //Deactivate the character after their turn
        }

        public abstract void Draw(Color colour, int pos); //Draws the player to the screen

        public abstract void Draw(); //Draws the player for game over
    }
}
