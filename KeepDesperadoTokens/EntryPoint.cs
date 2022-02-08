using System;
using BepInEx;
using BepInEx.Logging;
using RoR2;
using On.EntityStates.GameOver;

namespace KeepDesperadoTokens
{
    [BepInPlugin("com.nutty.keepDesperadoTokens", "keepDesperadoTokens", "1.0.1")]
    public class EntryPoint : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger { get; private set; }

        private readonly KeepDesperadoTokens _keepDesperadoTokens = new KeepDesperadoTokens();

        public EntryPoint()
        {
            Logger = base.Logger;

            CharacterBody.onBodyStartGlobal += _keepDesperadoTokens.RecalculateTokenAmount;
            TeleporterInteraction.onTeleporterFinishGlobal += _keepDesperadoTokens.OnAdvanceStageSaveTokens;
            ShowReport.OnEnter += _keepDesperadoTokens.ResetTokens;
        }

        public void Awake()
        {
            InitConfig();
            _keepDesperadoTokens.Init();
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

        public void Destroy()
        {
            _keepDesperadoTokens.ResetTokens();

            CharacterBody.onBodyStartGlobal -= _keepDesperadoTokens.RecalculateTokenAmount;
            TeleporterInteraction.onTeleporterFinishGlobal -= _keepDesperadoTokens.OnAdvanceStageSaveTokens;
            ShowReport.OnEnter -= _keepDesperadoTokens.ResetTokens;
        }

    }
}
