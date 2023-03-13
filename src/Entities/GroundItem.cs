using SFML.Graphics;
using SFML.System;

namespace TAC {
    class GroundItem : Entity {

        public Item ItemReference {get; set;}
        public bool PickedUp {get; set;} = false;

        private RectangleShape tooltipBackground;
        private Text tooltipText;

        public GroundItem(Item itemRef, float x, float y) : base() {
            ItemReference = itemRef;

            MaxHealth = 1;
            Health = 1;
            DisplayHealth = 1;

            X = x;
            Y = y;

            EntitySprite = new Sprite(Assets.items);
            EntitySprite.TextureRect = itemRef.Icon.TextureRect;

            tooltipBackground = new RectangleShape(new Vector2f(96.0f, 48.0f));
            tooltipBackground.FillColor = new Color(36, 58, 71);
            tooltipBackground.OutlineColor = Color.Black;
            tooltipBackground.OutlineThickness = 1.0f;

            tooltipText = new Text(ItemReference.Name + "\nValue: " + ItemReference.Value + "\nWeight:" + ItemReference.Weight, Assets.defaultFont);
            tooltipText.FillColor = new Color(200, 200, 200);
            tooltipText.OutlineColor = Color.Black;
            tooltipText.OutlineThickness = 1.0f;
            tooltipText.CharacterSize = 15;

            tick += () => {

            };

            render += (RenderWindow window) => {
                
            };
        }

        public void renderTooltip(RenderWindow window) {
            if (!Hovered) return;

            //render tooltip if hovered
            tooltipBackground.Position = new Vector2f((int)(EntitySprite.Position.X + EntitySprite.TextureRect.Width), (int)(EntitySprite.Position.Y + EntitySprite.TextureRect.Height));
            window.Draw(tooltipBackground);
            tooltipText.Position = new Vector2f(tooltipBackground.Position.X + 2, tooltipBackground.Position.Y);
            window.Draw(tooltipText);
        }
    }
}