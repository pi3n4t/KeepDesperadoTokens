using System;
using BepInEx.Configuration;
using RoR2;

namespace KeepDesperadoTokens
{
    public class KeepDesperadoTokens
    {
        private int _maxDesperadoTokens;

        public ConfigEntry<double> TokenMultiplier { get; set; }
        public double Min { get; } = 0;
        public double Max { get; } = 1;

        public void Init()
        {
            TokenMultiplier.Value = Math.Min(Math.Max(TokenMultiplier.Value, Min), Max);
        }

        public void OnRunEndResetTokens(On.EntityStates.GameOver.ShowReport.orig_OnEnter orig, EntityStates.GameOver.ShowReport self)
        {
            _maxDesperadoTokens = 0;
            orig(self);
        }

        public void RecalculateTokenAmount(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            if (self.isPlayerControlled && self.teamComponent.teamIndex == TeamIndex.Player)
            {
                int currentTokenAmount = self.GetBuffCount(RoR2Content.Buffs.BanditSkull);
                int newTokenAmount = (int)Math.Floor(_maxDesperadoTokens * TokenMultiplier.Value);

                if (currentTokenAmount < newTokenAmount)
                {
                    for (int i = 0; i < newTokenAmount - currentTokenAmount; i++)
                    {
                        self.AddBuff(RoR2Content.Buffs.BanditSkull);
                    }
                }

                if (currentTokenAmount > _maxDesperadoTokens)
                {
                    _maxDesperadoTokens = self.GetBuffCount(RoR2Content.Buffs.BanditSkull);
                }
            }

            orig(self);
        }
    }
}
