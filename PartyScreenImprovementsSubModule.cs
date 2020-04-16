using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using System.Linq;
using System.Text;
using ModLib;
using System.Windows.Forms;
using HarmonyLib;

namespace PartyScreenImprovements
{
    public class PartyScreenImprovementsSubModule : MBSubModuleBase
    {
        public static readonly string ModuleFolderName = "PartyScreenImprovements";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                FileDatabase.Initialise(ModuleFolderName);
                SettingsDatabase.RegisterSettings(Settings.Instance);
                Harmony harmony = new Harmony("mod.bannerlord.mipen");
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error initializing Party Screen Improvements: " + e.Message);
            }
        }
    }
}
