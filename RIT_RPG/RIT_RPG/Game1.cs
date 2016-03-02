#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

namespace RIT_RPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState kState;
        MouseState mState;

        #region Game Content
        #region Window Attributes
        int gameWidth = 600; //Width of the display window
        int gameHeight = 480; //Height of the display window
        #endregion

        #region Button Attributes
        int buttonWidth = 128;
        int buttonHeight = 64;
        #endregion

        #region BattleMenu Height
        int bMenuHeight = 175;
        #endregion

        #region Texture and Font Attributes
        Texture2D speechMenuBack; // background for the speech menu
        Texture2D ritLogo; // game logo
        Texture2D newButtonFile; // new button texture
        Texture2D loadButtonFile; // load button texture
        Texture2D continueButtonFile; //Continue button texture
        Texture2D mainBackground; // background for the main menu
        Texture2D selectorText; // texture for the battle menu's selector
        Texture2D crystalSnow; //Game over background texture
        Texture2D gameOverTop; //Game over top texture
        Texture2D gameOverBottom; //Game over bottom texture
        Texture2D iceCube; //Ice cube for game over texture
        Texture2D victory; //Victory texture
        Texture2D orangeHall; //Orange hall background
        Texture2D eastman; //Eastman background
        SpriteFont gameFont; // sprite font for the game (possibly tentative)
        #endregion

        #region Rectangle Attributes
        Rectangle mouseCursor; // rectangle for clicking on buttons
        Rectangle logo; // The size and position of the logo;
        #endregion

        #region Menu Objects
        MainMenu titleScreen; // main game screen object
        BattleMenu bMenu; // battle menu
        SpeechMenu sMenu;
        TransitionScreen tScreen;
        TransitionScreen tScreen2;
        TransitionScreen tScreen3;
        TransitionScreen end;
        #region Buttons
        Button newButton; // new button drawn to the screen
        Button loadButton; // load button drawn to the screen
        Button continueButton;
        #endregion
        #endregion

        #region Battle Objects
        DemoBattle dBattle; //Creates a DemoBattle object
        DestlerBossBattle destlerBattle; //DestlerBattle object
        FinalBossBattle finalBattle; //FinalBattle object
        #endregion

        #region Finite State Machine
        enum MenuState { Main, Battle, DestlerBattle, FinalBattle, Transition, Transition2, Transition3, Stop }
        MenuState gameState = new MenuState();
        #endregion

        #region Character Objects and Textures
        #region SmartassGuy
        SmartassGuy sAG; //SmartassGuy object
        Texture2D sAGSprite; //SmartassGuy sprite
        #endregion

        #region LowJokesGuy
        LowJokesGuy lJG; //LowJokesGuy object
        Texture2D lJGSprite; //LowJokesGuy sprite
        #endregion

        #region WittyGuy
        WittyGuy wG; //WittyGuy object
        Texture2D wGSprite; //WittyGuy sprite
        #endregion

        #region CoffeeGuy
        CoffeeGuy cG; //CoffeeGuy object
        Texture2D cGSprite; //CoffeeGuy sprite
        #endregion
        #endregion

        #region Enemy Objects and Textures
        #region Professor
        CodeProfessor cPDemo; //CodeProfessor object
        Texture2D cPDemoSprite; //CodeProfessor sprite
        #endregion

        #region Code Monkey
        CodeMonkey cMDemo; //CodeMonkey object
        Texture2D cMDemoSprite; //CodeMonkey sprite
        #endregion

        #region TA
        CodeTA cTADemo; //CodeTA object
        Texture2D cTADemoSprite; //CodeTA sprite
        #endregion

        #region Destler
        Destler dest;
        Texture2D destlerSprite;
        #endregion

        #region Final Boss
        FinalBoss fBoss;
        Texture2D fBossSprite;
        #endregion

        #region Tank
        TankCharacter tank;
        Texture2D tankSprite;
        #endregion
        #endregion

        #region TransitionText
        string chapter1 = "In the frozen tundra that is the RIT campus, your valiant party sets forth \nacross the frozen wasteland. " +
            "Along the way, you realize you will encounter \nhorrors too terrible to describe. The horrors of imagination, hilarity,\n and a GPA below 2.0. "
            + "\nThe four heroes set forth to restore the forgotten \nseason of Spring to the frozen land of abhorrent tortures." +
            "\nIt is believed that there is an evil warlock that lurks at the \nhighest peak of the Eastman tower." +
            "\nLegend (RIT Message Center) says, that this warlock can \ncontrol the very weather itself." +
            "\nAlong the way to the warlock many horrors emerge, \ndetermined to fail and humiliate the party." +
            "\nCan their wit, intelligence, and humor hold their ego afloat? \nWill it keep their GPA from freezing?";
        string chapter2 = "After vanquishing the villains of Code poop and \n professoring of the Bro variety, the party\n feeling wary rest their battle worn selves. " +
            "\nThe snow and ice relentlessly continuing to pile and grow as time passes, \nwith the threat of a wintery death encroaching," +
           " \nthe party reaches Eastman tower and with haste \nmake sure that the magical machine of vending restocks their supplies." +
            "\nOnce the party feels warm and secure enough, they race to the tiger statue..." +
            "\nIT WAS A LONG RUN..." +
            "\nAt the tiger statue, the party gears up for battle, albeit with a need \nto catch up their breath. Here, they encounter the Banjo Warlock himself:\n President Destler.";
        string chapter3 = "With the Banjo warlock defeated, the party receives a shocking revelation.\n The Warlock not the true foe in this endeavor." +
           "'You wretched fools!'\n The warlock cried as he was cast down, 'I was not the one who \nwilled this to be! It was Prof...'" +
           "Suddenly a tank erupts through the wall.\n Without any warning or introduction, the true mastermind reveals himself...\n" +
           "A long haired man comes up the top of the tank.\n" +
           "'You nerds are so gullible!' the man exclaimed 'With the banjo warlock \ndefeated we can now take control of the weather machine entirely'" +
           "\nThe word 'we' seemed to cue a grey haired man to appear. " +
           "\n'We now have what we need to keep this winter going on \nfor ALL ETERNITY,' He added." +
           "'Gladly Professor!'";
        string chapter4 = "The professor begins to laugh in his defeat. \n" +
          "He cried, 'You fools, the machine is what my Mega Computer need to run!\n It keeps it alive! How else am I going to play Crysis on maximum\n specifications!'" + 
	      "Confused the party looks on and sees a computer the size of \na room; the weather machine was plain as day open for destruction. \nSpring was upon them.\n" + 
	      "'Allow me,' A fallen warlock suggested, then with a mighty Banjo Riff that\n would shatter skulls he shredded until the machine was overloading, \noverheating, over-banjo'd." +
	      "The Professor dismayed at the events\n grumbled, 'I have to return to my peasant-box! Nooo.' Abruptly the professor\n, begins to melt..." +
          "The defeat of the professor and the destruction of the weather\n machine began to warm to air, melt the snow\n, and defeat the horrors outside. " +
          "SPRING HAS RETURNED." +
          "END: \n"
        + "The plants and trees had begun to grow their foliage. \nSpring has returned and with it the comfort of being\n outside. That is until the mulch gremlins came...";
        #endregion

        #region Save Data
        private bool loadGame = false; //Loaded game
        private string battleCompleted1; //If the first battle is completed
        private string battleCompleted2; //If the second battle is completed
        private string battleCompleted3; //If the third battle is completed
        #endregion
        #endregion

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            #region Resize Game Window
            graphics.PreferredBackBufferWidth = gameWidth; // set the width of the game screen to the value entered
            graphics.PreferredBackBufferHeight = gameHeight; // set the height of the game screen to the value entered
            graphics.ApplyChanges(); // without this command, the game window won't be resized
            #endregion

            gameState = MenuState.Main;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region Load Textures
            ritLogo = Content.Load<Texture2D>("RITRPGLogo");
            mainBackground = Content.Load<Texture2D>("christmas-snow-background");
            speechMenuBack = Content.Load<Texture2D>("SpeechMenu");
            newButtonFile = Content.Load<Texture2D>("NewGame");
            loadButtonFile = Content.Load<Texture2D>("LoadGame");
            continueButtonFile = Content.Load<Texture2D>("ContinueButton");
            selectorText = Content.Load<Texture2D>("Selector");
            crystalSnow = Content.Load<Texture2D>("CrystalSnow"); //Load the battle background
            gameOverTop = Content.Load<Texture2D>("GameOverTop"); //Load the game over top texture
            gameOverBottom = Content.Load<Texture2D>("GameOverBottom"); //Load the game over bottom texture
            iceCube = Content.Load<Texture2D>("IceCube"); //Load the ice cube for game over texture
            victory = Content.Load<Texture2D>("Victory"); //Load the victory texture
            orangeHall = Content.Load<Texture2D>("OrangeHall"); //Load the battle background
            eastman = Content.Load<Texture2D>("Eastman"); //Load the battle background
            gameFont = Content.Load<SpriteFont>("SpriteFont1");

            #region Characters
            sAGSprite = Content.Load<Texture2D>("KevinArt"); //Load the SmartassGuySprite
            lJGSprite = Content.Load<Texture2D>("CharlesArt"); //Load the LowJokesSprite
            wGSprite = Content.Load<Texture2D>("MannyArt"); //Load the WittyGuySprite
            cGSprite = Content.Load<Texture2D>("ChrisArt"); //Load the CoffeeGuySprite
            #endregion

            #region Enemies
            cPDemoSprite = Content.Load<Texture2D>("BrofessorFinal"); //Load the ProfessorSprite
            cMDemoSprite = Content.Load<Texture2D>("CodeMankey64x64"); //Load the CodeMonkeyGuySprite
            cTADemoSprite = Content.Load<Texture2D>("CodeT"); //Load the TASprite
            destlerSprite = Content.Load<Texture2D>("DestlerArtFinal"); //load the Boss Sprite
            fBossSprite = Content.Load<Texture2D>("FinalBossManFinal"); //load the Final Boss
            tankSprite = Content.Load<Texture2D>("Bierre'sTank"); //load the Tank
            #endregion
            #endregion

            #region Create Attributes
            newButton = new Button(graphics, spriteBatch, new Vector2((gameWidth/2) - (buttonWidth / 2), gameHeight - 175), newButtonFile, buttonWidth, buttonHeight);
            continueButton = new Button(graphics, spriteBatch, new Vector2((gameWidth - buttonWidth), gameHeight - 75), continueButtonFile, buttonWidth, buttonHeight);
            loadButton = new Button(graphics, spriteBatch, new Vector2((gameWidth / 2) - (buttonWidth / 2), newButton.Position.Y + buttonHeight + 20), loadButtonFile, buttonWidth, buttonHeight);
            titleScreen = new MainMenu(graphics, spriteBatch, mainBackground, ritLogo, new Vector2(0, 0), gameHeight, gameWidth, newButton, loadButton);
            bMenu = new BattleMenu(graphics, spriteBatch, gameFont, speechMenuBack, selectorText, new Vector2(0, gameHeight - bMenuHeight), bMenuHeight, gameWidth);

            #region Enemies
            cPDemo = new CodeProfessor(spriteBatch, graphics, cPDemoSprite, "Brofessor", 150, 10, 40, 10, 30, 20); //Codeprofessor object
            cMDemo = new CodeMonkey(spriteBatch, graphics, cMDemoSprite, "Mankey",75, 25, 5, 12, 40, 5); //Codemonkey object
            cTADemo = new CodeTA(spriteBatch, graphics, cTADemoSprite, "Sassman", 100, 35, 25, 5, 10, 10); //CodeTA object
            dest = new Destler(spriteBatch, graphics, destlerSprite, "Pres. Destler", 275, 40, 80, 5, 20, 70); //Destler obj
            fBoss = new FinalBoss(spriteBatch, graphics, fBossSprite, "Prof. Bierre", 250, 50, 100, 10, 25, 50); //Final boss obj
            tank = new TankCharacter(spriteBatch, graphics, tankSprite, "Tank", 150, 100, 5, 5, 20, 50); //tank
            #endregion

            tScreen = new TransitionScreen(graphics, spriteBatch, mainBackground, new Vector2(0, 0), gameHeight, gameWidth, chapter1, gameFont, continueButton, "The Beginning");
            tScreen2 = new TransitionScreen(graphics, spriteBatch, mainBackground, new Vector2(0, 0), gameHeight, gameWidth, chapter2, gameFont, continueButton, "The Middle Bit");
            tScreen3 = new TransitionScreen(graphics, spriteBatch, mainBackground, new Vector2(0, 0), gameHeight, gameWidth, chapter3, gameFont, continueButton, "The TRUE Villain");
            end = new TransitionScreen(graphics, spriteBatch, mainBackground, new Vector2(0, 0), gameHeight, gameWidth, chapter4, gameFont, continueButton, "THE END");
            #endregion

            #region Read the save file
            try //Try to read the file
            {
                StreamReader fileReader = new StreamReader("savedGame.bin"); //Creates a streamreader to read the file
                string line = null; //Stores the strings to be read from the file
                string[] saveData = new string[3]; //An array to hold the nine pieces of data from the file
                while ((line = fileReader.ReadLine()) != null) //While there is data to be read
                {
                    saveData = line.Split(','); //Tells the array where each piece of data ends
                    battleCompleted1 = saveData[0]; //The first battle
                    battleCompleted2 = saveData[1]; //The second battle
                    battleCompleted3 = saveData[2]; //The third battle
                }
                fileReader.Close(); //Closes the fileReader
            }
            catch (Exception) //If the save file cannot be read or does not exist
            {
                try //Write the save file
                {
                    StreamWriter fileWriter = new StreamWriter("savedGame.bin"); //Create the StreamWriter object
                    fileWriter.Write("false" + ","); //First battle completion
                    fileWriter.Write("false" + ","); //Second battle completion
                    fileWriter.Write("false"); //Third battle completion
                    fileWriter.Close(); //Close the StreamWriter
                }
                catch (Exception) //If the save file cannot be written
                {
                    battleCompleted1 = "false"; //First battle completion
                    battleCompleted2 = "false"; //Second battle completion
                    battleCompleted3 = "false"; //Third battle completion
                }
            }
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            IsMouseVisible = true; // mouse is always visible
            mState = Mouse.GetState();
            // set a teeny tiny rectangle to follow the mouse's cursor
            mouseCursor = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

            switch (gameState)
            {
                case MenuState.Main:
                    #region When buttons are pressed on the main menu
                    if (mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(newButton.ButtonBox)) //New Game
                    {
                        gameState = MenuState.Transition;
                        sAG = new SmartassGuy(spriteBatch, graphics, sAGSprite, "smartassGuyDefault.bin"); //SmartassGuy object
                        lJG = new LowJokesGuy(spriteBatch, graphics, lJGSprite, "lowjokesGuyDefault.bin"); //LowJokesGuy object
                        wG = new WittyGuy(spriteBatch, graphics, wGSprite, "wittyGuyDefault.bin"); //WittyGuy object
                        cG = new CoffeeGuy(spriteBatch, graphics, cGSprite, "coffeeGuyDefault.bin"); //CoffeeGuy object
                        sAG.ReadAttackFileName("Insults\\smartassGuyAttack1.bin", "Insults\\smartassGuyAttack2.bin", "Insults\\smartassGuyAttack3.bin", "Insults\\smartassGuyAttack4.bin");
                        lJG.ReadAttackFileName("Insults\\lowjokesGuyAttack1.bin", "Insults\\lowjokesGuyAttack2.bin", "Insults\\lowjokesGuyAttack3.bin", "Insults\\lowjokesGuyAttack4.bin");
                        wG.ReadAttackFileName("Insults\\wittyGuyAttack1.bin", "Insults\\wittyGuyAttack2.bin", "Insults\\wittyGuyAttack3.bin", "Insults\\wittyGuyAttack4.bin");
                        cG.ReadAttackFileName("Insults\\coffeeGuyAttack1.bin", "Insults\\coffeeGuyAttack2.bin", "Insults\\coffeeGuyAttack3.bin", "Insults\\coffeeGuyAttack4.bin");
                        dBattle = new DemoBattle(sAG, lJG, wG, cG, cPDemo, cTADemo, cMDemo, bMenu, graphics, spriteBatch, orangeHall, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //DemoBattle object                       
                        destlerBattle = new DestlerBossBattle(sAG, lJG, wG, cG, dest, bMenu, graphics, spriteBatch, eastman, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //DestlerBattle object
                        finalBattle = new FinalBossBattle(sAG, lJG, wG, cG, tank, fBoss, bMenu, graphics, spriteBatch, eastman, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //FinalBattle object
                    }
                    if (mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(loadButton.ButtonBox)) //Load Game
                    {
                        sAG = new SmartassGuy(spriteBatch, graphics, sAGSprite, "smartassGuy.bin"); //SmartassGuy object
                        lJG = new LowJokesGuy(spriteBatch, graphics, lJGSprite, "lowjokesGuy.bin"); //LowJokesGuy object
                        wG = new WittyGuy(spriteBatch, graphics, wGSprite, "wittyGuy.bin"); //WittyGuy object
                        cG = new CoffeeGuy(spriteBatch, graphics, cGSprite, "coffeeGuy.bin"); //CoffeeGuy object
                        sAG.ReadAttackFileName("Insults\\smartassGuyAttack1.bin", "Insults\\smartassGuyAttack2.bin", "Insults\\smartassGuyAttack3.bin", "Insults\\smartassGuyAttack4.bin");
                        lJG.ReadAttackFileName("Insults\\lowjokesGuyAttack1.bin", "Insults\\lowjokesGuyAttack2.bin", "Insults\\lowjokesGuyAttack3.bin", "Insults\\lowjokesGuyAttack4.bin");
                        wG.ReadAttackFileName("Insults\\wittyGuyAttack1.bin", "Insults\\wittyGuyAttack2.bin", "Insults\\wittyGuyAttack3.bin", "Insults\\wittyGuyAttack4.bin");
                        cG.ReadAttackFileName("Insults\\coffeeGuyAttack1.bin", "Insults\\coffeeGuyAttack2.bin", "Insults\\coffeeGuyAttack3.bin", "Insults\\coffeeGuyAttack4.bin");
                        dBattle = new DemoBattle(sAG, lJG, wG, cG, cPDemo, cTADemo, cMDemo, bMenu, graphics, spriteBatch, orangeHall, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //DemoBattle object
                        destlerBattle = new DestlerBossBattle(sAG, lJG, wG, cG, dest, bMenu, graphics, spriteBatch, eastman, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //DestlerBattle object
                        finalBattle = new FinalBossBattle(sAG, lJG, wG, cG, tank, fBoss, bMenu, graphics, spriteBatch, eastman, iceCube, victory, gameOverTop, gameOverBottom, crystalSnow, gameFont); //FinalBattle object
                        loadGame = true; //This is a loaded game

                        if (battleCompleted1 == "true" && battleCompleted2 == "false" && battleCompleted3 == "false")
                        {
                            gameState = MenuState.Transition2;
                        }

                        else if (battleCompleted1 == "true" && battleCompleted2 == "true" && battleCompleted3 == "false")
                        {
                            gameState = MenuState.Transition3;
                        }

                        else if (battleCompleted1 == "true" && battleCompleted2 == "true" && battleCompleted3 == "true")
                        {
                            gameState = MenuState.Stop;
                        }

                    }
                    break;
                    #endregion

                case MenuState.Transition:
                    // go to the next battle, currently the only battle
                    if(mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(continueButton.ButtonBox))
                    {
                        gameState = MenuState.Battle;
                    }
                    break;

                case MenuState.Battle:
                    dBattle.RunBattle();
                    if (dBattle.DisplayScreen == true || (battleCompleted1 == "true" && loadGame == true))
                    {
                        gameState = MenuState.Transition2;
                    }
                    break;

                case MenuState.Transition2:
                    if (mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(continueButton.ButtonBox))
                    {
                        gameState = MenuState.DestlerBattle;
                    }
                    break;

                case MenuState.DestlerBattle:
                    destlerBattle.RunBattle();
                    if (destlerBattle.DisplayScreen == true || (battleCompleted2 == "true" && loadGame == true))
                    {
                        gameState = MenuState.Transition3;
                    }
                    break;

                case MenuState.Transition3:
                    if (mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(continueButton.ButtonBox))
                    {
                        gameState = MenuState.FinalBattle;
                    }
                    break;

                case MenuState.FinalBattle:
                    finalBattle.RunBattle();
                    if (finalBattle.DisplayScreen == true || (battleCompleted3 == "true" && loadGame == true))
                    {
                        gameState = MenuState.Stop;
                    }
                    break;

                case MenuState.Stop:
                    if (mState.LeftButton == ButtonState.Pressed && mouseCursor.Intersects(continueButton.ButtonBox))
                    {
                        Environment.Exit(1);
                    }
                    break;

                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin(); //Begin drawing
            switch (gameState)
            {
                case MenuState.Main:
                    titleScreen.DrawMenu(); //Draw the title screen
                    break;

                case MenuState.Transition:
                    tScreen.DrawMenu(); //Draw the transition screen
                    break;

                case MenuState.Battle:
                    dBattle.DrawScreen(); //Draw the battle        
                    break;

                case MenuState.Transition2:
                    tScreen2.DrawMenu();
                    break;

                case MenuState.DestlerBattle:
                    destlerBattle.DrawScreen(); //Draw the battle        
                    break;

                case MenuState.Transition3:
                    tScreen3.DrawMenu();
                    break;

                case MenuState.FinalBattle:
                    finalBattle.DrawScreen(); //Draw the battle        
                    break;

                case MenuState.Stop:
                    end.DrawMenu();
                    break;
                default:
                    break;
            }
            spriteBatch.End(); //End drawing

            base.Draw(gameTime);
        }
    }
}
