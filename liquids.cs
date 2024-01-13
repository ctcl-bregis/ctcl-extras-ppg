// CTCL's Extras for People Playground - CTCL 2023-2024
// File: script.cs
// Purpose: Liquids added by the mod
// Created: September 23, 2023
// Modified: January 13, 2024

using UnityEngine;
using System.Collections;

namespace Liquids
{
    public class BloodRemoverSyringe : SyringeBehaviour {
        public override string GetLiquidID() => BloodRemover.ID;
        
        public class BloodRemover : TemporaryBodyLiquid
        {
            public const string ID = "BLOOD REMOVER";
            public override string GetDisplayName() => "Blood Remover";
        
            public BloodRemover() { 
                Color = new UnityEngine.Color(0.95f, 0.95f, 0.0f, 0.8f); 
            }

            public override void OnEnterLimb(LimbBehaviour limb) {}
            public override void OnUpdate(BloodContainer container)
            {
                base.OnUpdate(container); //just in case whatever you're deriving from does anything

                if (container is CirculationBehaviour circ) {
                    circ.RemoveLiquid(Liquid.GetLiquid("BLOOD"), circ.GetAmount(Liquid.GetLiquid("BLOOD")));
                }
            }
            public override void OnEnterContainer(BloodContainer container) {}
            public override void OnExitContainer(BloodContainer container) {}
        }
    }
    public class BloodSyringe : SyringeBehaviour {
        public override string GetLiquidID() => Blood.ID;
    }
}
