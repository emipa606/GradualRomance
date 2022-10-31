using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Popularity : AttractionCalculator
{
    private const float FriendAttractionDampener = 0.4f;

    public override bool Check(Pawn observer, Pawn assessed)
    {
        return observer.IsColonist && assessed.IsColonist;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        float numOfAssessedFriends = RelationshipUtility.NumberOfFriends(assessed);
        float numOfObservedFriends = RelationshipUtility.NumberOfFriends(observer);

        var friendDifference = numOfAssessedFriends - numOfObservedFriends;
        return friendDifference == 0f
            ? 1f
            : Mathf.Pow((numOfAssessedFriends + 1f) / (numOfObservedFriends + 1f), FriendAttractionDampener);
    }
}