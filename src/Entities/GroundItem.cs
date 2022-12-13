using SFML.Graphics;
using SFML.System;

namespace TAC {
    class GroundItem : Entity {

        public Item ItemReference {get; set;}
        public bool Hovered = false;
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

            sprite = new Sprite(Assets.items);
            sprite.TextureRect = itemRef.Icon.TextureRect;

            tooltipBackground = new RectangleShape(new Vector2f(96.0f, 48.0f));
            tooltipBackground.FillColor = new Color(36, 58, 71);
            tooltipBackground.OutlineColor = Color.Black;
            tooltipBackground.OutlineThickness = 1.0f;

            tooltipText = new Text(ItemReference.Name + "\nValue: " + ItemReference.Value + "\nWeight:" + ItemReference.Weight, Assets.defaultFont);
            tooltipText.FillColor = new Color(200, 200, 200);
            tooltipText.OutlineColor = Color.Black;
            tooltipText.OutlineThickness = 1.0f;
            tooltipText.CharacterSize = 15;
        }

        public override void tick() {
            Hovered = false;
            if (new IntRect((int)(X - Handler.gameState.gameCameraOffset.X), (int)(Y - Handler.gameState.gameCameraOffset.Y), sprite.TextureRect.Width, sprite.TextureRect.Height).Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY))
                Hovered = true;
        }

        public override void render(RenderWindow window) {
            sprite.Position = new Vector2f(X - Handler.gameState.gameCameraOffset.X, Y - Handler.gameState.gameCameraOffset.Y);
            window.Draw(sprite);
        }

        public void renderTooltip(RenderWindow window) {
            if (!Hovered) return;

            //render tooltip if hovered
            tooltipBackground.Position = new Vector2f(sprite.Position.X + sprite.TextureRect.Width, sprite.Position.Y + sprite.TextureRect.Height);
            window.Draw(tooltipBackground);
            tooltipText.Position = new Vector2f(tooltipBackground.Position.X + 2, tooltipBackground.Position.Y);
            window.Draw(tooltipText);
        }
    }
}