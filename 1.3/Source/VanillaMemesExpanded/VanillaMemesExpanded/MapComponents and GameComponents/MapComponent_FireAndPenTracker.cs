﻿using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace VanillaMemesExpanded
{
    public class MapComponent_FireAndPenTracker : MapComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 2000;
        public int firesInTheMap_backup = 0; 
        public int pensInTheMap_backup = 0;



        public MapComponent_FireAndPenTracker(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            if (map.IsPlayerHome)
            {
                PawnCollectionClass.firesInTheMap = firesInTheMap_backup;
                PawnCollectionClass.pensInTheMap = pensInTheMap_backup;

            }

            base.FinalizeInit();

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.firesInTheMap_backup, "firesInTheMap_backup", 0, true);
            Scribe_Values.Look<int>(ref this.pensInTheMap_backup, "pensInTheMap_backup", 0, true);

            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterFire", 0, true);

        }
        public override void MapComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {


                if (map.IsPlayerHome)
                {
                    if ((Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Fire_Desired) != null) || (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Fire_Despised) != null))

                    {

                        firesInTheMap_backup = PawnCollectionClass.firesInTheMap;
                   
                        int wildFires = map.listerThings.ThingsOfDef(ThingDefOf.Fire).Count;
                        int torches = map.listerThings.ThingsOfDef(ThingDefOf.TorchLamp).Count;
                        int campFires = map.listerThings.ThingsOfDef(ThingDefOf.Campfire).Count;
                        int bonfires = map.listerThings.ThingsOfDef(InternalDefOf.VME_BonfireAfterRitual).Count;
                        int stoneCampfires = 0;
                        if (DefDatabase<ThingDef>.GetNamedSilentFail("Stone_Campfire") != null)
                        {
                            stoneCampfires = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamedSilentFail("Stone_Campfire")).Count;
                        }
                        int hearths = 0;
                        if (DefDatabase<ThingDef>.GetNamedSilentFail("VFEV_Hearth") != null)
                        {
                            hearths = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamedSilentFail("VFEV_Hearth")).Count;
                        }
                        int braziers = 0;
                        if (DefDatabase<ThingDef>.GetNamedSilentFail("Brazier") != null)
                        {
                            braziers = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamedSilentFail("Brazier")).Count;
                        }
                        firesInTheMap_backup = wildFires + campFires + stoneCampfires + braziers + bonfires + torches + hearths;
                       
                        PawnCollectionClass.firesInTheMap = firesInTheMap_backup;


                    }

                    if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Ranching_Disliked) != null)

                    {

                     
                        pensInTheMap_backup = PawnCollectionClass.pensInTheMap;
                        int pens = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamedSilentFail("PenMarker")).Count;
                    
                        pensInTheMap_backup = pens;
                      
                        PawnCollectionClass.pensInTheMap = pensInTheMap_backup;


                    }


                   
                }

                


                tickCounter = 0;
            }



        }
       



    }


}



