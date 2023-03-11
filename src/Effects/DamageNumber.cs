using SFML.Graphics;
using SFML.System;

namespace TAC {
    class DamageNumber {
        public bool Finished {get; set;} = false;
        public float Transparency {get; set;} = 1.0f;
        public Text Damage {get; set;}
        public Vector2f Position {get; set;}
        public DamageNumber(float x, float y, int damage) {
            Damage = new Text(damage.ToString(), Assets.defaultFont, 20);
            Position = new Vector2f(x, y);
            Damage.Position = Position;
            Damage.FillColor = new Color(255, 0, 0, 255);
            Damage.OutlineColor = new Color(0, 0, 0, 255);
            Damage.OutlineThickness = 1.0f;
        }

        public void render(RenderWindow window) {
            Transparency -= 0.001f;
            
            if (Transparency <= 0.0f)
                Finished = true;

            Position = new Vector2f(Position.X, Position.Y - 0.05f);
            Damage.FillColor = new Color(255, 0, 0, (byte)(255.0f * Transparency));
            Damage.OutlineColor = new Color(0, 0, 0, (byte)(255.0f * Transparency));
            Damage.Position = new Vector2f(Position.X - Handler.gameState.gameCameraOffset.X, Position.Y - Handler.gameState.gameCameraOffset.Y);
            window.Draw(Damage);
        }
    }
}