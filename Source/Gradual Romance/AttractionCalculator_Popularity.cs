using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Popularity : AttractionCalculator
    {
        private const float FriendAttractionDampener = 0.4f;

        public override bool Check(Pawn observer, Pawn assessed)
        {
            if (!observer.IsColonist || !assessed.IsColonist)
            {
                return false;
            }

            return true;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            //List<Pawn> allPawns = assessed.MapHeld.mapPawns.AllPawnsSpawned;
            /*
            if (cachedValues)
            {
                numOfAssessedFriends = GRHelper.GRPawnComp(assessedPawn).cachedNumberOfColonyFriends;
                numOfObservedFriends = GRHelper.GRPawnComp(observerPawn).cachedNumberOfColonyFriends;
            }
            */
            float numOfAssessedFriends = RelationshipUtility.NumberOfFriends(assessed);
            float numOfObservedFriends = RelationshipUtility.NumberOfFriends(observer);

            var friendDifference = numOfAssessedFriends - numOfObservedFriends;
            if (friendDifference == 0f)
            {
                return 1f;
            }

            return Mathf.Pow((numOfAssessedFriends + 1f) / (numOfObservedFriends + 1f), FriendAttractionDampener);
        }
    }
}