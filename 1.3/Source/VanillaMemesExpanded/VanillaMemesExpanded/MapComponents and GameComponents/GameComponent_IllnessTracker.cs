﻿using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace VanillaMemesExpanded
{
    public class GameComponent_IllnessTracker : GameComponent
    {

       
       
        public int tickCounter = 0;
        public int tickInterval = 3000;
        public Dictionary<Pawn, int> colonist_illness_tracker_backup = new Dictionary<Pawn, int>();
        List<Pawn> list2;
        List<int> list3;



        public GameComponent_IllnessTracker(Game game) : base()
        {

        }

        public override void FinalizeInit()
        {
            PawnCollectionClass.colonist_illness_tracker = colonist_illness_tracker_backup;            
            base.FinalizeInit();

        }

        public override void ExposeData()
       {
           base.ExposeData();

           Scribe_Collections.Look(ref colonist_illness_tracker_backup, "colonist_illness_tracker_backup", LookMode.Reference, LookMode.Value, ref list2, ref list3);           
          
       }



        public override void GameComponentTick()
        {

          
            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Illness_Exalted) != null) 
                {
                    colonist_illness_tracker_backup = PawnCollectionClass.colonist_illness_tracker;
                    List<Pawn> listPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
                    foreach(Pawn p in listPawns)
                    {
                        if (p.Ideo.GetPrecept(InternalDefOf.VME_Illness_Exalted) != null)
                        {
                            PawnCollectionClass.AddColonistToIllnessList(p,0);

                            if (p.health.hediffSet.AnyHediffMakesSickThought)
                            {
                                PawnCollectionClass.ResetPawnIlnessTicks(p);
                            }
                            else { 
                                if(PawnCollectionClass.colonist_illness_tracker[p]<int.MaxValue- tickInterval)
                                {
                                    PawnCollectionClass.IncreasePawnIllnessTicks(p, tickInterval);

                                }
                            }


                        }

                    }

                }

                if (Current.Game.World.factionManager.OfPlayer.ideos.GetPrecept(InternalDefOf.VME_Illness_TimeOff) != null)
                {
                    
                    List<Pawn> listPawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
                    foreach (Pawn p in listPawns)
                    {
                        if (p.health.hediffSet.AnyHediffMakesSickThought)
                        {
                            if (!p.story.traits.HasTrait(InternalDefOf.VME_Time_Off))
                            {
                                Trait trait = new Trait(InternalDefOf.VME_Time_Off, 0, true);
                                p.story.traits.GainTrait(trait);
                            }
                        }
                        else {

                            if (p.story.traits.HasTrait(InternalDefOf.VME_Time_Off))
                            {
                               
                                p.story.traits.RemoveTrait(p.story.traits.GetTrait(InternalDefOf.VME_Time_Off));
                            }
                        }

                    }

                }

                tickCounter = 0;
            }



        }


    }


}
