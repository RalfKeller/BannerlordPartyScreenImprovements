using MountAndBlade.CampaignBehaviors;
using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using static TaleWorlds.CampaignSystem.PartyScreenLogic;

namespace PartyScreenImprovements
{
    class RecruitAllPrisonersWidget : ButtonWidget
    {
		private PartyVM _partyVM;
		private PartyScreenLogic _partyLogic;
		private MBBindingList<PartyCharacterVM> _mainPartyList;

		public RecruitAllPrisonersWidget(UIContext context) : base(context)
        {
			if (ScreenManager.TopScreen is GauntletPartyScreen)
			{
				this._partyVM = (PartyVM)((GauntletPartyScreen)ScreenManager.TopScreen).GetField("_dataSource");
				this._partyLogic = (PartyScreenLogic)this._partyVM.GetField("_partyScreenLogic");
				this._mainPartyList = this._partyVM.MainPartyTroops;
			}
		}

        protected override void OnClick()
        {
			
			TroopRoster prisonRoster = MobileParty.MainParty.PrisonRoster;
			int num = 0;
			IRecruitPrisonersCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IRecruitPrisonersCampaignBehavior>();
			CharacterObject bestPrisoner = null;
			while ((bestPrisoner = getBestRecruitablePrisoner()) != null)
			{
				if (MobileParty.MainParty.Party.PartySizeLimit <= MobileParty.MainParty.MemberRoster.TotalManCount)
				{
					break;
				}

				int recruitableNumber = Campaign.Current.GetCampaignBehavior<IRecruitPrisonersCampaignBehavior>().GetRecruitableNumber(bestPrisoner);
				if (recruitableNumber > 0)
				{
					int maxRecruitable = MobileParty.MainParty.Party.PartySizeLimit - MobileParty.MainParty.MemberRoster.TotalManCount;
					recruitableNumber = Math.Min(recruitableNumber, maxRecruitable);
					num += recruitableNumber;
					PartyCommand command = new PartyCommand();
					command.FillForRecruitTroop(PartyRosterSide.Right, TroopType.Prisoner, bestPrisoner, recruitableNumber);
					_partyLogic.AddCommand(command);
				}
			}
			InformationManager.DisplayMessage(new InformationMessage("Recruited " + num.ToString() + " prisoners"));
		}

		private CharacterObject getBestRecruitablePrisoner()
		{
			TroopRoster prisoners = MobileParty.MainParty.PrisonRoster;
			CharacterObject bestPrisoner = null;
			int highestLevel = -1;
			for (int i = 0; i < prisoners.Count; i++)
			{
				CharacterObject current = prisoners.GetCharacterAtIndex(i);
				int level = current.Level;
				int recruitableNumer = Campaign.Current.GetCampaignBehavior<IRecruitPrisonersCampaignBehavior>().GetRecruitableNumber(current); 
				if(recruitableNumer > 0 && level > highestLevel)
				{
					highestLevel = level;
					bestPrisoner = current;
				}
			}
			return bestPrisoner;
		}
    }
}
