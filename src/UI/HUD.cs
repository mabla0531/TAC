using SFML.Graphics;
using SFML.System;

namespace TAC {

    class HUD {
        private RectangleShape hudFrame;
        private GaussianBlur gb;

        private Sprite handSlot;

        private RectangleShape statBar;
        private RectangleShape healthFill;
        private RectangleShape staminaFill;

        private Player player;

        private float hudWidth, hudHeight;

        public HUD(Player p) {

            hudWidth = 512.0f;
            hudHeight = 96.0f;

            hudFrame = new RectangleShape(new Vector2f(hudWidth, hudHeight));
            hudFrame.Position = new Vector2f((Game.displayWidth / 2) - (hudWidth / 2), Game.displayHeight - hudHeight);
            hudFrame.FillColor = new Color(36, 58, 71, 230);
            gb = new GaussianBlur((int)hudWidth, (int)hudHeight);

            statBar     = new RectangleShape(new Vector2f(128.0f, 16.0f));
            healthFill  = new RectangleShape(new Vector2f(128.0f, 16.0f));
            staminaFill = new RectangleShape(new Vector2f(128.0f, 16.0f));

            statBar.FillColor = new Color(200, 200, 200);
            statBar.OutlineColor = new Color(50, 50, 50);
            statBar.OutlineThickness = 1.0f;
            healthFill.FillColor = new Color(158, 43, 35);
            staminaFill.FillColor = new Color(51, 115, 0);

            player = p;
        }

        public void render(RenderWindow window) {
            hudFrame.Position = new Vector2f((Game.displayWidth / 2) - (hudWidth / 2), Game.displayHeight - hudHeight);
            gb.blurArea((int)hudFrame.Position.X, (int)hudFrame.Position.Y, window);
            window.Draw(hudFrame);

            //draw in position of health bar
            statBar.Position    = new Vector2f(hudFrame.Position.X + 16, Game.displayHeight - hudHeight + 16);
            window.Draw(statBar);
            //draw in position of stamina bar
            statBar.Position    = new Vector2f(hudFrame.Position.X + 16, Game.displayHeight - hudHeight + 36);
            window.Draw(statBar);

            if (player.DisplayHealth > player.Health) {
                player.DisplayHealth -= 0.01f;
            }
            
            healthFill.Position = new Vector2f(hudFrame.Position.X + 16,  Game.displayHeight - hudHeight + 16);
            healthFill.Size = new Vector2f(128.0f * (player.DisplayHealth / (float)player.MaxHealth), 16.0f);
            window.Draw(healthFill);
            staminaFill.Position = new Vector2f(hudFrame.Position.X + 16,  Game.displayHeight - hudHeight + 36);
            staminaFill.Size = new Vector2f(128.0f * (player.Stamina / (float)player.MaxStamina), 16.0f);
            window.Draw(staminaFill);

            if (player.Hand != null) {
                handSlot = player.Hand.Icon;
                handSlot.Scale = new Vector2f(3.0f, 3.0f);
                handSlot.Position = new Vector2f(hudFrame.Position.X + hudFrame.Size.X - 56,  hudFrame.Position.Y + 8);
                window.Draw(handSlot);
            }
        }
    }
}