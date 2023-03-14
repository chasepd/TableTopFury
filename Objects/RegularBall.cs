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
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            ballTexture = content.Load<Texture2D>("RegularBall");
            explosionTexture = _contentManager.Load<Texture2D>("Explosion");
            texture = ballTexture;
        }
    }
}
