namespace LD54.Scripts.AsteroidGame.GameObjects
{
    using LD54.Scripts.Engine.Components;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameUIContainer : GameObject
    {
        SpriteFont gameUIFont;
        float timeElapsed;
        UITextComponent scoreText;
        public GameUIContainer(SpriteFont gameUI, string name, Game appCtx) : base(name, appCtx)
        {
            gameUIFont = gameUI;
        }

        public override void OnLoad(GameObject? parentObject)
        {
            this.SetLocalPosition(new Vector2(0,0));

            //SpriteFont font = new SpriteFont()

            scoreText = new UITextComponent("ui", app);
            scoreText.LoadTextElementData(
                app,
                this.GetGlobalTransform(),
                new Vector2(3, 3),
                "SCORE SCORE",
                gameUIFont,
                new Color(255, 255, 255));
            this.AddComponent(scoreText);

            scoreText.PositionXAtRightEdge(new Vector2(-20, 10));
            timeElapsed = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //update time score thingy
            //reposition text sprite to be on right top corner
            timeElapsed += (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            scoreText.SetText("T=t+ " + MathF.Round(timeElapsed).ToString());
            scoreText.PositionXAtRightEdge(new Vector2(-20, 10));
        }
    }
}
