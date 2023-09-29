using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Leviathan.Content;
using Microsoft.Xna.Framework.Content;
using static System.Net.Mime.MediaTypeNames;

namespace Leviathan.Content
{
    internal class LeviathanEngine
    {
        
        private Vector2[] lightPositions = new Vector2[64];
        private Vector3[] lightColors = new Vector3[64];
        private Queue<int> openLocations = new Queue<int>();
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Effect lightingShader;
        private Game game;

        RenderTarget2D colorTarget;
        RenderTarget2D normalTarget;
        RenderTarget2D litTarget;

        public List<Sprite> sprites = new List<Sprite>();

        public LeviathanEngine(Game g)
        {
            game = g;
            graphics = new GraphicsDeviceManager(game);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            for (int i = 0; i < 64; i++)
            {
                openLocations.Enqueue(i);
            }

        }

        public void initialize()
        {
            colorTarget = new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            normalTarget = new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            litTarget = new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public int addSprite(Sprite sprite)
        {
            sprites.Add(sprite);
            return sprites.IndexOf(sprite);
        }
        public void removeSprite(int index)
        {
            sprites.RemoveAt(index);
        }

        public void loadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            lightingShader = game.Content.Load<Effect>("lighting");
        }

        public void draw(GameTime gameTime) {
            Matrix view = Matrix.Identity*Matrix.CreateTranslation(0,0,0);

            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);

            //lightingShader.Parameters["view"]?.SetValue(view);
            //lightingShader.Parameters["projection"]?.SetValue(projection);
            Vector3 translation;
            view.Decompose(out _, out _, out translation);

            game.GraphicsDevice.SetRenderTarget(colorTarget);
            game.GraphicsDevice.Clear(Color.Black); ;
            spriteBatch.Begin(transformMatrix: view);

            foreach (Sprite sprite in sprites) 
            {
                spriteBatch.Draw(sprite.color,new Rectangle(sprite.getPositionXY().ToPoint(),sprite.size), Color.White);
            }

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(normalTarget);
            game.GraphicsDevice.Clear(new Color(0.5f, 0.5f, 1f));
            spriteBatch.Begin(transformMatrix: view);

            foreach (Sprite sprite in sprites)
            {
                if (sprite.useNormal)
                {
                    spriteBatch.Draw(sprite.normal, new Rectangle(sprite.getPositionXY().ToPoint(), sprite.size), Color.White);
                }
            }

            spriteBatch.End();

            lightingShader.Parameters["translation"]?.SetValue(new Vector2(translation.X, translation.Y));
            lightingShader.Parameters["viewProjection"]?.SetValue(projection);
            lightingShader.Parameters["time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            lightingShader.Parameters["width"]?.SetValue(width);
            lightingShader.Parameters["height"]?.SetValue(height);
            lightingShader.Parameters["lightColors"]?.SetValue(lightColors);
            lightingShader.Parameters["lightPositions"]?.SetValue(lightPositions);
            lightingShader.Parameters["normalSampler"]?.SetValue(normalTarget);


            game.GraphicsDevice.SetRenderTarget(null);
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(effect: lightingShader);
            spriteBatch.Draw(colorTarget, new Vector2(0), Color.White);
            spriteBatch.End();
        }

        //public override void Update(GameTime gameTime)
        //{

        //}

        public int AddLight(Vector2 position, Vector3 color)
        {
            int location = openLocations.Dequeue();
            lightPositions[location] = position;
            lightColors[location] = color;
            return location;
        }
        public void removeLight(int id)
        {
            lightColors[id] = new Vector3(0);
            openLocations.Enqueue(id);
        }

        public void setLightColor(int id,  Color color) {
            lightColors[id] = color.ToVector3();
        }
        public void setLightPosition(int id, Vector2 position)
        {
            lightPositions[id] = position;
        }
        public Vector3 getLightColor(int id)
        {
            return lightColors[id];
        }
        public Vector2 getLightPosition(int id)
        {
            return lightPositions[id];
        }
        public void updateLightPosition(int id, Vector2 offset)
        {
            lightPositions[id] += offset;
        }
    }
}
