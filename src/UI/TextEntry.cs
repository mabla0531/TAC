using SFML.System;
using SFML.Graphics;

namespace TAC {
    class TextEntry {
        public Text Input {get; set;}
        public Vector2f Position {get; set;}
        public Vector2f Size {get; set;}
        public string InputString {get; set;}
        private int maxCharacters;
        private RectangleShape textEntryRect;

        public TextEntry(Vector2f position, int characters) {
            Position = position;
            InputString = "";
            Input = new Text("", Assets.defaultFont, 16);
            Input.FillColor = Color.Black;
            Size = new Vector2f((characters * 9) + 2, Input.CharacterSize + 2);
            maxCharacters = characters;
            textEntryRect = new RectangleShape(Size);
            textEntryRect.FillColor = new Color(220, 220, 220);
            textEntryRect.OutlineThickness = 1.0f;
            textEntryRect.OutlineColor = Color.Black;
        }

        public void tick() {
            foreach (int character in TextInputHandler.Characters) {
                if (character == 8) {
                    if (InputString.Length > 0)
                        InputString = InputString.Substring(0, InputString.Length - 1);
                    continue;
                }

                if (character > 31 && character < 126) InputString += (char)character;
            }
        }

        public void render(RenderWindow window) {
            Input.Position = Position;
            textEntryRect.Position = Position;

            Input.DisplayedString = InputString;
            if (InputString.Length >= maxCharacters)
                Input.DisplayedString = InputString.Substring(InputString.Length - maxCharacters, maxCharacters);

            window.Draw(textEntryRect);
            window.Draw(Input);
        }
    }
}