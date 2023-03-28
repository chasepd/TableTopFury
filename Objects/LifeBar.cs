using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TableTopFury.Objects
{
    internal class LifeBar : UIElement
    {
        Stack<LifeIndicator> lifeIndicators;
        const int maxLife = 7;
        int playerNum;

        public LifeBar(int playerNum) : base() 
        {
            this.playerNum = playerNum;
            lifeIndicators = new Stack<LifeIndicator>();

            for (int i = 0; i < maxLife; i++)
            {
                lifeIndicators.Push(new LifeIndicator(playerNum, i));
            }
        }

        public override void Initialize()
        {
            foreach (LifeIndicator indicator in lifeIndicators)
            {
                indicator.Initialize();
            }
        }

        public override void LoadContent()
        {
            foreach (LifeIndicator indicator in lifeIndicators)
            {
                indicator.LoadContent();
            }
        }

        public override void Update(List<TTFObject> objects) { }

        public override void Draw()
        {
            foreach (LifeIndicator indicator in lifeIndicators)
            {
                indicator.Draw();
            }
        }


        public bool TakeDamage(int amount)
        {
            for (int i = 0; i < amount && lifeIndicators.Count > 0; i++)
            {
                lifeIndicators.Pop();
            }
            if (lifeIndicators.Count == 0)
            {
                return true;
            }
            return false;
        }

        public void HealDamage (int amount)
        {
            if (lifeIndicators.Count < maxLife)
            {
                for (int i = 0; i < amount; i++)
                {
                    lifeIndicators.Push(new LifeIndicator(this.playerNum, lifeIndicators.Count));
                }
            }
        }
    }
}
