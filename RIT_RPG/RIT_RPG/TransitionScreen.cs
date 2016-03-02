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
    class TransitionScreen:Menu
    {
        private SpriteFont font; // the font that will be used to display story text
        private Button button; // button that allows the player to advance to the next battle
        private string text; // the main text that the screen will write
        private string title; // slightly bigger text that will go on the top of the menu

        public TransitionScreen(GraphicsDeviceManager gr, SpriteBatch sp, Texture2D bak, Vector2 pos, int ht, int wd, string txt, SpriteFont ft, Button bt, string ttle)
            : base(gr, sp, bak, pos, ht, wd)
        {
            text = txt;
            font = ft;
            button = bt;
            title = ttle;
        }

        public override void DrawMenu()
        {
            base.DrawMenu();

            button.DrawButton();

            sprite.DrawString(font, text, new Vector2(15, 50), Color.Black);
            sprite.DrawString(font, title, new Vector2(10, 10), Color.Black);
        }
    }
}
