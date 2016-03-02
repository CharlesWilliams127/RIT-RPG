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
using System.IO;

namespace RIT_RPG
{
    class DestlerBossBattle 
    {
        //attributes
        #region Attributes
        #region Character Instantiation
        protected SmartassGuy c1; //Create a SmartassGuy object
        protected LowJokesGuy c2; //Create a LowJokesGuy object
        protected WittyGuy c3; //Create a WittyGuy object
        protected CoffeeGuy c4; //Create a CoffeeGuy object
        #endregion

        #region Enemy Instantiation and List
        private Destler destler;
        private List<Character> enemies; //Create a list of enemies
        #endregion

        private Character activeCharacter;
        private Random rgen; //Create a random number generator
        private int currentRound;
        private int prevRound;
        private int partyEnergy;
        private int enemyEnergy;
        private int currEnemyIndex;
        private int prevEnemyIndex;
        private KeyboardState keyState; //To read in keyboard input
        private KeyboardState prevKeyState;
        protected int pain; // final damage
        protected int actionChosen;
        protected int prevActionChosen;
        protected int time;
        protected Item usedItem;
        protected bool gameOver = false; //If the game is over
        #region Finite State Machine
        private enum BattleState { Player, Enemy, GameOver }
        private BattleState currentState = new BattleState();
        #endregion
        private bool hasRun = false; //If the check for the EndBattle has run
        private Character defender;
        private Timer stopwatch;
        private List<Item> bag;
        private bool isTalking = false;
        private bool returnToMain = false; // becomes true when the game is over and you return to main menu
        private string defenderName; //Defender name
        private SpeechMenu speechBalloon;
        private SpriteFont dbFont;
        private bool displayScreen;
        int damage; //Damage

        #region Row Switching
        private CharacterRows currentRoster;
        private CharacterRows newRoster; // created when a player switches a character's rows
        #endregion

        #region Drawing
        GraphicsDeviceManager graphics; //GraphicsDeviceManager
        SpriteBatch sBatch; //SpriteBatch
        Texture2D background; //Background
        Texture2D victory; //Victory
        Texture2D gameOverTop; //Game over top half
        Texture2D gameOverBottom; //Game over bottom half
        Texture2D iceCube; //Ice Cube
        Texture2D endBackground; //Background for game over
        private BattleMenu bMenu; //Create a BattleMenu object
        private int victoryPosition = -320; //For moving the victory screen
        private int randomCharacterDraw; //Choose a random character to draw at game over
        private int topGameOver = -240; //For moving the game over screen
        private int bottomGameOver = 480; //For moving the game over screen
        #endregion
        #endregion

        //Properties
        #region Properties
        public Character ActiveCharacter //ActiveCharacter property
        {
            get { return activeCharacter; }
        }
        public int Pain
        {
            get { return pain; }
        }
        public Character Defender
        {
            get { return defender; }
        }

        public bool IsTalking
        {
            get { return isTalking; }
        }

        public bool ReturnToMain
        {
            get { return returnToMain; }
            set { returnToMain = value; }
        }

        public string DefenderName
        {
            get { return defenderName; }
            set { defenderName = value; }
        }
        public bool DisplayScreen
        {
            get { return displayScreen; }
        }
        public int Damage
        {
            get { return damage; }
        }
        #endregion
        

        //constructor
        public DestlerBossBattle(SmartassGuy char1, LowJokesGuy char2, WittyGuy char3, CoffeeGuy char4, Destler dest, BattleMenu bM, GraphicsDeviceManager gdm,
                                 SpriteBatch sb, Texture2D bgrnd, Texture2D iCube, Texture2D vic, Texture2D gOTH, Texture2D gOBH, Texture2D endBgrnd, SpriteFont fnt)
        {
            #region Set Characters and add to Current Roster
            c1 = char1; //Set the SmartassGuy
            c2 = char2; //Set the LowJokesGuy
            c3 = char3; //Set the WittyGuy
            c4 = char4; //Set the CoffeeGuy
            #endregion

            #region Assign the Characters in a Linked List
            currentRoster = new CharacterRows(); // make a new linked list
            currentRoster.Add(c4); // add to the head of linked list
            currentRoster.Add(c3); // add to the linked list
            currentRoster.Add(c2); // ditto
            currentRoster.Add(c1); // ditto
            newRoster = Clone(currentRoster);
            #endregion
            //2 Revivals, 5 heals, 3 logic of class bomb
            #region Make a list of items
            bag = new List<Item>(); // create a list of items for the team to use
            SummonRitchie sr = new SummonRitchie(2); // give the team a summon ritchie item
            CuppaJoe cj = new CuppaJoe(5); // give the team three cuppa joes
            LogicBomb lb = new LogicBomb(3); // give the team two logic bombs
            // add them to the bag
            bag.Add(cj);
            bag.Add(lb);
            bag.Add(sr);
            #endregion 

            #region Set Enemies
            destler = dest; //Set the Professor
            
            #endregion

            #region Put Destler in a List
            enemies = new List<Character>();
            enemies.Add(destler);
            #endregion

            #region Drawing
            graphics = gdm; //Set the GraphicsDeviceManager
            sBatch = sb; //Set the SpriteBatch
            background = bgrnd; //Set the background
            iceCube = iCube; //Set the ice cube
            victory = vic; //Set the victory
            gameOverTop = gOTH; //Game over top half
            gameOverBottom = gOBH; //Game over bottom half
            endBackground = endBgrnd; //Game over background
            dbFont = fnt;
            bMenu = bM;
            speechBalloon = new SpeechMenu(graphics, sBatch, dbFont, bMenu.Background, bMenu.Position, bMenu.Height, bMenu.Width, this);
            #endregion

            #region Other
            rgen = new Random();
            pain = 0;
            currentRound = 0;
            time = 0;
            usedItem = null;
            stopwatch = new Timer();
            activeCharacter = currentRoster.Head;
            defender = null;
            displayScreen = false;

            randomCharacterDraw = rgen.Next(0, 4); //Random number generator to choose a character to draw
            #endregion
        }
        //methods

        public void RunBattle()
        {
            #region If it is the first round, decide whether the players or enemies will go first
            partyEnergy = c1.Energy + c2.Energy + c3.Energy + c4.Energy + rgen.Next(1, 11); //Calculate the total energy for the characters
            if (currentRound == 0)
            {
                enemyEnergy += destler.Energy + rgen.Next(1, 11);

                if (partyEnergy <= enemyEnergy)
                {
                    currentState = BattleState.Enemy;
                }
                else
                {
                    currentState = BattleState.Player;
                }
            }
            #endregion

            switch (currentState)
            {
                case BattleState.Enemy:
                   #region If the enemy is first
                    #region Reset integers for calculations
                        pain = 0; //Placeholder for damage calculation
                        damage = 0; //How much damage a character takes
                        #endregion

                    #region Loop through all enemies
                        if (destler.IsDead == false && destler.HasAttacked == false)
                        {
                            destler.IsTalking = true;
                            destler.HasAttacked = true;
                            defender = null;
                            defenderName = null;

                            #region Destler attacks
                                destler.Activate(); //Activate the enemy
                                switch (rgen.Next(0, 6))
                                {
                                    case 0: pain = destler.Attack("Insults\\destlerAttack.bin"); // base attack
                                        #region Base attack
                                        switch (rgen.Next(0, 4))
                                        {
                                            case 0: damage = c1.Defend(pain);
                                                defender = c1;
                                                defender.IsBeingAttacked = true;
                                                c1.TakeDamage(damage);
                                                c1.CheckDead(); //Check if the SmartassGuy is dead
                                                if (c1.IsDead == true) //If the SmartassGuy is dead
                                                {
                                                    currentRoster.Delete(c1.Name);
                                                }
                                                break;
                                            case 1: damage = c2.Defend(pain);
                                                defender = c2;
                                                defender.IsBeingAttacked = true;
                                                c2.TakeDamage(damage);
                                                c2.CheckDead(); //Check if the LowJokesGuy is dead
                                                if (c2.IsDead == true) //If the LowJokesGuy is dead
                                                {
                                                    currentRoster.Delete(c2.Name);
                                                }
                                                break;
                                            case 2: damage = c3.Defend(pain);
                                                defender = c3;
                                                defender.IsBeingAttacked = true;
                                                c3.TakeDamage(damage);
                                                c3.CheckDead(); //Check if the WittyGuy is dead
                                                if (c3.IsDead == true) //If the WittyGuy is dead
                                                {
                                                    currentRoster.Delete(c3.Name);
                                                }
                                                break;
                                            case 3: damage = c4.Defend(pain);
                                                defender = c4;
                                                defender.IsBeingAttacked = true;
                                                c4.TakeDamage(damage);
                                                c4.CheckDead(); //Check if the CoffeeGuy is dead
                                                if (c4.IsDead == true) //If the CoffeeGuy is dead
                                                {
                                                    currentRoster.Delete(c4.Name);
                                                }
                                                break;
                                        }
                                        #endregion
                                        break;
                                    case 1: pain = destler.Superiority(); //intelligence based attack
                                        #region Superiority
                                        switch (rgen.Next(0, 4))
                                        {
                                            case 0: damage = c1.DefendIntelligence(pain);
                                                defender = c1;
                                                defender.IsBeingAttacked = true;
                                                c1.TakeDamage(damage);
                                                c1.CheckDead(); //Check if the SmartassGuy is dead
                                                if (c1.IsDead == true) //If the SmartassGuy is dead
                                                {
                                                    currentRoster.Delete(c1.Name);
                                                }
                                                break;
                                            case 1: damage = c2.DefendIntelligence(pain);
                                                defender = c2;
                                                defender.IsBeingAttacked = true;
                                                c2.TakeDamage(damage);
                                                c2.CheckDead(); //Check if the LowJokesGuy is dead
                                                if (c2.IsDead == true) //If the LowJokesGuy is dead
                                                {
                                                    currentRoster.Delete(c2.Name);
                                                }
                                                break;
                                            case 2: damage = c3.DefendIntelligence(pain);
                                                defender = c3;
                                                defender.IsBeingAttacked = true;
                                                c3.TakeDamage(damage);
                                                c3.CheckDead(); //Check if the WittyGuy is dead
                                                if (c3.IsDead == true) //If the WittyGuy is dead
                                                {
                                                    currentRoster.Delete(c3.Name);
                                                }
                                                break;
                                            case 3: damage = c4.DefendIntelligence(pain);
                                                defender = c4;
                                                defender.IsBeingAttacked = true;
                                                c4.TakeDamage(damage);
                                                c4.CheckDead(); //Check if the CoffeeGuy is dead
                                                if (c4.IsDead == true) //If the CoffeeGuy is dead
                                                {
                                                    currentRoster.Delete(c4.Name);
                                                }
                                                break;
                                        }
                                        #endregion
                                        break;
                                    case 2: damage = destler.MADdamage(); // hits ENTIRE party
                                        #region DesperationSpecialMove
                                        defenderName = "All";
                                        c1.TakeDamage(damage);
                                        c2.TakeDamage(damage);
                                        c3.TakeDamage(damage);
                                        c4.TakeDamage(damage);
                                        if (c1.IsDead == true) //If the SmartassGuy is dead
                                        {
                                            currentRoster.Delete(c1.Name);
                                        }
                                        if (c2.IsDead == true) //If the LowJokesGuy is dead
                                        {
                                            currentRoster.Delete(c2.Name);
                                        }
                                        if (c3.IsDead == true) //If the WittyGuy is dead
                                        {
                                            currentRoster.Delete(c3.Name);
                                        }
                                        if (c4.IsDead == true) //If the CoffeeGuy is dead
                                        {
                                            currentRoster.Delete(c4.Name);
                                        }
                                        #endregion
                                        break;
                                    case 3: destler.TuneBanjo();
                                        #region Healing move
                                        defenderName = destler.Name;
                                        #endregion
                                        break;
                                    case 4: destler.Tuition(); //boosts his own stats
                                        #region Tuition
                                        defenderName = destler.Name;
                                        #endregion
                                        break;
                                    case 5: destler.BanjoShreddin(); //other main attack
                                        #region Banjo attack
                                        switch (rgen.Next(0, 4))
                                        {
                                            case 0: damage = c1.Defend(pain);
                                                defender = c1;
                                                defender.IsBeingAttacked = true;
                                                c1.TakeDamage(damage);
                                                c1.CheckDead(); //Check if the SmartassGuy is dead
                                                if (c1.IsDead == true) //If the SmartassGuy is dead
                                                {
                                                    currentRoster.Delete(c1.Name);
                                                }
                                                break;
                                            case 1: damage = c2.Defend(pain);
                                                defender = c2;
                                                defender.IsBeingAttacked = true;
                                                c2.TakeDamage(damage);
                                                c2.CheckDead(); //Check if the LowJokesGuy is dead
                                                if (c2.IsDead == true) //If the LowJokesGuy is dead
                                                {
                                                    currentRoster.Delete(c2.Name);
                                                }
                                                break;
                                            case 2: damage = c3.Defend(pain);
                                                defender = c3;
                                                defender.IsBeingAttacked = true;
                                                c3.TakeDamage(damage);
                                                c3.CheckDead(); //Check if the WittyGuy is dead
                                                if (c3.IsDead == true) //If the WittyGuy is dead
                                                {
                                                    currentRoster.Delete(c3.Name);
                                                }
                                                break;
                                            case 3: damage = c4.Defend(pain);
                                                defender = c4;
                                                defender.IsBeingAttacked = true;
                                                c4.TakeDamage(damage);
                                                c4.CheckDead(); //Check if the CoffeeGuy is dead
                                                if (c4.IsDead == true) //If the CoffeeGuy is dead
                                                {
                                                    currentRoster.Delete(c4.Name);
                                                }
                                                break;
                                        }
                                        #endregion
                                        break;
                                }
                            #endregion
                        } //FUCK WE GOTTA COMMENT THIS SHEEEEIT

                        //prevEnemyIndex = currEnemyIndex;

                        if (stopwatch.Freeze(120) == true)
                        {
                            defender = null;
                            defenderName = null;
                            destler.IsTalking = false;
                            destler.HasAttacked = false;
                            #region End of round checks and break
                            CheckForGameOver(); //Run check for game over method
                            currentRound++;
                            //currEnemyIndex = -1;
                            //prevEnemyIndex = -1;
                            newRoster = Clone(currentRoster);
                            activeCharacter = currentRoster.Head; // reset the player character's linked list
                            currentState = BattleState.Player;
                            #endregion
                        }
                        break;
                    
                    #endregion
                   #endregion //   eorks

                case BattleState.Player:
                   #region If the player is first
                    keyState = Keyboard.GetState(); //Get the keyboard state

                    if (activeCharacter != null)
                    {
                        // if character already has attacked, bring in the next one
                        if (activeCharacter.Next != null && activeCharacter.HasAttacked == true)
                        {
                            if (stopwatch.Freeze(120) == true)
                            {
                                defender = null; //Reset the defender

                                activeCharacter.IsTalking = false;
                                prevActionChosen = -1;
                                activeCharacter = activeCharacter.Next;
                            }
                        }

                        // if current character has not attacked yet, run select action until it does
                        else if (activeCharacter.HasAttacked == false)
                        {
                            activeCharacter.IsActive = true;
                            selectAction(activeCharacter);
                        }

                        // if all the characters have attacked
                        else if (activeCharacter.Next == null && activeCharacter.HasAttacked == true)
                        {
                            prevActionChosen = -1;

                            if (stopwatch.Freeze(120) == true)
                            {
                                #region End of round checks and break
                                activeCharacter = currentRoster.Head;
                                while (activeCharacter != null)
                                {
                                    activeCharacter.IsTalking = false;
                                    activeCharacter.IsActive = false;
                                    activeCharacter.HasAttacked = false;
                                    activeCharacter.UsedItem = false;
                                    activeCharacter = activeCharacter.Next;
                                }
                                currentRoster = Clone(newRoster); // set the current roster to the modified character roster
                                currentRound++;
                                currentState = BattleState.Enemy;
                                #endregion
                            }
                        }
                    }
                    CheckForGameOver();
                    break;
                    #endregion

                case BattleState.GameOver:
                   #region If the game ends
                    if (c1.IsDead == true && c2.IsDead == true && c3.IsDead == true && c4.IsDead == true)
                       {
                               GameOver();
                       }
                    else if (destler.IsDead == true)
                      {
                            EndBattle();
                      }
                      break;
                    #endregion
            }
         }
        


        // cleaned up version of character choice selection
        public void selectAction(Character ch)
        {
            actionChosen = bMenu.ProcessInput(ch, bag);

            if (actionChosen > -1)
            {
                prevActionChosen = actionChosen;
            }

            #region Secret Keys
            /*
            // Kill Key
            if(prevKeyState.IsKeyDown(Keys.K) == true && keyState.IsKeyUp(Keys.K) == true)
            {
                c1.IsDead = true;
                c2.IsDead = true;
                c3.IsDead = true;
                c4.IsDead = true;
            }

            // Win Key
            if (prevKeyState.IsKeyDown(Keys.L) == true && keyState.IsKeyUp(Keys.L) == true)
            {
                codeMankey.IsDead = true;
                codeTA.IsDead = true;
                codeProf.IsDead = true;
            }
            */
            #endregion

            switch (prevActionChosen)
            {
                #region If the first attack was selected
                case 0: //if attack is selected, 
                    {
                       /* if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true) // select enemy
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName1); // pass in the attack value from the file to an attack variable
                            pain = codeMankey.Defend(attack); // The enemy will attempt to defend itself
                            codeMankey.TakeDamage(pain); // BRING THE PAIN
                            defender = codeMankey;
                            defender.IsBeingAttacked = true; //The code monkey is being attacked                           
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeMankey.CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        } */
                        if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true)
                        {
                            damage = 0;
                            int attack = ch.Attack(ch.AttackFileName1); // pass in the attack value from the file to an attack variable
                            damage = enemies[0].Defend(attack); // The enemy will attempt to defend itself
                            enemies[0].TakeDamage(damage); // BRING THE PAIN
                            defender = enemies[0];
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            enemies[0].CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        }
                        /*else if (keyState.IsKeyDown(Keys.D3) == true && prevKeyState.IsKeyUp(Keys.D3) == true || keyState.IsKeyDown(Keys.NumPad3) == true && prevKeyState.IsKeyUp(Keys.NumPad3) == true)
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName1); // pass in the attack value from the file to an attack variable
                            pain = codeProf.Defend(attack); // The enemy will attempt to defend itself
                            codeProf.TakeDamage(pain); // BRING THE PAIN
                            defender = codeProf;
                            defender.IsBeingAttacked = true; //The professor is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeProf.CheckDead(); //Check if the Professor is dead
                            CheckForGameOver(); //Run check for game over method
                        }*/
                    }
                    break;
                #endregion

                #region If the second attack was selected
                case 1:
                    {
                      /* if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true) // select enemy
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName2); // pass in the attack value from the file to an attack variable
                            pain = codeMankey.Defend(attack); // The enemy will attempt to defend itself
                            codeMankey.TakeDamage(pain); // BRING THE PAIN
                            defender = codeMankey;
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeMankey.CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        }*/
                        if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true)
                        {
                            damage = 0;
                            int attack = ch.Attack(ch.AttackFileName2); // pass in the attack value from the file to an attack variable
                            damage = enemies[0].Defend(attack); // The enemy will attempt to defend itself
                            enemies[0].TakeDamage(damage); // BRING THE PAIN
                            defender = enemies[0];
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            enemies[0].CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        }
                       /* else if (keyState.IsKeyDown(Keys.D3) == true && prevKeyState.IsKeyUp(Keys.D3) == true || keyState.IsKeyDown(Keys.NumPad3) == true && prevKeyState.IsKeyUp(Keys.NumPad3) == true) // select enemy
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName2); // pass in the attack value from the file to an attack variable
                            pain = codeProf.Defend(attack); // The enemy will attempt to defend itself
                            codeProf.TakeDamage(pain); // BRING THE PAIN
                            defender = codeProf;
                            defender.IsBeingAttacked = true; //The professor is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeProf.CheckDead(); //Check if the Professor is dead
                            CheckForGameOver(); //Run check for game over method
                        } */
                    }
                    break;
                #endregion

                #region If the third attack was selected
                case 2:
                    {
                       /* if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true) // select enemy
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName3); // pass in the attack value from the file to an attack variable
                            pain = codeMankey.Defend(attack); // The enemy will attempt to defend itself
                            codeMankey.TakeDamage(pain); // BRING THE PAIN
                            defender = codeMankey;
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeMankey.CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        } */
                        if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true)
                        {
                            damage = 0;
                            int attack = ch.Attack(ch.AttackFileName3); // pass in the attack value from the file to an attack variable
                            damage = enemies[0].Defend(attack); // The enemy will attempt to defend itself
                            enemies[0].TakeDamage(damage); // BRING THE PAIN
                            defender = enemies[0];
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            enemies[0].CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        }
                        /*else if (keyState.IsKeyDown(Keys.D3) == true && prevKeyState.IsKeyUp(Keys.D3) == true || keyState.IsKeyDown(Keys.NumPad3) == true && prevKeyState.IsKeyUp(Keys.NumPad3) == true)
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName3); // pass in the attack value from the file to an attack variable
                            pain = codeProf.Defend(attack); // The enemy will attempt to defend itself
                            codeProf.TakeDamage(pain); // BRING THE PAIN
                            defender = codeProf;
                            defender.IsBeingAttacked = true; //The professor is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeProf.CheckDead(); //Check if the Professor is dead
                            CheckForGameOver(); //Run check for game over method
                        }*/
                    }
                    break;
                #endregion

                #region If the fourth attack was selected
                case 3:
                    {
                       /* if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true) // select enemy
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName4); // pass in the attack value from the file to an attack variable
                            pain = codeMankey.Defend(attack); // The enemy will attempt to defend itself
                            codeMankey.TakeDamage(pain); // BRING THE PAIN
                            defender = codeMankey;
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeMankey.CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        } */
                        if (keyState.IsKeyDown(Keys.D1) == true && prevKeyState.IsKeyUp(Keys.D1) == true || keyState.IsKeyDown(Keys.NumPad1) == true && prevKeyState.IsKeyUp(Keys.NumPad1) == true)
                        {
                            damage = 0;
                            int attack = ch.Attack(ch.AttackFileName4); // pass in the attack value from the file to an attack variable
                            damage = enemies[0].Defend(attack); // The enemy will attempt to defend itself
                            enemies[0].TakeDamage(damage); // BRING THE PAIN
                            defender = enemies[0];
                            defender.IsBeingAttacked = true; //The code monkey is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            enemies[0].CheckDead(); //Check if the Code Monkey is dead
                            CheckForGameOver(); //Run check for game over method
                        }
                       /* else if (keyState.IsKeyDown(Keys.D3) == true && prevKeyState.IsKeyUp(Keys.D3) == true || keyState.IsKeyDown(Keys.NumPad3) == true && prevKeyState.IsKeyUp(Keys.NumPad3) == true)
                        {
                            pain = 0;
                            int attack = ch.Attack(ch.AttackFileName4); // pass in the attack value from the file to an attack variable
                            pain = codeProf.Defend(attack); // The enemy will attempt to defend itself
                            codeProf.TakeDamage(pain); // BRING THE PAIN
                            defender = codeProf;
                            defender.IsBeingAttacked = true; //The professor is being attacked
                            ch.IsTalking = true;
                            ch.HasAttacked = true;
                            bMenu.ResetMenu(); // reset the menu for the next guy
                            codeProf.CheckDead(); //Check if the Professor is dead
                            CheckForGameOver(); //Run check for game over method
                        }*/
                    }
                    break;
                #endregion

                #region First item selected
                case 4:
                    {
                        // do nothing unless there is one or more item in the bag
                        if (bag.Count >= 1)
                        {
                            ch.IsTalking = true;
                            ch.UsedItem = true;
                            usedItem = bag[0];

                            if (bag[0] is ReviveItem)
                            {
                                ReviveItem item = (ReviveItem)bag[0]; // cast the item as a revive item

                                item.Use(currentRoster);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[0] is HealthItem)
                            {
                                HealthItem item = (HealthItem)bag[0]; // cast the item as a health item
                                item.Use(ch);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[0] is DamageItem)
                            {
                                DamageItem item = (DamageItem)bag[0]; // cast the item as a damage item
                                item.Use(enemies);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            // delete the item if you've run out
                            if (bag[0].Uses <= 0)
                            {
                                bag.RemoveAt(0);
                            }
                        }
                    }
                    break;
                #endregion

                #region Second item selected
                case 5:
                    {
                        // do nothing unless there are two or more items in the bag
                        if (bag.Count >= 2)
                        {
                            ch.IsTalking = true;
                            ch.UsedItem = true;
                            usedItem = bag[1];

                            if (bag[1] is ReviveItem)
                            {
                                ReviveItem item = (ReviveItem)bag[1]; // cast the item as a revive item

                                item.Use(currentRoster);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[1] is HealthItem)
                            {
                                HealthItem item = (HealthItem)bag[1]; // cast the item as a health item
                                item.Use(ch);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[1] is DamageItem)
                            {
                                DamageItem item = (DamageItem)bag[1]; // cast the item as a damage item
                                item.Use(enemies);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            // delete the item if you've run out
                            if (bag[1].Uses <= 0)
                            {
                                bag.RemoveAt(1);
                            }
                        }
                    }
                    break;
                #endregion

                #region Third item selected
                case 6:
                    {
                        // do nothing unless there are three or more items in the bag
                        if (bag.Count >= 3)
                        {
                            ch.IsTalking = true;
                            ch.UsedItem = true;
                            usedItem = bag[2];

                            if (bag[2] is ReviveItem)
                            {
                                ReviveItem item = (ReviveItem)bag[2]; // cast the item as a revive item

                                item.Use(currentRoster);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[2] is HealthItem)
                            {
                                HealthItem item = (HealthItem)bag[2]; // cast the item as a health item
                                item.Use(ch);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[2] is DamageItem)
                            {
                                DamageItem item = (DamageItem)bag[2]; // cast the item as a damage item
                                item.Use(enemies);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            // delete the item if you've run out
                            if (bag[2].Uses <= 0)
                            {
                                bag.RemoveAt(2);
                            }
                        }
                    }
                    break;
                #endregion

                #region Fourth item selected
                case 7:
                    {
                        // do nothing unless there are four or more items in the bag
                        if (bag.Count >= 4)
                        {
                            ch.IsTalking = true;
                            ch.UsedItem = true;
                            usedItem = bag[3];

                            if (bag[3] is ReviveItem)
                            {
                                ReviveItem item = (ReviveItem)bag[3]; // cast the item as a revive item

                                item.Use(currentRoster);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[3] is HealthItem)
                            {
                                HealthItem item = (HealthItem)bag[3]; // cast the item as a health item
                                item.Use(ch);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            if (bag[3] is DamageItem)
                            {
                                DamageItem item = (DamageItem)bag[3]; // cast the item as a damage item
                                item.Use(enemies);

                                ch.HasAttacked = true;
                                bMenu.ResetMenu();
                            }

                            // delete the item if you've run out
                            if (bag[3].Uses <= 0)
                            {
                                bag.RemoveAt(3);
                            }
                        }
                    }
                    break;
                #endregion

                #region First row chosen
                case 8:
                    {
                        Character removedGuy = newRoster.Delete(ch.Name); // remove the selected character from the rows and copy it into a new variable

                        newRoster.Insert(removedGuy, -1); // put the guy at the head
                        bMenu.ResetMenu(); // reset the menu for the next guy
                        ch.IsTalking = true;
                        ch.HasAttacked = true;
                    }
                    break;
                #endregion

                #region Second row chosen
                case 9:
                    {
                        Character removedGuy = newRoster.Delete(ch.Name); // remove the selected character from the rows and copy it into a new variable

                        newRoster.Insert(removedGuy, 1); // put the guy at the new row
                        bMenu.ResetMenu(); // reset the menu for the next guy
                        ch.IsTalking = true;
                        ch.HasAttacked = true;
                    }
                    break;
                #endregion

                #region Third row chosen
                case 10:
                    {
                        Character removedGuy = newRoster.Delete(ch.Name); // remove the selected character from the rows and copy it into a new variable

                        newRoster.Insert(removedGuy, 2); // put the guy at the new row
                        bMenu.ResetMenu(); // reset the menu for the next guy
                        ch.IsTalking = true;
                        ch.HasAttacked = true;
                    }
                    break;
                #endregion

                #region Fourth row chosen
                case 11:
                    {
                        Character removedGuy = newRoster.Delete(ch.Name); // remove the selected character from the rows and copy it into a new variable

                        newRoster.Insert(removedGuy, 4); // put the guy at the end
                        bMenu.ResetMenu(); // reset the menu for the next guy
                        ch.IsTalking = true;
                        ch.HasAttacked = true;
                    }
                    break;
                #endregion

                #region Run Away
                case 12:
                    {
                        c1.IsDead = true;
                        c2.IsDead = true;
                        c3.IsDead = true;
                        c4.IsDead = true;
                    }
                    break;
                #endregion
            }

            prevKeyState = keyState; //Reset the previous keyboard state
        }

        // clone this list into a new one
        public CharacterRows Clone(CharacterRows orig)
        {
            CharacterRows duplicate = new CharacterRows(); //Create a CharacterRows object
            Character curr = orig.Head; // start at the head of the original list

            // loop until character list is duplicated
            while (curr != null)
            {
                #region Characters
                if (curr.CharacterClass == "Coffee Guy")
                {
                    CoffeeGuy clone = new CoffeeGuy(c4);
                    duplicate.Add(clone);
                }

                if (curr.CharacterClass == "Witty Guy")
                {
                    WittyGuy clone = new WittyGuy(c3);
                    duplicate.Add(clone);
                }

                if (curr.CharacterClass == "Low Jokes Guy")
                {
                    LowJokesGuy clone = new LowJokesGuy(c2);
                    duplicate.Add(clone);
                }

                if (curr.CharacterClass == "Smartass Guy")
                {
                    SmartassGuy clone = new SmartassGuy(c1);
                    duplicate.Add(clone);
                }
                #endregion

                curr = curr.Next; // step through to the next character
            }
            return duplicate;
        }

        /// <summary>
        /// Brings up the battle Menu and the battlescreen
        /// </summary>
        /// 
        public void CheckForGameOver() //Checks to see if the game is over
        {
            if (c1.IsDead == true && c2.IsDead == true && c3.IsDead == true && c4.IsDead == true) //If all players are dead
            {
                currentState = BattleState.GameOver;
            }
            else if (destler.IsDead == true) //If all enemies are dead
            {
                currentState = BattleState.GameOver;
            }
        }

        public void DrawScreen()
        {
            #region Background Drawing
            sBatch.Draw(background, new Rectangle(0, 0, 600, 480), Color.White); //Draw the background
            #endregion

            #region Draw the BattleMenu
            if (currentState == BattleState.Player && activeCharacter.IsTalking == false)
            {
                bMenu.DrawBattleMenu(activeCharacter, bag, enemies);
            }
            #endregion

            #region Character Drawing
            Character curr = newRoster.Head;
            int pos = 0;

            while (curr != null)
            {
                if (curr.IsDead == false)
                {
                    curr.Draw(Color.White, pos);
                }
                else if (curr.IsDead == true)
                {
                    curr.Draw(Color.Transparent, pos);
                }

                curr = curr.Next;
                pos++;
            }
            #endregion

            #region Enemy Drawing

                if (destler.IsDead == false) //If the enemy is alive
                {
                    destler.Draw(Color.White, 0); //Draw the Professor
                }
                else //If the enemy is dead
                {
                    destler.Draw(Color.Transparent, 0); //Draw the Professor
                }
 
            #endregion

            #region Drawing Text
            #region Active Character
            if (activeCharacter != null)
            {
                if (activeCharacter.IsTalking == true && activeCharacter.HasAttacked == true) //Current guy text
                {
                    if (activeCharacter.UsedItem == true) //If the character uses an item
                    {
                        speechBalloon.Talk(activeCharacter, usedItem);
                    }
                    else //Switch rows selected
                    {
                        speechBalloon.Talk(activeCharacter); //Tell which row is being switched
                    }
                    if (defender is Destler) //If the Destler is being attacked
                    {
                        speechBalloon.Talk(activeCharacter, destler);
                    }
                }
            }
            #endregion

            #region Current Enemy
                if (destler.IsTalking == true && destler.HasAttacked == true) //Current enemy text
                {
                    if (defenderName != null)
                    {
                        speechBalloon.Talk(destler, defenderName);
                    }
                    else if (c1.IsBeingAttacked == true) //If the SmartassGuy is being attacked
                    {
                        speechBalloon.Talk(destler, c1);
                    }
                    else if (c2.IsBeingAttacked == true) //If the LowJokesGuy is being attacked
                    {
                        speechBalloon.Talk(destler, c2);
                    }
                    else if (c3.IsBeingAttacked == true) //If the WittyGuy is being attacked
                    {
                        speechBalloon.Talk(destler, c3);
                    }
                    else if (c4.IsBeingAttacked == true) //If the CoffeeGuy is being attacked
                    {
                        speechBalloon.Talk(destler, c4);
                    }
                }
         
            #endregion
            #endregion

            if (currentState == BattleState.GameOver) //If the game is over
            {
                const int MOVE = 1; //For animation

                #region VICTORY
                if (hasRun == true) //If the party wins
                {
                    sBatch.Draw(endBackground, new Vector2(0, 0), Color.White); //Draw the background           

                    if (victoryPosition != 0) //While the animation should run
                    {
                        sBatch.Draw(victory, new Vector2(0, victoryPosition += MOVE), Color.White); //Draw the victory screen
                    }
                    else //Draw the last frame and stop the animation
                    {
                        sBatch.Draw(victory, new Vector2(victoryPosition, 0), Color.White); //Draw the victory screen
                    }
                    if (stopwatch.Freeze(360) == true) //Freeze the screen
                    {
                        displayScreen = true;
                    }
                }
                #endregion

                #region DEFEAT
                if (gameOver == true) //If the party loses
                {
                    sBatch.Draw(endBackground, new Vector2(0, 0), Color.White); //Draw the background                   

                    #region Add the character for the ice cube
                    if (randomCharacterDraw == 0) //If the number was 0
                    {
                        c1.Draw(); //Draw the SmartassGuy
                    }
                    if (randomCharacterDraw == 1) //If the number was 1
                    {
                        c2.Draw(); //Draw the LowJokesGuy
                    }
                    if (randomCharacterDraw == 2) //If the number was 2
                    {
                        c3.Draw(); //Draw the WittyGuy
                    }
                    if (randomCharacterDraw == 3) //If the number was 3
                    {
                        c4.Draw(); //Draw the CoffeeGuy
                    }
                    #endregion

                    sBatch.Draw(iceCube, new Vector2(0, 0), Color.White); //Draw the ice cube

                    if (topGameOver != 0) //While the animation should run
                    {
                        sBatch.Draw(gameOverTop, new Vector2(0, topGameOver += MOVE), Color.White); //Draw the top game over screen
                    }
                    else //Draw the last frame and stop the animation
                    {
                        sBatch.Draw(gameOverTop, new Vector2(0, 0), Color.White); //Draw the top game over screen
                    }

                    if (bottomGameOver != 241) //While the animation should run
                    {
                        sBatch.Draw(gameOverBottom, new Vector2(0, bottomGameOver -= MOVE), Color.White); //Draw the bottom game over screen
                    }
                    else //Draw the last frame and stop the animation
                    {
                        sBatch.Draw(gameOverBottom, new Vector2(0, 241), Color.White); //Draw the bottom game over screen
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Ends the battle if the player wins
        /// </summary>
        public void EndBattle()
        {
            #region Experience Gain
            if (hasRun == false)
            {
                if (c1.IsDead == false) //If the character is not dead
                {
                    c1.ExpGain(destler.ExperienceFromEnemy); //Give the SmartassGuy some experience
                }

                if (c2.IsDead == false) //If the character is not dead
                {
                    c2.ExpGain(destler.ExperienceFromEnemy); //Give the LowJokesGuy some experience
                }

                if (c3.IsDead == false) //If the character is not dead
                {
                    c3.ExpGain(destler.ExperienceFromEnemy); //Give the WittyGuy some experience
                }

                if (c4.IsDead == false) //If the character is not dead
                {
                    c4.ExpGain(destler.ExperienceFromEnemy); //Give the CoffeeGuy some experience
                }
                hasRun = true;
            }
            #endregion

            #region Heal Characters
            #region Smartass Guy
            c1.EgoInBattle = c1.Ego; //Reset ego
            c1.HumorInBattle = c1.Humor; //Reset humor
            c1.IntelligenceInBattle = c1.Intelligence; //Reset intelligence
            c1.EnergyInBattle = c1.Energy; //Reset energy
            c1.WitInBattle = c1.Wit; //Reset wit
            c1.IsDead = false; //IT"S ALIVE
            #endregion

            #region Low Jokes Guy
            c2.EgoInBattle = c2.Ego; //Reset ego
            c2.HumorInBattle = c2.Humor; //Reset humor
            c2.IntelligenceInBattle = c2.Intelligence; //Reset intelligence
            c2.EnergyInBattle = c2.Energy; //Reset energy
            c2.WitInBattle = c2.Wit; //Reset wit
            c2.IsDead = false; //IT"S ALIVE
            #endregion

            #region Witty Guy
            c3.EgoInBattle = c3.Ego; //Reset ego
            c3.HumorInBattle = c3.Humor; //Reset humor
            c3.IntelligenceInBattle = c3.Intelligence; //Reset intelligence
            c3.EnergyInBattle = c3.Energy; //Reset energy
            c3.WitInBattle = c3.Wit; //Reset wit
            c3.IsDead = false; //IT"S ALIVE
            #endregion

            #region Coffee Guy
            c4.EgoInBattle = c4.Ego; //Reset ego
            c4.HumorInBattle = c4.Humor; //Reset humor
            c4.IntelligenceInBattle = c4.Intelligence; //Reset intelligence
            c4.EnergyInBattle = c4.Energy; //Reset energy
            c4.WitInBattle = c4.Wit; //Reset wit
            c4.IsDead = false; //IT"S ALIVE
            #endregion
            #endregion

            #region Saving
            try //Write the save file
            {
                StreamWriter fileWriter = new StreamWriter("savedGame.bin"); //Create the StreamWriter object
                fileWriter.Write("true" + ","); //First battle completion
                fileWriter.Write("true" + ","); //Second battle completion
                fileWriter.Write("false"); //Third battle completion
                fileWriter.Close(); //Close the StreamWriter
            }
            catch (Exception)
            {

            }
            #endregion
        }

        /// <summary>
        /// if all allies are dead, Draw Game Over Screen
        /// </summary>
        public void GameOver()
        {
            gameOver = true;

            if (stopwatch.Freeze(360) == true)
            {
                returnToMain = true;
                //currentState = BattleState.Player;
                Environment.Exit(1);
            }
        }
    }
}
