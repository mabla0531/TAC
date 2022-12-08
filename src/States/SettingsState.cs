using SFML.Graphics;
using SFML.System;
using System;
using System.IO;

namespace TAC {
    class SettingsState : State {

        private Slider volumeSlider;
        private Button applyButton;
        private Button cancelButton;
        private Label volumeLabel;
        private Sprite menuArt;
        private Sprite settingsBanner;

        public static float Volume;
        public static bool Fullscreen;

        public static void save() {
             StreamWriter sw = new StreamWriter("settings.dat");
             sw.WriteLine(Volume);
             sw.Close();
        }

        public static void load() {
            if (!File.Exists("settings.dat")) {
                StreamWriter sw = new StreamWriter("settings.dat");
                sw.WriteLine("1.0");
                sw.Close();
            }

            string[] lines = File.ReadAllLines("settings.dat");
            
            Volume = float.Parse(lines[0]);
        }

        public SettingsState() : base() {
            load();

            volumeSlider = new Slider(new Vector2f(32.0f, 164.0f), 128.0f);
            volumeSlider.Fill = (Volume * volumeSlider.Length);
            volumeLabel = new Label("Volume:" + (int)(Volume * 100), new Vector2f(32.0f, 128.0f), 128.0f);
            
            applyButton = new Button("Apply", new Vector2f((Game.displayWidth / 2) - 128.0f, Game.displayHeight - 64.0f), true);
            applyButton.onClick += (sender, e) => {
                save();
                Handler.game.popState();
            };
            cancelButton = new Button("Cancel", new Vector2f((Game.displayWidth / 2), Game.displayHeight - 64.0f), true);
            cancelButton.onClick += (sender, e) => { Handler.game.popState(); };
            
            menuArt = new Sprite(Assets.menuArt, new IntRect(new Vector2i(0, 0), (Vector2i)Assets.menuArt.Size));
            menuArt.Position = new Vector2f(0.0f, 0.0f);
            menuArt.Scale = new Vector2f((float)Game.displayWidth / 1920.0f, (float)Game.displayHeight / 1080.0f); //scale from 1920x1080 to game height
            settingsBanner = new Sprite(Assets.settings, new IntRect(new Vector2i(0, 0), (Vector2i)Assets.settings.Size));
            settingsBanner.Position = new Vector2f((Game.displayWidth / 2) - (Assets.settings.Size.X / 2), 32.0f);

        }

        public override void tick() {
            volumeSlider.tick();
            Volume = (float)Math.Round((volumeSlider.Fill / volumeSlider.Length), 2);
            volumeLabel.drawText.DisplayedString = "Volume:" + (int)(Volume * 100);
            Assets.updateVolume(Volume * 100.0f);

            applyButton.Position = new Vector2f((Game.displayWidth / 2) - 128.0f, Game.displayHeight - 64.0f);
            cancelButton.Position = new Vector2f((Game.displayWidth / 2), Game.displayHeight - 64.0f);
            applyButton.tick();
            cancelButton.tick();

            menuArt.Scale = new Vector2f((float)Game.displayWidth / 1920.0f, (float)Game.displayHeight / 1080.0f); //scale from 1920x1080 to game height
            settingsBanner.Position = new Vector2f((Game.displayWidth / 2) - (Assets.settings.Size.X / 2), 32.0f);
        }

        public override void render(RenderWindow window) {
            window.Draw(menuArt);
            window.Draw(settingsBanner);

            volumeSlider.render(window);
            volumeLabel.render(window);

            applyButton.render(window);
            cancelButton.render(window);
        }
    }
}
