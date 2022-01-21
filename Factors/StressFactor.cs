﻿using KSP.Localization;

namespace KerbalHealth
{
    public class StressFactor : HealthFactor
    {
        public override string Name => "Stress";

        public override string Title => Localizer.Format("#KH_Factor_Stress");

        public override bool ConstantForUnloaded => false;

        public override double BaseChangePerDay => HighLogic.CurrentGame.Parameters.CustomParams<KerbalHealthFactorsSettings>().StressFactor;

        public override double ChangePerDay(KerbalHealthStatus khs)
        {
            if (Core.IsInEditor)
                if (IsEnabledInEditor())
                    return (!KerbalHealthFactorsSettings.Instance.TrainingEnabled || KerbalHealthEditorReport.SimulateTrained
                        ? BaseChangePerDay * (1 - Core.TrainingCap)
                        : BaseChangePerDay)
                        / Core.GetColleaguesCount(khs.ProtoCrewMember);
                else return 0;

            return khs.ProtoCrewMember.rosterStatus == ProtoCrewMember.RosterStatus.Assigned
                ? BaseChangePerDay * (1 - khs.TrainingLevel) / Core.GetColleaguesCount(khs.ProtoCrewMember)
                : 0;
        }
    }
}
