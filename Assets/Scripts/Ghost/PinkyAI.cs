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
	public class PinkyAI : GhostAI
	{
		public PinkyAI ( PacmanData[] players, BoardAccessor Accessor, BoardObject Data ) 
			: base( players, Accessor, Data )
		{
		}

		public override IntVector2 ComputeDirection( List<IntVector2> legalTurns, int maxSpeed )
		{
			List<BoardLocation> targets = new List<BoardLocation>();
			foreach ( PacmanData player in Players )
			{
				targets.Add( player.Data.boardLocation + new BoardLocation( ( player.Data.direction.Normalized() * 5 ) , new IntVector2( 0, 0 ) ) );
			}
			
			return base.ComputeDirectionToTargets( targets, legalTurns, maxSpeed );
		}
	}
}

