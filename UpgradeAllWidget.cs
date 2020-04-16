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

namespace PartyScreenImprovements
{
	class UpgradeAllWidget : ButtonWidget
	{
		private PartyVM _partyVM;
		private PartyScreenLogic _partyLogic;
		private MBBindingList<PartyCharacterVM> _mainPartyList;

		public UpgradeAllWidget(UIContext context) : base(context)
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
			MobileParty mainParty = MobileParty.MainParty;
			TroopRoster memberRoster = mainParty.MemberRoster;
			Hero playerHero = mainParty.Party.LeaderHero;
			Random r = new Random();
			int num = 0;

			bool upgrade1LastTime = false;
			for(int index = 0; index < _mainPartyList.Count; index++)
			{
				PartyCharacterVM partyCharacter = _mainPartyList[index];
				int numOfUpgradableTroops = partyCharacter.NumOfUpgradeableTroops;
				if (numOfUpgradableTroops > 0)
				{
					for (int i = 0; i < numOfUpgradableTroops; i++)
					{

						if(Settings.Instance.DontUpgradeMultipath && partyCharacter.IsUpgrade2Exists)
						{
							continue;
						}

						bool[] availableUpgrade = new bool[2];
						availableUpgrade[0] = partyCharacter.IsUpgrade1Available && !partyCharacter.IsUpgrade1Insufficient;
						availableUpgrade[1] = partyCharacter.IsUpgrade2Available && !partyCharacter.IsUpgrade2Insufficient;

						if (!availableUpgrade[0] && !availableUpgrade[1])
						{
							continue;
						}

						int onlyPossibleIndex = -1;
						if (availableUpgrade[0] && !availableUpgrade[1])
						{
							onlyPossibleIndex = 0;
						}
						if (!availableUpgrade[0] && availableUpgrade[1])
						{
							onlyPossibleIndex = 1;
						}


						int upgradePath;
						if (onlyPossibleIndex == -1)
						{
							if(upgrade1LastTime)
							{
								upgradePath = 1;
							}
							else
							{
								upgradePath = 0;
							}
							upgrade1LastTime = !upgrade1LastTime;
						}
						else
						{
							upgradePath = onlyPossibleIndex;
						}

						PartyScreenLogic.PartyCommand partyCommand = new PartyScreenLogic.PartyCommand();
						if(partyCharacter.Number == 1)
						{
							index--;
						}
						partyCommand.FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide.Right, PartyScreenLogic.TroopType.Member, partyCharacter.Character, 1, (PartyScreenLogic.PartyCommand.UpgradeTargetType)upgradePath);
						this._partyVM.CurrentCharacter = partyCharacter;
						this._partyLogic.AddCommand(partyCommand);
						num++;
					}

				}
			}
			InformationManager.DisplayMessage(new InformationMessage("Upgraded " + num.ToString() + " soldiers."));
		}
	}
}
