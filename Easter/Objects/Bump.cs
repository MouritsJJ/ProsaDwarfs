using static SDL2.SDL;
using static SDL2.SDL_mixer;
using static Easter.Core.Game;

namespace Easter.Objects
{
    public class Bump
    {
        public SDL_Rect Pos = new SDL_Rect();
        public IntPtr Texture { get; set; }
        public IntPtr Egg { get; set; }
        public int Points { get; set; }

        private int Frame = 0;
        private byte alpha = 255;
        
        public bool collided { get; private set; } = false;

        public bool Update(App app)
        {
            // Collision detection
            if (!collided && CollisionDetection(Pos, app.Bunny.Pos))
            {
                Mix_PlayChannel(-1, app.Egg_Sound, 0);
                SDL_DestroyTexture(Texture);
                Texture = Egg;
                SDL_QueryTexture(Texture, out _, out _, out int w, out int h);
                Pos = new SDL_Rect() { w = w, h = h, x = Pos.x, y = Pos.y };
                collided = true;
                app.Points += Points;
                app.EggCounts[app.EggPoints.ToList().IndexOf(Points)]++;
            }
            // Move egg up
            if (collided && ++Frame < 30)
                Pos.y -= 2;
            // Fade out
            if (collided && alpha > 0 && Frame >= 30)
            {
                alpha = (byte)Math.Max(alpha - 5, 0);
                SDL_SetTextureAlphaMod(Texture, alpha);
            }
            if (collided && alpha == 0)
            {
                Clean();
                return true;
            }
            return false;
        }

        public void Clean()
        {
            SDL_DestroyTexture(Texture);
            SDL_DestroyTexture(Egg);
        }
    }
}