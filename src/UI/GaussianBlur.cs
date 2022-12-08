using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace TAC {
    class GaussianBlur {

            private int width, height;

            private Texture windowContent;
            private RenderTexture pass1;
            private RenderTexture pass2;
            private RenderTexture pass3;
            private Sprite blurContent;

            private Shader gb;

            public GaussianBlur(int w, int h) {
                width = w;
                height = h;

                windowContent = new Texture(Game.displayWidth, Game.displayHeight);
                pass1 = new RenderTexture((uint)width, (uint)height);
                pass2 = new RenderTexture((uint)width, (uint)height);
                pass3 = new RenderTexture((uint)width, (uint)height);

                gb = Assets.gaussianBlur;
            }

            public void blurArea(int x, int y, RenderWindow window) {
                
                if (windowContent.Size.X != Game.displayWidth || windowContent.Size.Y != Game.displayHeight)
                    windowContent = new Texture(Game.displayWidth, Game.displayHeight);
                
                //Start shader buffering
                windowContent.Update(window);
                blurContent = new Sprite(windowContent, new IntRect(x, y, width, height));
                blurContent.Position = new Vector2f(0.0f, 0.0f);
                gb.SetUniform("blur_radius", new Vector2f(0.25f / width, 0.0f));
                pass1.Draw(blurContent, new RenderStates(gb));
                pass1.Display();
                
                blurContent = new Sprite(pass1.Texture, new IntRect(0, 0, (int)pass1.Size.X, (int)pass1.Size.Y));
                blurContent.Position = new Vector2f(0.0f, 0.0f);
                gb.SetUniform("blur_radius", new Vector2f(0.0f, 0.25f / height));
                pass2.Draw(blurContent, new RenderStates(gb));
                pass2.Display();

                blurContent = new Sprite(pass2.Texture, new IntRect(0, 0, (int)pass2.Size.X, (int)pass2.Size.Y));
                blurContent.Position = new Vector2f(0.0f, 0.0f);
                gb.SetUniform("blur_radius", new Vector2f(0.25f / width, 0.0f));
                pass3.Draw(blurContent, new RenderStates(gb));
                pass3.Display();

                blurContent = new Sprite(pass3.Texture, new IntRect(0, 0, (int)pass3.Size.X, (int)pass3.Size.Y));
                blurContent.Position = new Vector2f(x, y);
                gb.SetUniform("blur_radius", new Vector2f(0.0f, 0.25f / height));
                window.Draw(blurContent, new RenderStates(gb));
                //End shader buffering
            }
    }
}