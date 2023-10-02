namespace LD54.AsteroidGame.GameObjects
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
    using LD54.AsteroidGame.GameObjects;

    public class Spaceship : GameObject
    {
        BlackHole blackHole;
        private ILeviathanEngineService renderer;

        Texture2D texture;

        CircleColliderComponent collider;
        RigidBodyComponent rb;
        SpriteRendererComponent src;

        public float moveForce = 30f;
        public float MaxRotationSpeed = 3.5f;
        private float velocityDamping = 0.98f;

        private float maxVelocityFactor = 8f;

        private const float forceConstant = 0.001f;
        private const float boostFactor = 0.4f;
        private const float fallFactor = 14f;

        public Spaceship(BlackHole blackHole, Texture2D texture, string name, Game appCtx) : base(name, appCtx)
        {
            this.blackHole = blackHole;
            this.texture = texture;
        }

        public override void OnLoad(GameObject? parentObject)
        {
            renderer = this.app.Services.GetService<ILeviathanEngineService>();

            float scaleDivider = 1;

            src = new SpriteRendererComponent("spaceship", this.app);
            src.LoadSpriteData(
                this.GetGlobalTransform(),
                new Vector2((this.texture.Width / scaleDivider), (this.texture.Height / scaleDivider)),
                this.texture
                );
            this.AddComponent(src);

            Vector3 colliderDimensions = new Vector3(this.texture.Width, this.texture.Height, 0);
            collider = new CircleColliderComponent(colliderDimensions.X/2, Vector3.Zero, "playerCollider", this.app);
            collider.isTrigger = true;
            this.collider.DebugMode = true;
            this.AddComponent(collider);

            rb = new RigidBodyComponent("rbPlayer", app);
            rb.Mass = 0;
            rb.Velocity += new Vector3(0, 0, 0);
            this.AddComponent(rb);
        }

        public override void Update(GameTime gameTime)
        {
            #region TOY_ORBIT_PHYSICS
            float distanceToBlackHole = 0;
            {
                var render = this.app.Services.GetService<ILeviathanEngineService>();

                Vector3 blackHolePosition = blackHole.GetGlobalPosition();
                Vector3 shipPosition = this.GetGlobalPosition();

                // Distance to black hole
                Vector2 positionDelta = new Vector2(
                    blackHolePosition.X - shipPosition.X,
                    blackHolePosition.Y - shipPosition.Y
                );
                float r = positionDelta.Magnitude();
                distanceToBlackHole = r;

                Vector2 orbitTangent = positionDelta.PerpendicularCounterClockwise();
                float tangentVelocity = Vector2.Dot(this.rb.Velocity.SwizzleXY(), orbitTangent) / orbitTangent.Length();

                Vector2 orbitNormal = positionDelta.RNormalize();
                Vector3 accelerationToCenter = new Vector3(
                    orbitNormal * MathF.Abs(tangentVelocity) * r * (gameTime.ElapsedGameTime.Milliseconds / 1000f), 0) * forceConstant;

                orbitTangent.Normalize();
                Vector3 finalAbsoluteVelocity = accelerationToCenter + new Vector3((tangentVelocity) * orbitTangent, 0);

                #region ROTATION
                {
                    Vector2 positionC = this.GetGlobalPosition().SwizzleXY();
                    Vector2 positionB = positionC + finalAbsoluteVelocity.SwizzleXY();
                    Vector2 positionA = blackHolePosition.SwizzleXY();

                    float a = finalAbsoluteVelocity.SwizzleXY().Length();
                    float b = r;
                    float c = r;

                    float totalRotation = MathF.Acos(
                        (b * b + c * c - (a * a)) / (2 * b * c)
                    );

                    this.Rotation += totalRotation * MathF.Sign(tangentVelocity) * -1;
                }
                #endregion

                // More speed = more radius increase per frame
                float speedAbs = this.rb.Velocity.Copy().Length();
                finalAbsoluteVelocity += new Vector3(-orbitNormal, 0) * boostFactor * speedAbs;

                // Always being pulled in
                finalAbsoluteVelocity += new Vector3(orbitNormal, 0) * fallFactor * (1 / (speedAbs < 0.1f ? 0.0000001f : MathF.Pow(speedAbs, 0.9f)));
                this.rb.Velocity = finalAbsoluteVelocity;

                // Debug stuff, no more math after here
                Vector2 overlapOffset = new Vector2(1, 1) * 10;
                float velScale = 50;
                render.DebugDrawCircle(new Vector2(blackHolePosition.X, blackHolePosition.Y), r, Color.Lime);

                render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY(), this.GetGlobalPosition().SwizzleXY() + positionDelta, Color.Pink);

                render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY(), this.GetGlobalPosition().SwizzleXY() + accelerationToCenter.SwizzleXY(), Color.Lime);
                render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY(), this.GetGlobalPosition().SwizzleXY() + finalAbsoluteVelocity.SwizzleXY(), Color.Pink);
                render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY(), this.GetGlobalPosition().SwizzleXY() + tangentVelocity * velScale * orbitTangent, Color.Cyan);
                // render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY(), this.GetGlobalPosition().SwizzleXY() + tangent, Color.Cyan);
                // render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY() + overlapOffset, this.GetGlobalPosition().SwizzleXY() + overlapOffset + positionDelta * 140, Color.Lime);
                render.DebugDrawLine(this.GetGlobalPosition().SwizzleXY() + overlapOffset * 2, this.GetGlobalPosition().SwizzleXY() + overlapOffset * 2 + this.rb.Velocity.SwizzleXY() * velScale, Color.Yellow);

            }
            #endregion

            Move(gameTime);

            //limit velocity
            if (rb.Velocity.Length() > this.maxVelocityFactor)//(1 - 1 / (d == 0 ? 1 : d)) *
            {
                rb.Velocity = (rb.Velocity / rb.Velocity.Length()) * this.maxVelocityFactor;
            }

            renderer.SetCameraPosition(
                new Vector2(
                    this.GetGlobalPosition().X + texture.Width / 2,
                    this.GetGlobalPosition().Y + texture.Height / 2
                    ) - renderer.getWindowSize() / 2
                );

            base.Update(gameTime);
            src.Rotation = Rotation;
        }

        private void RotateLeft(GameTime gameTime)
        {
            Rotation -= MaxRotationSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
        }
        private void RotateRight(GameTime gameTime)
        {
            Rotation += MaxRotationSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
        }

        /// <summary>
        /// Calculate and return the direction this object is facing in
        /// </summary>
        /// <returns>direction as a unit vector</returns>
        private Vector2 ForwardVector()
        {
            float x = MathF.Sin(Rotation);
            float y = -MathF.Cos(Rotation);
            Vector2 directionVector = new Vector2(x, y);
            return directionVector;
        }

        private void MoveInForwardDirection(GameTime gameTime)
        {
            Vector2 directionVector = ForwardVector();
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
            // if (Keyboard.GetState().IsKeyDown(Keys.S))
            // {
            //     //rb.Velocity.Y += Speed;
            // }
        }
    }
}
