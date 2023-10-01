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
        public float moveForce = 10f;

        //private float rotationSpeed;
        public float MaxRotationSpeed = 3.5f;
        //public float RotationAccel = 1f;

        private float maxVelocity = 5f;
        private float velocityDamping = 0.98f;

        CircleColliderComponent collider;
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
            collider.isTrigger = true;
            this.collider.DebugMode = true;

            this.AddComponent(collider);

            rb = new RigidBodyComponent("rbPlayer", app);
            rb.Mass = 0;
            this.AddComponent(rb);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //rb.Velocity = Vector3.Zero;
            Move(gameTime);

            //limit velocity
/*            if (rb.Velocity.Length() > maxVelocity)
            {
                rb.Velocity = (rb.Velocity / rb.Velocity.Length()) * maxVelocity;
            }*/
            //rb.Velocity *= velocityDamping;
            ILeviathanEngineService re = this.app.Services.GetService<ILeviathanEngineService>();
            re.SetCameraPosition(new Vector2(
                this.GetGlobalPosition().X + texture.Width/2,
                this.GetGlobalPosition().Y + texture.Height/2) - re.getWindowSize() / 2);

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
            Vector2 forceVector = directionVector * moveForce * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            rb.Velocity += new Vector3(forceVector, 0);
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
