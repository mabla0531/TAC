using SFML.Graphics;
using SFML.System;
using System.Threading;

namespace TAC {
    class DialogBox : InterfaceWindow {

        private RectangleShape chatLogBackground;
        private TextEntry chatBox;
        private Text chatResponse;
        private Text chatPrompt;
        public string ResponseEntityType {get; set;}
        private ChatAPIHandler chatAPIHandler;

        public DialogBox() : base() {
            chatLogBackground = new RectangleShape();
            chatLogBackground.FillColor = new Color(200, 200, 200);
            chatLogBackground.OutlineColor = Color.Black;
            chatLogBackground.OutlineThickness = 1.0f;
            chatBox = new TextEntry(new Vector2f(background.Position.X + 2, background.Position.Y + background.Size.Y - 18), 30);
            chatResponse = new Text("", Assets.defaultFont, 16);
            chatPrompt = new Text("", Assets.defaultFont, 16);
            chatPrompt.Style = Text.Styles.Italic;
            chatPrompt.FillColor = Color.Green;
            ResponseEntityType = "";
            chatAPIHandler = new ChatAPIHandler();
        }

        public async void awaitResponse() {
            string responseString = (await chatAPIHandler.getResponse("Within 240 characters, roleplay briefly as a " + ResponseEntityType + " and respond to the following: " + chatBox.InputString)).Replace("\n", " ");
            chatResponse.DisplayedString = "";
            int lastSpace = -1;
            while (responseString.Length > 30) {
                for (int i = 0; i < 30; i++) {
                    if (responseString[i] == ' ') {
                        lastSpace = i;
                    }
                }

                chatResponse.DisplayedString += responseString.Substring(0, lastSpace) + '\n';
                responseString = responseString.Substring(lastSpace + 1);
            }
            chatResponse.DisplayedString += responseString;
        }

        public override void tick() {
            if (!Active) return;

            if (TextInputHandler.Characters.Contains(9)) { //Tab key to close
                Active = false;
                return;
            }

            if (TextInputHandler.Characters.Contains(13)) { //Enter key to send prompt
                chatPrompt.DisplayedString = chatBox.InputString;
                if (chatPrompt.DisplayedString.Length > 30)
                    chatPrompt.DisplayedString = chatPrompt.DisplayedString.Substring(0, 27) + "...";
                chatResponse.DisplayedString = "Thinking...";
                awaitResponse();
                chatBox.InputString = "";
            }
            
            chatBox.tick();
        }

        public override void render(RenderWindow window) {
            if (!Active) return;

            background.Position = new Vector2f((Game.displayWidth / 2) - (background.Size.X / 2), (Game.displayHeight / 2) - (background.Size.Y));
            gaussianBlur.blurArea((int)background.Position.X, (int)background.Position.Y, window);
            window.Draw(background);

            chatResponse.Position = new Vector2f(background.Position.X + 4, background.Position.Y + 24);
            window.Draw(chatResponse);

            chatPrompt.Position = new Vector2f(background.Position.X + 2, background.Position.Y + 2);
            window.Draw(chatPrompt);

            chatBox.Position = new Vector2f(background.Position.X + 2, background.Position.Y + background.Size.Y - 20);
            chatBox.render(window);
        }
    }
}
