namespace LD54.Scripts.AsteroidGame.GameObjects
{
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

    public class Spaceship : GameObject
    {
        Texture2D texture;
        public float moveForce = 30f;

        private float rotationSpeed;
        public float MaxRotationSpeed = 3.5f;
        public float RotationAccel = 1f;

        ColliderComponent collider;
        RigidBodyComponent rb;
        SpriteRendererComponent src;

        public Spaceship(Texture2D texture, string name, Game appCtx) : base(name, appCtx)
        {
            this.texture = texture;

            Matrix pos = this.GetLocalTransform();
            pos.Translation = new Vector3(150, 150, 1);

            this.SetLocalTransform(pos);
        }

        public override void OnLoad(GameObject? parentObject)
        {
            float scaleDivider = 1;

            src = new SpriteRendererComponent("spaceship", this.app);
            src.LoadSpriteData(
                this.GetGlobalTransform(),
                new Vector2((this.texture.Width / scaleDivider), (this.texture.Height / scaleDivider)),
                this.texture,
                null);
            this.AddComponent(src);

            Vector3 colliderDimensions = new Vector3(this.texture.Width, this.texture.Height, 0);
            collider = new CircleColliderComponent(colliderDimensions.X/2, Vector3.Zero, "playerCollider", this.app);
            this.AddComponent(collider);

            rb = new RigidBodyComponent("rbPlayer", app);
            this.AddComponent(rb);

            rb.SetMaxVelocity(5f);
            rb.SetDampingFactor(0.98f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //rb.Velocity = Vector3.Zero;
            Move(gameTime);

            //ILeviathanEngineService re = this.app.Services.GetService<ILeviathanEngineService>();
            //re.SetCameraPosition(new Vector2(this.GetGlobalPosition().X, this.GetGlobalPosition().Y) - re.getWindowSize() / 2);

        }

        private void RotateLeft(GameTime gameTime)
        {
            Rotation -= MaxRotationSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            src.Rotation = Rotation;
            
        }
        private void RotateRight(GameTime gameTime)
        {
            Rotation += MaxRotationSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            src.Rotation = Rotation;
        }

        /// <summary>
        /// Calculate and return the direction this object is facing in
        /// </summary>
        /// <returns>direction as a unit vector</returns>
        private Vector2 forwardVector()
        {
            float x = MathF.Sin(Rotation);
            float y = -MathF.Cos(Rotation);
            Vector2 directionVector = new Vector2(x, y);
            return directionVector;
        }

        private void MoveInForwardDirection(GameTime gameTime)
        {
            Vector2 directionVector = forwardVector();
            Vector2 forceVector = directionVector * moveForce;
            rb.AddForce(forceVector, gameTime);
        }


        private void Move(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                MoveInForwardDirection(gameTime);
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                RotateLeft(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                RotateRight(gameTime);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //rb.Velocity.Y += Speed;
            }
        }
    }
}
