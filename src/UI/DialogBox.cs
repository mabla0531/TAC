using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace TAC {
    class DialogBox : InterfaceWindow {

        private RectangleShape chatLogBackground;
        private Text chatPrompt; //current npc prompt
        private Text chatResponse; //response submitted, drawn at top of window
        private string chatPromptBuffer;
        private Clock chatPromptBufferClock;

        public FriendlyMob ReferenceEntity {get; set;}

        private RectangleShape[] candidateFrames;
        private Text candidateText;

        private DialogTree currentDialogTree;
        private int currentPromptID;
        private int hoveringID;

        public DialogBox() : base() {
            background.Position = new Vector2f((Game.displayWidth / 2) - (background.Size.X / 2), (Game.displayHeight / 2) - (background.Size.Y));
            background.Size = new Vector2f(500.0f, 300.0f);
            gaussianBlur = new GaussianBlur((int)background.Size.X, (int)background.Size.Y);
            chatLogBackground = new RectangleShape();
            chatLogBackground.FillColor = new Color(200, 200, 200);
            chatLogBackground.OutlineColor = Color.Black;
            chatLogBackground.OutlineThickness = 1.0f;
            chatPrompt = new Text("", Assets.defaultFont, 18);
            chatPrompt.FillColor = new Color(200, 200, 200);
            chatResponse = new Text("", Assets.defaultFont, 18);
            chatResponse.Style = Text.Styles.Italic;
            chatResponse.FillColor = Color.Green;

            candidateFrames = new RectangleShape[3];
            for (int i = 0; i < candidateFrames.Length; i++) {
                candidateFrames[i] = new RectangleShape();
                candidateFrames[i].Size = new Vector2f(background.Size.X - 4.0f, 24.0f);
                candidateFrames[i].FillColor = new Color(80, 80, 80);
                candidateFrames[i].OutlineColor = new Color(80, 80, 80);
                candidateFrames[i].OutlineThickness = 1.0f;
            }
            candidateText = new Text();
            candidateText.Font = Assets.defaultFont;
            candidateText.CharacterSize = 20;
            candidateText.FillColor = new Color(200, 200, 200);

            currentDialogTree = Assets.merchant;
            currentPromptID = 0;
            chatPromptBuffer = currentDialogTree.getPromptByID(currentPromptID).EntryMessage;
            chatPromptBufferClock = new Clock();
            hoveringID = -1;
        }

        public override void tick() {
            if (!Active) return;

            if (TextInputHandler.Characters.Contains(9)) {
                Active = false;
                return;
            }

            if (!candidateFrames[0].GetGlobalBounds().Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY) && 
                !candidateFrames[1].GetGlobalBounds().Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY) && 
                !candidateFrames[2].GetGlobalBounds().Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY)) {
                hoveringID = -1;
            }

            for (int i = 0; i < 3; i++) {
                candidateFrames[i].FillColor = new Color(80, 80, 80);
                candidateFrames[i].OutlineColor = new Color(80, 80, 80);
                if (candidateFrames[i].GetGlobalBounds().Contains((int)MouseHandler.MouseX, (int)MouseHandler.MouseY) && currentDialogTree.getPromptByID(currentPromptID).Options[i] != -1) {
                    candidateFrames[i].FillColor = new Color(100, 100, 100);
                    candidateFrames[i].OutlineColor = Color.Black;
                    
                    if (hoveringID != i)
                        Assets.hover.Play();

                    hoveringID = i;
                    
                    if (MouseHandler.LeftPressed) {
                        Assets.click.Play();
                        Response choice = currentDialogTree.getResponseByID(currentDialogTree.getPromptByID(currentPromptID).Options[i]);
                        chatResponse.DisplayedString = choice.EntryMessage;
                        
                        if (choice.Transition == "shop") {
                            //shop
                            continue;
                        }
                        
                        if (choice.Transition == "exit") {
                            Active = false;
                            continue;
                        }
                        
                        currentPromptID = int.Parse(choice.Transition);

                        chatPrompt.DisplayedString = "";
                        chatPromptBuffer = currentDialogTree.getPromptByID(currentPromptID).EntryMessage;
                        int lastSpace = 0;
                        //Do word wrapping
                        for (int currentChar = 1; currentChar < chatPromptBuffer.Length - 1; currentChar++) {
                            if (chatPromptBuffer.Substring(currentChar, 1) == " ") {
                                lastSpace = currentChar;
                            }
                            if (currentChar % 45 == 0) {
                                chatPromptBuffer = chatPromptBuffer.Substring(0, currentChar - 1) + "\n" + chatPromptBuffer.Substring(currentChar - 1);
                            }
                        }
                    }
                }
            }
        }

        public override void render(RenderWindow window) {
            if (!Active) return;

            background.Position = new Vector2f((Game.displayWidth / 2) - (background.Size.X / 2), (Game.displayHeight / 2) - (background.Size.Y));
            gaussianBlur.blurArea((int)background.Position.X, (int)background.Position.Y, window);
            window.Draw(background);

            if (chatPrompt.DisplayedString != chatPromptBuffer && chatPromptBufferClock.ElapsedTime.AsMilliseconds() >= 30) {
                chatPrompt.DisplayedString = chatPromptBuffer.Substring(0, chatPrompt.DisplayedString.Length + 1);
                if (!chatPrompt.DisplayedString.EndsWith(" "))
                    Assets.text.Play();

                chatPromptBufferClock.Restart();
            }

            chatPrompt.Position = new Vector2f(background.Position.X + 4, background.Position.Y + 24);
            window.Draw(chatPrompt);

            chatResponse.Position = new Vector2f(background.Position.X + 2, background.Position.Y + 2);
            window.Draw(chatResponse);

            for (int i = 0; i < 3; i++) {
                candidateFrames[i].Position = new Vector2f(background.Position.X + 2, background.Position.Y + background.Size.Y - (78.0f - (26.0f * i))); //bottom padding of 2, height of each is 20
                window.Draw(candidateFrames[i]);

                candidateText.DisplayedString = currentDialogTree.getResponseByID(currentDialogTree.getPromptByID(currentPromptID).Options[i]).EntryMessage;
                candidateText.Position = new Vector2f(background.Position.X + 4, background.Position.Y + background.Size.Y - (78.0f - (26.0f * i)));
                window.Draw(candidateText);
            }
        }
    }
}
