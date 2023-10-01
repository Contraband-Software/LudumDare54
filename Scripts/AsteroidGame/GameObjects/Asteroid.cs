namespace LD54.Scripts.AsteroidGame.GameObjects
{
    using LD54.AsteroidGame.GameObjects;
    using LD54.Engine.Collision;
    using LD54.Engine.Components;
    using LD54.Engine.Leviathan;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public  class Asteroid : GameObject
    {
        Texture2D texture;
        Texture2D brokenTexture;

        public float rotationSpeed;

        CircleColliderComponent collider;
        RigidBodyComponent rb;
        SpriteRendererComponent src;

        private enum State { ALIVE, DYING};
        State state = State.ALIVE;

        float scale;

        public Asteroid(Texture2D texture, Texture2D brokenText, string name, Game appCtx) : base(name, appCtx)
        {
            this.texture = texture;
            Random rnd = new Random();
            this.scale = 0.3f + (rnd.NextSingle() * 0.7f);

            rotationSpeed = (0.1f + (rnd.NextSingle() * 0.8f)) * (MathF.Sign(rnd.NextSingle()-0.5f));
        }

        public override void OnLoad(GameObject? parentObject)
        {
            src = new SpriteRendererComponent("asteroid", this.app);
            src.LoadSpriteData(
                this.GetGlobalTransform(),
                new Vector2((this.texture.Width * this.scale), (this.texture.Height * this.scale)),
                this.texture
                );
            this.AddComponent(src);

            Vector3 colliderDimensions = new Vector3(
                this.texture.Width * this.scale,
                this.texture.Height * this.scale,
                0);


            PrintLn(this.rotationSpeed.ToString());
            collider = new CircleColliderComponent(colliderDimensions.X / 2, Vector3.Zero, "asteroidCollider", this.app);
            collider.isTrigger = true;
            this.collider.DebugMode = true;

            this.AddComponent(collider);

            rb = new RigidBodyComponent("rbAsteroid", app);
            this.AddComponent(rb);
            rb.Velocity = new Vector3(1, 1, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(state == State.ALIVE)
            {
                Rotation += rotationSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
                src.Rotation = Rotation;
            }
            if(state == State.DYING)
            {

            }
        }

        /// <summary>
        /// goes into DYING state and changes to "blown up" version of sprite
        /// </summary>
        public void StartDeathCountdown()
        {
            if(state == State.ALIVE)
            {
                PrintLn("starting DEATH sequence for asteroid");
                state = State.DYING;
                rotationSpeed = 0;
            }    
        }
    }
}
