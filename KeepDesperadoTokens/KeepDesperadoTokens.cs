using System;
using BepInEx;
using BepInEx.Configuration;
using RoR2;

namespace ROR2_Infinite
{
	[BepInPlugin("com.nutty.keepDesperadoTokens", "keepDesperadoTokens", "1.0.0")]
	public class KeepDesperadoTokens : BaseUnityPlugin
	{
		public static int MaxDesperadoTokens;

		public static ConfigEntry<double> TokenMultiplier { get; set; }
		public KeepDesperadoTokens()
		{
			On.RoR2.CharacterBody.RecalculateStats += this.CharacterBody_RecalculateStats;
			On.EntityStates.GameOver.ShowReport.OnEnter += this.ShowReport_OnEnter;
		}
		public void Awake()
        {
			double min = 0.0;
			double max = 1.0;
			InitConfig();
			TokenMultiplier.Value = Math.Min(Math.Max(TokenMultiplier.Value, min), max);
		}

		private void ShowReport_OnEnter(On.EntityStates.GameOver.ShowReport.orig_OnEnter orig, EntityStates.GameOver.ShowReport self)
		{
			KeepDesperadoTokens.MaxDesperadoTokens = 0;
			orig(self);
		}
		private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
		{
			if (self.isPlayerControlled && self.teamComponent.teamIndex == TeamIndex.Player)
			{
				int currentTokenAmount = self.GetBuffCount(RoR2.RoR2Content.Buffs.BanditSkull);
				int newTokenAmount = (int)Math.Floor(KeepDesperadoTokens.MaxDesperadoTokens * TokenMultiplier.Value);
				if (currentTokenAmount < newTokenAmount)
				{
					for (int i = 0; i < newTokenAmount - currentTokenAmount; i++)
					{
						self.AddBuff(RoR2.RoR2Content.Buffs.BanditSkull);
					}
				}
				if (currentTokenAmount > KeepDesperadoTokens.MaxDesperadoTokens)
				{
					KeepDesperadoTokens.MaxDesperadoTokens = self.GetBuffCount(RoR2.RoR2Content.Buffs.BanditSkull);
				}
			}
			orig(self);
		}
		private void InitConfig()
        {
			TokenMultiplier = Config.Bind<double>(
				"Settings",
				"TokenMultiplier",
				0.25,
				"The multiplier for your tokens on stage change. Has to be written in decimal, i.e.: 1.0 = 100%, 0.5 = 50%, etc.\n"
				+ "Minimum: 0.0, Maximum: 1.0"
			);
		}
	}
}
