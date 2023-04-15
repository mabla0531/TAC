using SFML.Graphics;
using SFML.System;
using System;

namespace TAC {
    abstract class Entity {
        public float X { get; set; }
        public float Y { get; set; }

        protected FloatRect collisionBounds;
        public bool Hovered {get; set;} = false;

        public int MaxHealth {get; set;}
        public int Health {get; set;}
        public float DisplayHealth {get; set;}
        public bool IsKillable {get; set;}

        public IntRect DefaultTextureRect {get; set;}
        public Sprite EntitySprite {get; set;}

        public Inventory inventory {get; set;} //I would love to capitalize it, I really would, but unfortunately my hands are tied


        public delegate void Tick();
        public Tick tick;
        public delegate void Render(RenderWindow window);
        public Render render;

        public Entity() {
            collisionBounds.Left   = 0;
            collisionBounds.Top    = 0;
            collisionBounds.Width  = 32;
            collisionBounds.Height = 32;

            IsKillable = false;

            inventory = new Inventory();

            tick += () => {
                Hovered = false;
                if (new IntRect((int)(X - Handler.gameState.gameCameraOffset.X), (int)(Y - Handler.gameState.gameCameraOffset.Y), EntitySprite.TextureRect.Width, EntitySprite.TextureRect.Height).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY))
                    Hovered = true;
            };

            render += (RenderWindow window) => {
                EntitySprite.Position = new Vector2f((int)(X - Handler.gameState.gameCameraOffset.X), (int)(Y - Handler.gameState.gameCameraOffset.Y));
                VertexArray shadow = new VertexArray(PrimitiveType.Quads, 4);

                byte shadowTransparency = (byte)(128 - System.Math.Abs(128.0f * Handler.gameState.TimeofDay));
                if (Math.Abs(Handler.gameState.TimeofDay) > 1.0f)
                    shadowTransparency = 0;

                shadow[0] = new Vertex(new Vector2f(Handler.gameState.TimeofDay * EntitySprite.TextureRect.Width, 0.0f), new Color(0, 0, 0, shadowTransparency), new Vector2f(EntitySprite.TextureRect.Left, EntitySprite.TextureRect.Top));
                shadow[1] = new Vertex(new Vector2f(EntitySprite.TextureRect.Width + (Handler.gameState.TimeofDay * EntitySprite.TextureRect.Width), 0.0f), new Color(0, 0, 0,shadowTransparency), new Vector2f(EntitySprite.TextureRect.Left + EntitySprite.TextureRect.Width, EntitySprite.TextureRect.Top));
                shadow[2] = new Vertex(new Vector2f(EntitySprite.TextureRect.Width, EntitySprite.TextureRect.Height), new Color(0, 0, 0, shadowTransparency), new Vector2f(EntitySprite.TextureRect.Left + EntitySprite.TextureRect.Width, EntitySprite.TextureRect.Top + EntitySprite.TextureRect.Height));
                shadow[3] = new Vertex(new Vector2f(0.0f, EntitySprite.TextureRect.Height), new Color(0, 0, 0, shadowTransparency), new Vector2f(EntitySprite.TextureRect.Left, EntitySprite.TextureRect.Top + EntitySprite.TextureRect.Height));
                
                RenderStates renderStates = new RenderStates(EntitySprite.Texture);
                
                renderStates.Transform.Translate(EntitySprite.Position);
                shadow.Draw(window, renderStates);

                window.Draw(EntitySprite);
            };
        }

        public FloatRect getCollisionBounds() {
            return new FloatRect(collisionBounds.Left + X, collisionBounds.Top + Y, collisionBounds.Width, collisionBounds.Height);
        }
    }
}
