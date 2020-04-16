using ModLib;
using ModLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PartyScreenImprovements
{
    public class Settings : SettingsBase
    {
        private String _ID = "PartyScreenImprovements";

        private static Settings _instance;

        [XmlElement]
        public override string ID
        {
            get
            {
                return _ID; ;
            }
            set
            {
                _ID = value;
            }
        }

        public override string ModuleFolderName => "PartyScreenImprovements";

        public override string ModName => "PartyScreenImprovements";

        public static Settings Instance
        {
            get
            {
                bool flag = Settings._instance == null;
                if (flag)
                {
                    Settings._instance = FileDatabase.Get<Settings>("PartyScreenImprovements");
                    bool flag2 = Settings._instance == null;
                    if (flag2)
                    {
                        Settings._instance = new Settings();
                        FileDatabase.SaveToFile(PartyScreenImprovementsSubModule.ModuleFolderName, Settings._instance);
                    }
                }
                return Settings._instance;
            }
        }

        [XmlElement]
        [SettingProperty("Don't upgrade multipath Troops", "Disables the 50/50 split on multipath troops and does not upgrade them")]
        public bool DontUpgradeMultipath { get; set; } = false;

    }
}
