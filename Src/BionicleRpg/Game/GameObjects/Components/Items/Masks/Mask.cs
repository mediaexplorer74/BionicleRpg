
// Type: GameManager.GameObjects.Components.Items.Masks.Mask
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using GameManager.DataTypes;
using GameManager.GameObjects.Components.PlayerComponents;
using System;

#nullable disable
namespace GameManager.GameObjects.Components.Items.Masks
{
    public abstract class Mask : Item
    {
        private float maskTimer;
        private const float MaskDelayTimer = 1f;
        protected float MaskEnergyConsumption = 5f;
        protected float energyRegain = 1f;

        public float MaxMaskEnergy { get; set; } = 100f;

        public float MinMaskEnergy { get; protected set; }

        public float MaskEnergy { get; set; } = 100f;

        public bool PowerActive { get; private set; }

        public string MaskSprite { get; protected set; } = "None";

        public string MaskUiSprite { get; protected set; }

        public GameObject Owner { get; set; }

        public void Start()
        {
        }

        public virtual void ActivateMaskPower()
        {
            if ((double)this.MaskEnergy < 0.10000000149011612)
                return;
            this.PowerActive = true;
            Player.Instance.Light.UpdateLightSettings(new LightColor?(), new int?(5), new float?(1f), new float?());
            Player.Instance.UpdateCombatStats();
        }

        public virtual void DeactivateMaskPower()
        {
            this.PowerActive = false;
            Player.Instance.Light.UpdateLightSettings(new LightColor?(), new int?(3), new float?(0.25f), new float?());
            Player.Instance.UpdateCombatStats();
        }

        public void Update()
        {
            if ((double)this.maskTimer > Glob.GameTime.TotalGameTime.TotalSeconds)
                return;
            this.maskTimer = (float)Glob.GameTime.TotalGameTime.TotalSeconds + 1f;
            if (this.PowerActive)
                this.MaskEnergy = Clamp(this.MaskEnergy -= this.MaskEnergyConsumption, 0.0f, this.MaxMaskEnergy);
            if (!this.PowerActive && (double)this.MaskEnergyConsumption < (double)this.MaxMaskEnergy)
                this.MaskEnergy = Clamp(this.MaskEnergy += this.energyRegain, 0.0f, this.MaxMaskEnergy);
            if ((double)this.MaskEnergy > 0.0 || !this.PowerActive)
                return;
            this.DeactivateMaskPower();
        }

        public virtual void EquipMask()
        {
        }

        public virtual void UnequipMask()
        {
            if (!this.PowerActive)
                return;
            this.DeactivateMaskPower();
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
