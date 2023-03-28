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
        List<LifeIndicator> explodingIndicators;

        public LifeBar(int playerNum) : base() 
        {
            this.playerNum = playerNum;
            lifeIndicators = new Stack<LifeIndicator>();
            explodingIndicators = new List<LifeIndicator>();

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

        public override void Update(List<TTFObject> objects) 
        {
            LifeIndicator explodedIndicator = null;
            foreach (LifeIndicator indicator in lifeIndicators)
            {
                indicator.Update(objects);
            }
            foreach (LifeIndicator indicator in explodingIndicators)
            {
                if (indicator.IsDoneExploding())
                {
                    explodedIndicator = indicator;
                }
                indicator.Update(objects);
            }
            if (explodedIndicator is not null)
            {
                explodingIndicators.Remove(explodedIndicator);
            }   
        }

        public override void Draw()
        {
            foreach (LifeIndicator indicator in lifeIndicators)
            {                
                indicator.Draw();
            }
            foreach (LifeIndicator indicator in explodingIndicators)
            {
                indicator.Draw();
            }
        }


        public bool TakeDamage(int amount)
        {
            for (int i = 0; i < amount && lifeIndicators.Count > 0; i++)
            {
                LifeIndicator affected = lifeIndicators.Pop();
                affected.Explode();
                explodingIndicators.Add(affected);
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
