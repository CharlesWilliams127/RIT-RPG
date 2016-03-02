using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace RIT_RPG
{
    class BattleMenu : SmallMenu
    {
        private int selectorHeight = 40;
        private int selectorWidth;
        private int menuPages; // pages of the attack and items menu, 4 options per page
        private string itemType = ""; // the type of item used
        private bool attack; // determines if the attack action is selected
        private bool item; // determines if the item action is selected
        private bool switchR; // determines if the switch rows action is selected
        private bool runAway; // determines if the "run away" action is selected
        private bool pos1Sel;
        private bool pos2Sel;
        private bool pos3Sel;
        private bool pos4Sel;
        private bool characterSelected;
        private Texture2D selectorTex; // texture for the selector
        private Rectangle selector; // the selector, which will update in the draw menu constantly
        private Vector2 selectorPos; // the current position of the selector
        private Vector2 pos1; // position of attack label
        private Vector2 pos2; // position of item label
        private Vector2 pos3; // position of the switch rows label
        private Vector2 pos4; // position of the run away label
        private SpriteFont battleFont; // font for menu
        private Timer actionTimer; // timer for unfinished actions


        // the Battle state will decide how the menu will be controlled and drawn
        public enum BattleState { Main, Attack, Items, Switch, Run, EnemySelect, CharacterSelect }
        BattleState currentState = new BattleState();
        public BattleMenu(GraphicsDeviceManager gr, SpriteBatch sp, SpriteFont sf, Texture2D bak, Texture2D sel, Vector2 pos, int ht, int wd)
            : base(gr, sp, bak, pos, ht, wd)
        {
            selectorTex = sel;
            selectorWidth = (width / 2) - 30; // the width of the selector changes depending on the size of the game window
            battleFont = sf;

            // use complicated algorithms to set the positions of the label vectors
            pos1 = new Vector2(position.X + 20, position.Y + 60);
            pos2 = new Vector2((width - selectorWidth - 10), position.Y + 60);
            pos3 = new Vector2(position.X + 20, (position.Y + height) - (selectorHeight + 10));
            pos4 = new Vector2((width - selectorWidth - 10), (position.Y + height) - (selectorHeight + 10));

            currentState = BattleState.Main; // by default, the menu state is in the action selection menu first
            selectorPos = pos1; // by default, the selector is over the attack label first
            actionTimer = new Timer();
        }

        //called in the Game1's update class
        public void DrawBattleMenu()
        {
            base.DrawMenu();
        }

        //called in the Game1's update class - overloaded
        public void DrawBattleMenu(Character ch, List<Item> bg, List<Character> enem)
        {
            base.DrawMenu();

            if (ch != null)
            {
                switch (currentState)
                {
                    // this is drawn as the user is given a choice of actions to pick
                    case BattleState.Main:
                        // put together a string of the character passed into the method and its stats
                        string stats = ch.Name + ": Level " + ch.Level + " " + ch.CharacterClass + ", Ego: " + ch.EgoInBattle + ", Humor: " + ch.HumorInBattle + ", Intel: " + ch.IntelligenceInBattle + ", Wit: " + ch.WitInBattle;

                        // define the action selector, with offsets from the label vectors
                        selector = new Rectangle((int)selectorPos.X - 5, (int)selectorPos.Y - 5, selectorWidth, selectorHeight);
                        sprite.Draw(selectorTex, selector, Color.White);

                        // list out the current character and its stats
                        sprite.DrawString(battleFont, stats, new Vector2(position.X + 20, position.Y + 20), Color.White);

                        // draw the attack label
                        sprite.DrawString(battleFont, "Attack", pos1, Color.White);

                        // draw the item label
                        sprite.DrawString(battleFont, "Items", pos2, Color.White);

                        // draw the switch rows label
                        sprite.DrawString(battleFont, "Switch Rows", pos3, Color.White);

                        // draw the run label
                        sprite.DrawString(battleFont, "Run Away", pos4, Color.White);
                        break;
                    case BattleState.Attack:
                        // define the action selector, with offsets from the label vectors
                        selector = new Rectangle((int)selectorPos.X - 5, (int)selectorPos.Y - 5, selectorWidth, selectorHeight);
                        sprite.Draw(selectorTex, selector, Color.White);

                        // draw the attack label
                        sprite.DrawString(battleFont, ch.AttackString1, pos1, Color.White);

                        // draw the item label
                        sprite.DrawString(battleFont, ch.AttackString2, pos2, Color.White);

                        // draw the switch rows label
                        sprite.DrawString(battleFont, ch.AttackString3, pos3, Color.White);

                        // draw the run label
                        sprite.DrawString(battleFont, ch.AttackString4, pos4, Color.White);
                        break;
                    case BattleState.Items:
                        if (bg.Count != 0)
                        {
                            // define the action selector, with offsets from the label vectors
                            selector = new Rectangle((int)selectorPos.X - 5, (int)selectorPos.Y - 5, selectorWidth, selectorHeight);
                            sprite.Draw(selectorTex, selector, Color.White);

                            // draw the name of the first item
                            if (bg.Count >= 1)
                            {
                                sprite.DrawString(battleFont, bg[0].Name + ": x" + bg[0].Uses, pos1, Color.White);

                                if (selectorPos == pos1)
                                {
                                    sprite.DrawString(battleFont, bg[0].Description, new Vector2(position.X + 20, position.Y + 20), Color.White);
                                }
                            }

                            // draw the name of the second item
                            if (bg.Count >= 2)
                            {
                                sprite.DrawString(battleFont, bg[1].Name + ": x" + bg[1].Uses, pos2, Color.White);

                                if (selectorPos == pos2)
                                {
                                    sprite.DrawString(battleFont, bg[1].Description, new Vector2(position.X + 20, position.Y + 20), Color.White);
                                }
                            }

                            // draw the name of the third item
                            if (bg.Count >= 3)
                            {
                                sprite.DrawString(battleFont, bg[2].Name + ": x" + bg[2].Uses, pos3, Color.White);

                                if (selectorPos == pos3)
                                {
                                    sprite.DrawString(battleFont, bg[2].Description, new Vector2(position.X + 20, position.Y + 20), Color.White);
                                }
                            }

                            // draw the name of the fourth item
                            if (bg.Count >= 4)
                            {
                                sprite.DrawString(battleFont, bg[3].Name + ": x" + bg[3].Uses, pos4, Color.White);

                                if (selectorPos == pos4)
                                {
                                    sprite.DrawString(battleFont, bg[3].Description, new Vector2(position.X + 20, position.Y + 20), Color.White);
                                }
                            }

                        }
                        else
                        {
                            sprite.DrawString(battleFont, "No more items, press the 'Q' key to go back.", pos1, Color.White);
                        }
                        break;
                    case BattleState.Switch:
                        // define the action selector, with offsets from the label vectors
                        selector = new Rectangle((int)selectorPos.X - 5, (int)selectorPos.Y - 5, selectorWidth, selectorHeight);
                        sprite.Draw(selectorTex, selector, Color.White);

                        // draw the attack label
                        sprite.DrawString(battleFont, "Row 1", pos1, Color.White);

                        // draw the item label
                        sprite.DrawString(battleFont, "Row 2", pos2, Color.White);

                        // draw the switch rows label
                        sprite.DrawString(battleFont, "Row 3", pos3, Color.White);

                        // draw the run label
                        sprite.DrawString(battleFont, "Row 4", pos4, Color.White);
                        break;
                    case BattleState.Run:
                        // add running away code here.
                        sprite.DrawString(battleFont, "The party flees the battle!", pos1, Color.White);
                        break;
                    case BattleState.EnemySelect:
                        // draw instructions
                        sprite.DrawString(battleFont, "Use the number keys to select an enemy.", new Vector2(position.X + 20, position.Y + 20), Color.White);
                        // draw the name of the first enemy
                        if (enem.Count >= 1)
                        {
                            sprite.DrawString(battleFont, "1: " + enem[0].Name, pos1, Color.White);
                        }
                        // draw the name of the second enemy
                        if (enem.Count >= 2)
                        {
                            sprite.DrawString(battleFont, "2: " + enem[1].Name, pos2, Color.White);
                        }

                        // draw the name of the third enemy
                        if (enem.Count >= 3)
                        {
                            sprite.DrawString(battleFont, "3: " + enem[2].Name, pos3, Color.White);
                        }
                        break;

                    case BattleState.CharacterSelect:
                        selector = new Rectangle((int)selectorPos.X - 5, (int)selectorPos.Y - 5, selectorWidth, selectorHeight);
                        sprite.Draw(selectorTex, selector, Color.White);
                        //draw instructions
                        if (itemType == "Revive Item")
                        {
                            // draw the attack label
                            sprite.DrawString(battleFont, "Smartass Guy", pos1, Color.White);

                            // draw the item label
                            sprite.DrawString(battleFont, "Low Jokes Guy", pos2, Color.White);

                            // draw the switch rows label
                            sprite.DrawString(battleFont, "Witty Guy", pos3, Color.White);

                            // draw the run label
                            sprite.DrawString(battleFont, "Coffee Guy", pos4, Color.White);
                        }
                        break;
                }
            }
            else
            {
                this.DrawBattleMenu();
            }
        }

        // called in the Game1's update class
        public int ProcessInput(Character ch, List<Item> bg)
        {
            #region Code Key
            /*
             * 0: Attack option selected, first attack chosen
             * 1: Attack option selected, second attack chosen
             * 2: Attack option selected, third attack chosen
             * 3: Attack option selected, fourth attack chosen
             * 4: Item option selected, first item chosen
             * 5: Item option selected, second item chosen
             * 6: Item option selected, third item chosen
             * 7: Item option selected, fourth item chosen
             * 8: Switch Rows option selected, first row chosen
             * 9: Switch Rows option selected, second row chosen
             * 10: Switch Rows option selected, third row chosen
             * 11: Switch Rows option selected, fourth row chosen
             * 12: Run Away option selected
             * -1: returned when no selection has been made
             */
            #endregion

            if (ch != null)
            {
                currentKState = Keyboard.GetState();

                // these logic statements move the selector to the desired label based on the labels around it
                if ((selectorPos == pos3 && currentKState.IsKeyDown(Keys.W) == true && prevKState.IsKeyUp(Keys.W)) || (selectorPos == pos2 && currentKState.IsKeyDown(Keys.A) == true && prevKState.IsKeyUp(Keys.A)))
                {
                    selectorPos = pos1;
                }

                else if ((selectorPos == pos1 && currentKState.IsKeyDown(Keys.D) == true && prevKState.IsKeyUp(Keys.D)) || (selectorPos == pos4 && currentKState.IsKeyDown(Keys.W) == true && prevKState.IsKeyUp(Keys.W)))
                {
                    selectorPos = pos2;
                }

                else if ((selectorPos == pos1 && currentKState.IsKeyDown(Keys.S) == true && prevKState.IsKeyUp(Keys.S)) || (selectorPos == pos4 && currentKState.IsKeyDown(Keys.A) == true && prevKState.IsKeyUp(Keys.A)))
                {
                    selectorPos = pos3;
                }

                else if ((selectorPos == pos3 && currentKState.IsKeyDown(Keys.D) == true && prevKState.IsKeyUp(Keys.D)) || (selectorPos == pos2 && currentKState.IsKeyDown(Keys.S) == true && prevKState.IsKeyUp(Keys.S)))
                {
                    selectorPos = pos4;
                }

                switch (currentState)
                {
                    #region Main
                    case BattleState.Main:
                        /* set all choice booleans to false
                        attack = false;
                        item = false;
                        switchR = false;
                        runAway = false;
                        pos1Sel = false;
                        pos2Sel = false;
                        pos3Sel = false;
                        pos4Sel = false;
                        */

                        // selecting the attack action
                        if (selectorPos == pos1 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.Attack;
                        }

                        // selecting the items action
                        if (selectorPos == pos2 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.Items;
                        }

                        // selecting the switch rows action
                        if (selectorPos == pos3 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.Switch;
                        }

                        // selecting the run away action
                        if (selectorPos == pos4 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.Run;
                        }
                        break;
                    #endregion

                    #region Attack
                    case BattleState.Attack:
                        // add the attack selection menu code here
                        attack = true;
                        // selecting the attack action
                        if (selectorPos == pos1 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.EnemySelect;
                            return 0;
                        }

                        // selecting the items action
                        if (selectorPos == pos2 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.EnemySelect;
                            return 1;
                        }

                        // selecting the switch rows action
                        if (selectorPos == pos3 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.EnemySelect;
                            return 2;
                        }

                        // selecting the run away action
                        if (selectorPos == pos4 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            currentState = BattleState.EnemySelect;
                            return 3;
                        }

                        if (currentKState.IsKeyDown(Keys.Q) == true && prevKState.IsKeyUp(Keys.Q) == true) // if the Q key is pressed at any time
                        {
                            currentState = BattleState.Main;
                        }
                        break;
                    #endregion

                    #region Items
                    case BattleState.Items:
                        if (bg.Count != 0)
                        {
                            // selecting the first item
                            if (selectorPos == pos1 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true && bg.Count >= 1)
                            {
                                return 4;
                            }

                            // selecting the second item
                            if (selectorPos == pos2 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true && bg.Count >= 2)
                            {
                                return 5;
                            }

                            // selecting the third item
                            if (selectorPos == pos3 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true && bg.Count >= 3)
                            {
                                return 6;
                            }

                            // selecting the fourth item
                            if (selectorPos == pos4 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true && bg.Count >= 4)
                            {
                                return 7;
                            }
                        }

                        if (currentKState.IsKeyDown(Keys.Q) == true && prevKState.IsKeyUp(Keys.Q) == true) // if the Q key is pressed at any time
                        {
                            currentState = BattleState.Main;
                        }
                        break;
                    #endregion

                    #region Switch
                    case BattleState.Switch:
                        switchR = true;
                        // selecting the first row
                        if (selectorPos == pos1 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            return 8;
                        }

                        // selecting the second row
                        if (selectorPos == pos2 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            return 9;
                        }

                        // selecting the third row
                        if (selectorPos == pos3 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            return 10;
                        }

                        // selecting the fourth row
                        if (selectorPos == pos4 && currentKState.IsKeyDown(Keys.E) == true && prevKState.IsKeyUp(Keys.E) == true)
                        {
                            return 11;
                        }

                        if (currentKState.IsKeyDown(Keys.Q) == true && prevKState.IsKeyUp(Keys.Q) == true) // if the Q key is pressed at any time
                        {
                            currentState = BattleState.Main;
                        }
                        break;
                    #endregion

                    #region Run
                    case BattleState.Run:
                        // add running away code here.
                        bool timeElapsed2 = actionTimer.Freeze(120);
                        if (timeElapsed2 == true)
                        {
                            runAway = true;
                            currentState = BattleState.Main;
                            return 12;
                        }
                        break;
                    #endregion

                    #region Enemy Select
                    case BattleState.EnemySelect:
                        if (currentKState.IsKeyDown(Keys.Q) == true && prevKState.IsKeyUp(Keys.Q) == true) // if the Q key is pressed at any time
                        {
                            currentState = BattleState.Attack;
                        }
                        break;
                    #endregion
                }

            }
            prevKState = currentKState;
            return -1;
        }

        // resets the menu to the default state
        public void ResetMenu()
        {
            currentState = BattleState.Main;
        }

        #region Properties
        public bool Attack
        {
            get { return attack; }
        }
        public bool Item
        {
            get { return item; }
        }
        public bool SwitchR
        {
            get { return switchR; }
        }
        public bool RunAway
        {
            get { return runAway; }
        }
        public bool Pos1Sel
        {
            get { return pos1Sel; }
            set { pos1Sel = value; }
        }
        public bool Pos2Sel
        {
            get { return pos2Sel; }
            set { pos2Sel = value; }
        }
        public bool Pos3Sel
        {
            get { return pos3Sel; }
            set { pos3Sel = value; }
        }
        public bool Pos4Sel
        {
            get { return pos4Sel; }
            set { pos4Sel = value; }
        }
        public bool CharacterSelected
        {
            get { return characterSelected; }
            set { characterSelected = value; }
        }
        #endregion
    }
}
