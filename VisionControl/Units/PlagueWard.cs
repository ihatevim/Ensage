﻿namespace VisionControl.Units
{
    using System.Linq;

    using Ensage;

    using SharpDX;

    internal class PlagueWard : IUnit
    {
        #region Constants

        private const string AbilityName = "venomancer_plague_ward";

        #endregion

        #region Fields

        private readonly Unit unit;

        #endregion

        #region Constructors and Destructors

        public PlagueWard(Unit unit)
        {
            this.unit = unit;
            PositionCorrection = new Vector2(25);
            Radius = 600;
            Duration =
                Ability.GetAbilityDataByName(AbilityName).AbilitySpecialData.First(x => x.Name == "duration").Value;
            Position = unit.Position;
            Texture = Drawing.GetTexture("materials/ensage_ui/other/plague_ward");
            Handle = unit.Handle;
            TextureSize = new Vector2(40);
            EndTime = Game.RawGameTime + Duration;
            ShowTimer = Menu.TimerEnabled(AbilityName);

            if (Menu.RangeEnabled(AbilityName))
            {
                ParticleEffect = new ParticleEffect("particles/ui_mouseactions/drag_selected_ring.vpcf", Position);
                ParticleEffect.SetControlPoint(1, new Vector3(153, 153, 0));
                ParticleEffect.SetControlPoint(2, new Vector3(Radius, 255, 0));
            }
        }

        #endregion

        #region Public Properties

        public float Duration { get; }

        public float EndTime { get; }

        public uint Handle { get; }

        public bool IsVisible => unit.IsVisible;

        public ParticleEffect ParticleEffect { get; }

        public Vector3 Position { get; }

        public Vector2 PositionCorrection { get; }

        public float Radius { get; }

        public bool ShowTexture => !unit.IsVisible;

        public bool ShowTimer { get; }

        public DotaTexture Texture { get; }

        public Vector2 TextureSize { get; set; }

        #endregion

        #region Properties

        private static MenuManager Menu => Variables.Menu;

        #endregion
    }
}