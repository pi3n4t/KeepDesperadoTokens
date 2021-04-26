using System;
using BepInEx.Configuration;
using RoR2;
using EntityStates.GameOver;

namespace KeepDesperadoTokens
{
    public class KeepDesperadoTokens
    {
        private int _lastStageTokenAmount = 0;

        public ConfigEntry<double> TokenMultiplier { get; set; }
        public double Min { get; } = 0;
        public double Max { get; } = 100;

        public void Init()
        {
            TokenMultiplier.Value = Math.Min(Math.Max(TokenMultiplier.Value, Min), Max);
        }

        public void ResetTokens(On.EntityStates.GameOver.ShowReport.orig_OnEnter orig, ShowReport self)
        {
            _lastStageTokenAmount = 0;
            orig(self);
        }

        public void RecalculateTokenAmount(CharacterBody body)
        {
            if (body.isPlayerControlled && body.teamComponent.teamIndex == TeamIndex.Player)
            {
                int currentTokenAmount = body.GetBuffCount(RoR2Content.Buffs.BanditSkull);
                int newTokenAmount = (int)Math.Floor(_lastStageTokenAmount * TokenMultiplier.Value);

                if (currentTokenAmount < newTokenAmount)
                {
                    for (int i = 0; i < newTokenAmount - currentTokenAmount; i++)
                    {
                        body.AddBuff(RoR2Content.Buffs.BanditSkull);
                    }
                }
            }
        }
        public void OnAdvanceStageSaveTokens(TeleporterInteraction interaction)
        {
            CharacterBody playerBody = PlayerCharacterMasterController.instances[0].master.GetBody();
            _lastStageTokenAmount = playerBody.GetBuffCount(RoR2Content.Buffs.BanditSkull);
        }
    }
}
