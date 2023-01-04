using SFML.Graphics;
using SFML.Audio;

namespace TAC {

    class Assets {

        //Textures
        public static Texture terrain;
        public static Texture player;
        public static Texture enemy;
        public static Texture ui;
        public static Texture items;
        public static Texture menuArt;
        public static Texture logo;
        public static Texture settings;
        public static Texture tree;
        public static byte[] cursorData;

        //Shaders
        public static Shader gaussianBlur;

        //Audio
        public static Sound slice;
        public static Sound walk;
        public static Sound swish;
        public static Sound grrr;
        public static Sound click;
        public static Sound hover;

        //Font
        public static Font defaultFont;

        public static void updateVolume(float volume) {
            slice.Volume = volume;
            walk.Volume  = volume;
            swish.Volume = volume;
            grrr.Volume  = volume;
            click.Volume = volume;
            hover.Volume = volume;
        }

        public static void init() { //bit schewpid, init()? 
            terrain = new Texture("res/textures/terrain.png");
            player = new Texture("res/textures/player.png");
            enemy = new Texture("res/textures/enemy.png");
            ui = new Texture("res/textures/ui.png");
            items = new Texture("res/textures/items.png");
            menuArt = new Texture("res/textures/menuart.png");
            logo = new Texture("res/textures/logo.png");
            settings = new Texture("res/textures/settings.png");
            tree = new Texture("res/textures/tree.png");

            //create cursor (SFML's Texture class and image handling is so goofy it's mind blowing)
            RenderTexture cursor = new RenderTexture(12, 17);
            Sprite cursorSprite = new Sprite(ui);
            cursorSprite.TextureRect = new IntRect(168, 128, 12, 17);
            cursor.Draw(cursorSprite);
            cursor.Display(); //why is this a thing
            cursorData = cursor.Texture.CopyToImage().Pixels; //hahaha the cursor has to be a pixel array instead of like maybe a sprite

            gaussianBlur = new Shader(null, null, "res/shaders/gaussian.frag");

            slice = new Sound(new SoundBuffer("res/sounds/knifeSlice.ogg"));
            slice.Volume = SettingsState.Volume * 100.0f;
            walk  = new Sound(new SoundBuffer("res/sounds/leaves01.ogg"));
            walk.Volume =  SettingsState.Volume * 100.0f;
            swish = new Sound(new SoundBuffer("res/sounds/swish-4.wav"));
            swish.Volume = SettingsState.Volume * 100.0f;
            grrr  = new Sound(new SoundBuffer("res/sounds/monster-4.wav"));
            grrr.Volume =  SettingsState.Volume * 100.0f;
            click = new Sound(new SoundBuffer("res/sounds/zipclick.wav"));
            click.Volume = SettingsState.Volume * 100.0f;
            hover = new Sound(new SoundBuffer("res/sounds/hover.wav"));
            hover.Volume = SettingsState.Volume * 100.0f;

            defaultFont = new Font("res/fonts/default.ttf");
        }

        public static void cleanup() {
            slice.SoundBuffer.Dispose();
            walk.SoundBuffer.Dispose();
            swish.SoundBuffer.Dispose();
            grrr.SoundBuffer.Dispose();
            click.SoundBuffer.Dispose();
            hover.SoundBuffer.Dispose();
            slice.Dispose();
            walk.Dispose();
            swish.Dispose();
            grrr.Dispose();
            click.Dispose();
            hover.Dispose();
        }
    }
}