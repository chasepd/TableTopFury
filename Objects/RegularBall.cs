using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class RegularBall : Ball
    {

        public RegularBall() : base() { }

        public RegularBall(Ball previous) : base(previous) { }
        public override void LoadContent()
        {
            ballTexture = GameState.Content.Load<Texture2D>("RegularBall");
            explosionTexture = GameState.Content.Load<Texture2D>("Explosion");
            texture = ballTexture;
            _explosionSound = GameState.Content.Load<SoundEffect>("ExplosionSound");
            _contactSounds.Add(GameState.Content.Load<SoundEffect>("clink1"));
            _contactSounds.Add(GameState.Content.Load<SoundEffect>("clink2"));
            _contactSounds.Add(GameState.Content.Load<SoundEffect>("thud2"));
            _contactSounds.Add(GameState.Content.Load<SoundEffect>("thud3"));
        }

        public override int DamageValue()
        {
            return 1;
        }
    }
}
