
// Type: GameManager.GameObjects.Components.Health
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;


namespace GameManager.GameObjects.Components
{
    public class Health : Component
    {
        private float health;
        private float maxHealth;
        private Dictionary<Element, Element> weaknesses = new Dictionary<Element, Element>();
        private Element ownerElement;
        private readonly float delayTimer = 0.5f;
        private float timer;

        public bool IsInvincible { get; set; }

        public Health() 
        {
            this.weaknesses.Add(Element.Fire, Element.Ice);
            this.weaknesses.Add(Element.Ice, Element.Fire);
            this.weaknesses.Add(Element.Water, Element.Earth);
            this.weaknesses.Add(Element.Earth, Element.Water);
            this.weaknesses.Add(Element.Stone, Element.Air);
            this.weaknesses.Add(Element.Air, Element.Stone);
        }

        public Health(GameObject gameObject) : this()
        {
           
        }
               

        public float CurrentHealth
        {
            get => this.health;
            set => this.health = value;
        }

        public float MaxHealth
        {
            get => this.maxHealth;
            set => this.maxHealth = value;
        }

        public float HealthPercent => this.health / this.maxHealth;

        public float TakeDamage(float baseDamage, Element? damageElement = null)
        {
            if ((double)this.timer > Glob.GameTime.TotalGameTime.TotalSeconds)
                return this.health;

            this.timer = (float)Glob.GameTime.TotalGameTime.TotalSeconds + this.delayTimer;

            if (this.GameObject.GetComponent<Combat>() == null)
                return 0.0f;

            if (this.IsInvincible)
                return this.health;

            this.GameObject.GetComponent<Audio>()?.Play("Object collision");
            this.ownerElement = this.GameObject.GetComponent<Combat>().SelectedElement;

            baseDamage = this.ModifyDamage(baseDamage, damageElement);

            return this.health = Clamp(this.health - baseDamage, 0.0f, this.maxHealth);
        }

        private float ModifyDamage(float baseDamage, Element? damageElement = null)
        {
            Element? nullable1 = damageElement;
            Element ownerElement = this.ownerElement;

            if (nullable1.GetValueOrDefault() == ownerElement & nullable1.HasValue)
                return baseDamage / 3f;

            int weakness = (int)this.weaknesses[this.ownerElement];
            Element? nullable2 = damageElement;
            int valueOrDefault = (int)nullable2.GetValueOrDefault();
            return weakness == valueOrDefault & nullable2.HasValue ? baseDamage * 2f : baseDamage;
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
