using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Gradual_Romance;

internal class JobDriver_AskForLovin : JobDriver
{
    private const TargetIndex PersonToAsk = TargetIndex.A;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(job.GetTarget(PersonToAsk), job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(PersonToAsk);
        this.FailOnDowned(PersonToAsk);
        this.FailOnNotCasualInterruptible(PersonToAsk);

        yield return Toils_Goto.GotoThing(PersonToAsk, PathEndMode.Touch);
        yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
        yield return Toils_Interpersonal.GotoInteractablePosition(PersonToAsk);

        //change for our new interaction
        yield return Toils_Interpersonal.Interact(PersonToAsk, InteractionDefOf.Chitchat);
    }
}