using System;
using BepInEx;
using BepInEx.Logging;
using On.RoR2;
using On.EntityStates.GameOver;

namespace KeepDesperadoTokens
{
    [BepInPlugin("com.nutty.keepDesperadoTokens", "keepDesperadoTokens", "1.0.1")]
    public class EntryPoint : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger { get; private set; }

        private readonly KeepDesperadoTokens _keepDesperadoTokens = new KeepDesperadoTokens();
        private readonly LightsoutRewards _lightsoutRewards = new LightsoutRewards();

        public EntryPoint()
        {
            Logger = base.Logger;

            CharacterBody.RecalculateStats += _keepDesperadoTokens.RecalculateTokenAmount;
            ShowReport.OnEnter += _keepDesperadoTokens.OnRunEndResetTokens;
        }

        public void Awake()
        {
            InitConfig();
            _keepDesperadoTokens.Init();
            _lightsoutRewards.Init();
        }

        private void InitConfig()
        {
            _keepDesperadoTokens.TokenMultiplier = Config.Bind(
                "Settings",
                "TokenMultiplier",
                0.25,
                $"The multiplier for your tokens on stage change. Has to be written in decimal, i.e.: 1.0 = 100%, 0.5 = 50%, etc.{Environment.NewLine}Minimum: {_keepDesperadoTokens.Min}, Maximum: {_keepDesperadoTokens.Max}"
            );
        }

    }
}
