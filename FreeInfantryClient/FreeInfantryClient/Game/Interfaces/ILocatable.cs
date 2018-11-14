using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Protocol;

namespace FreeInfantryClient.Game
{
	/// <summary>
	/// Interface for a class that can be spatially tracked
	/// </summary>
	public interface ILocatable
	{
		ushort getID();
	}
}
