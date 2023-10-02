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
        float timeElapsed;
        UITextComponent scoreText;
        UITextComponent gameOverText;
        UITextComponent finalScore;
        List<SpriteFont> fonts;
        private enum UIState { PLAYER_ALIVE, PLAYER_DEAD};
        private UIState state = UIState.PLAYER_ALIVE;
        public GameUIContainer(List<SpriteFont> gameUI, string name, Game appCtx) : base(name, appCtx)
        {
            fonts = gameUI;
            scoreText = new UITextComponent("ui", app);
            scoreText.LoadTextElementData(
                app,
                this.GetGlobalTransform(),
                new Vector2(1, 1),
                "SCORE SCORE",
                fonts[0],
                new Color(255, 255, 255));
            this.AddComponent(scoreText);
            scoreText.PositionXAtRightEdge(new Vector2(-20, 10));
        }

        public override void OnLoad(GameObject? parentObject)
        {
            this.SetLocalPosition(new Vector2(0,0));



            timeElapsed = 0;
            state = UIState.PLAYER_ALIVE;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(state == UIState.PLAYER_ALIVE)
            {
                UpdateTimer(gameTime);
            }
        }

        public void OnGameOver()
        {
            if(state == UIState.PLAYER_DEAD)
            {
                return;
            }
            PrintLn("GAAAAAAME OVERRRRRRRRRRRRRRRRR");
            state = UIState.PLAYER_DEAD;

            gameOverText = new UITextComponent("ui", app);
            gameOverText.LoadTextElementData(
                app,
                this.GetGlobalTransform(),
                new Vector2(1, 1),
                "GAME OVER",
                fonts[1],
                Color.Red,
                true);
            this.AddComponent(gameOverText);
            gameOverText.PositionXAtScreenCentre();
            gameOverText.PositionYAtScreenCentre(new Vector2(0,0));

            //========================================

            finalScore = new UITextComponent("ui", app);
            finalScore.LoadTextElementData(
                app,
                this.GetGlobalTransform(),
                new Vector2(1, 1),
                "FINAL SCORE",
                fonts[0],
                Color.White,
                true);
            this.AddComponent(finalScore);

            int totalSeconds = (int)MathF.Round(timeElapsed);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string minutesText = (minutes > 9) ? minutes.ToString() : "0" + minutes.ToString();
            string secondsText = (seconds > 9) ? seconds.ToString() : "0" + seconds.ToString();

            if (minutes > 0)
            {
                finalScore.SetText("Time Survived= " + minutesText + ":" + secondsText);
            }
            else
            {
                finalScore.SetText("Time Survived= 00:" + secondsText);
            }
            finalScore.PositionXAtScreenCentre();
            finalScore.PositionYAtScreenCentre(new Vector2(0,100));

            //============================
            UITextComponent restartText = new UITextComponent("ui", app);
            restartText.LoadTextElementData(
                app,
                this.GetGlobalTransform(),
                new Vector2(1, 1),
                "PRESS [R] TO [R]ESTART",
                fonts[0],
                Color.White,
                true);
            this.AddComponent(restartText);
            restartText.PositionXAtScreenCentre();
            restartText.PositionYAtScreenCentre(new Vector2(0, 200));
        }

        private void UpdateTimer(GameTime gameTime)
        {
            //update time score thingy
            //reposition text sprite to be on right top corner
            timeElapsed += (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            int totalSeconds = (int)MathF.Round(timeElapsed);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            string minutesText = (minutes > 9) ? minutes.ToString() : "0" + minutes.ToString();
            string secondsText = (seconds > 9) ? seconds.ToString() : "0" + seconds.ToString();


            if (minutes > 0)
            {
                scoreText.SetText("T=t+ " + minutesText + ":" + secondsText);
            }
            else
            {
                scoreText.SetText("T=t+ 00:" + secondsText);
            }

            scoreText.PositionXAtRightEdge(new Vector2(-20, 10));
        }
    }
}
