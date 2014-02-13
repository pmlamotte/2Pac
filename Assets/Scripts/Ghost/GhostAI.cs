//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
namespace AssemblyCSharp
{
	public abstract class GhostAI
	{
		protected PacmanData[] Players;
		protected BoardAccessor Accessor;
		protected GhostData Data;
		
		public GhostAI( PacmanData[] players, BoardAccessor Accessor, GhostData Data )
		{
			this.Players = players;
			this.Accessor = Accessor;
			this.Data = Data;
		}

		public abstract IntVector2 ComputeDirection( List<IntVector2> legalTurns, int maxSpeed );

		public IntVector2 ProcessTurn(  List<IntVector2> legalTurns, int maxSpeed )
		{
			IntVector2 result = null;
			if ( GameData.Instance.isDirectional )
			{
				if ( Accessor.Data.DirectionIndex.ContainsKey( Data.boardLocation.location ) )
				{
					result = Accessor.Data.PossibleDirectionsMap[Data.boardLocation.location][Accessor.Data.DirectionIndex[Data.boardLocation.location]];
					Accessor.gameObject.BroadcastMessage( "GhostOverIntersection", Data.boardLocation.location );
				}
				else
				{
					// wander aimlessly, rare case.
					result = legalTurns[Constants.random.Next( legalTurns.Count - 1 )];
				}
			}
			else if ( Data.PlayersCanEat.Count > 0 )
			{
				// wander aimlessly
				result = legalTurns[Constants.random.Next( legalTurns.Count - 1 )];
			}
			else
			{
				result = ComputeDirection(legalTurns, maxSpeed );
			}
			return result;

		}

		public IntVector2 ComputeDirectionToTargets( HashSet<IntVector2> targets, List<IntVector2> legalTurns, int maxSpeed )
		{
			IntVector2 bestTurn = legalTurns[0];
			int bestTurnDistance = int.MaxValue;

			foreach ( IntVector2 turn in legalTurns )
			{
				IntVector2 orthTurn = turn.Normalized();
				orthTurn = orthTurn + Data.boardLocation.location;
				foreach ( IntVector2 target in targets )
				{
					int distance = IntVector2.OrthogonalDistance( orthTurn, target );
					if ( distance < bestTurnDistance )
					{
						bestTurnDistance = distance;
						bestTurn = turn;
					}
				}
			}

			return bestTurn;
		}
	}
}

