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
        public float Speed = 5f;
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
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rb.Velocity = Vector3.Zero;
            Move();

            //ILeviathanEngineService re = this.app.Services.GetService<ILeviathanEngineService>();
            //re.SetCameraPosition(new Vector2(this.GetGlobalPosition().X, this.GetGlobalPosition().Y) - re.getWindowSize() / 2);

        }

        private void RotateLeft()
        {
            PrintLn("curr rotation: " + Rotation.ToString());
            Rotation -= 0.1f;
            PrintLn("curr rotation: " + Rotation.ToString());
            src.Rotation = Rotation;
            
        }
        private void RotateRight()
        {
            PrintLn("curr rotation: " + Rotation.ToString());
            Rotation += 0.1f;
            PrintLn("curr rotation: " + Rotation.ToString());
            src.Rotation = Rotation;
        }

        private void MoveInForwardDirection()
        {
            float x = MathF.Sin(Rotation);
            float y = -MathF.Cos(Rotation);
            Vector2 directionVector = new Vector2(x, y);
            Vector2 motionVector = directionVector * Speed;
            PrintLn(motionVector.ToString());
            rb.AddVelocityForward(motionVector);
        }


        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                MoveInForwardDirection();
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                RotateLeft();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                RotateRight();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                rb.Velocity.Y += Speed;
            }
        }
    }
}
