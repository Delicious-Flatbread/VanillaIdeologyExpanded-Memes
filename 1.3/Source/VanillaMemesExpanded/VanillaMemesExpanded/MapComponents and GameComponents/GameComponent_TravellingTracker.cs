﻿using System;
using RimWorld;
using Verse;
using RimWorld.Planet;
using System.Collections.Generic;


namespace VanillaMemesExpanded
{
    public class GameComponent_TravellingTracker : GameComponent
    {

       
       
        public int tickCounter = 0;
        public int tickInterval = 6000;
        public int ticksWithoutAbandoningbackup;
        public Dictionary<Pawn, int> colonist_caravan_tracker_backup = new Dictionary<Pawn, int>();
        List<Pawn> list2;
        List<int> list3;


        public GameComponent_TravellingTracker(Game game) : base()
        {

        }

        public override void FinalizeInit()
        {
            PawnCollectionClass.ticksWithoutAbandoning = ticksWithoutAbandoningbackup;
            PawnCollectionClass.colonist_caravan_tracker = colonist_caravan_tracker_backup;


            base.FinalizeInit();

        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterTravel", 0, true);
            Scribe_Values.Look<int>(ref this.ticksWithoutAbandoningbackup, "ticksWithoutAbandoningbackup", 0, true);
            Scribe_Collections.Look(ref colonist_caravan_tracker_backup, "colonist_caravan_tracker_backup", LookMode.Reference, LookMode.Value, ref list2, ref list3);



        }


        public override void GameComponentTick()
        {

          
            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                colonist_caravan_tracker_backup=PawnCollectionClass.colonist_caravan_tracker;
                ticksWithoutAbandoningbackup = PawnCollectionClass.ticksWithoutAbandoning;

                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_PermanentBases_Despised) != null)
                {

                    int num = 0;
                    List<Map> maps = Find.Maps;
                    for (int i = 0; i < maps.Count; i++)
                    {
                        if (maps[i].IsPlayerHome && maps[i].Parent is Settlement)
                        {
                            num++;
                        }
                    }
                    if (num != 0) {
                        if (PawnCollectionClass.ticksWithoutAbandoning < int.MaxValue - tickInterval)
                        {
                            PawnCollectionClass.ticksWithoutAbandoning += tickInterval;
                        }

                    }

                }
                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Travel_Desired) != null
                    || Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Travel_Despised) != null)

                    
                {
                    List<Pawn> listPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
                    foreach (Pawn p in listPawns)
                    {
                        if (p.Ideo.GetPrecept(InternalDefOf.VME_Travel_Desired) != null || p.Ideo.GetPrecept(InternalDefOf.VME_Travel_Despised) != null)
                        {



                            if (PawnCollectionClass.colonist_caravan_tracker.ContainsKey(p) && p.GetCaravan()==null)
                            {
                                if (PawnCollectionClass.colonist_caravan_tracker[p] < int.MaxValue - tickInterval)
                                {
                                    PawnCollectionClass.IncreasePawnCaravanTicks(p, tickInterval);

                                }
                            }
                            


                        }

                    }
                }




                tickCounter = 0;
            }



        }


    }


}
