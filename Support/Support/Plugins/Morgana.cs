﻿#region LICENSE

// /*
// Copyright 2014 - 2014 Support
// Morgana.cs is part of Support.
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// */
// 

#endregion

#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Support.Plugins
{
    public class Morgana : PluginBase
    {
        public Morgana()
            : base("h3h3", new Version(4, 17, 14))
        {
            Q = new Spell(SpellSlot.Q, 1175);
            W = new Spell(SpellSlot.W, 900);
            E = new Spell(SpellSlot.E, 750);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.234f, 70f, 1200f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.28f, 175f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (Q.CastCheck(Target, "ComboQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (W.CastCheck(Target, "ComboW") && E.GetPrediction(Target).Hitchance == HitChance.Immobile)
                {
                    W.Cast(Target.Position, UsePackets);
                }

                if (R.CastCheck(Target, "ComboR"))
                {
                    R.CastIfWillHit(Target, ConfigValue<Slider>("ComboCountR").Value, UsePackets);
                }
            }

            if (HarassMode)
            {
                if (Q.CastCheck(Target, "HarassQ"))
                {
                    Q.Cast(Target, UsePackets);
                }

                if (W.CastCheck(Target, "HarassW") && E.GetPrediction(Target).Hitchance == HitChance.Immobile)
                {
                    W.Cast(Target.Position, UsePackets);
                }
            }
        }

        public override void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
                return;

            if (Q.CastCheck(gapcloser.Sender, "GapcloserQ"))
            {
                Q.Cast(gapcloser.Sender, UsePackets);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
            config.AddSlider("ComboCountR", "Targets in range to Ult", 2, 1, 5);
        }

        public override void HarassMenu(Menu config)
        {
            config.AddBool("HarassQ", "Use Q", true);
            config.AddBool("HarassW", "Use W", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("GapcloserQ", "Use Q to Interrupt Gapcloser", true);
        }
    }
}