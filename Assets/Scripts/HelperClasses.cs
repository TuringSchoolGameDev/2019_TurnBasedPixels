
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
	public int rowCount;
	public int columnCount;
	public List<Vector3> obstacles = new List<Vector3>();
	public List<Vector3> player0Objects = new List<Vector3>();
	public List<Vector3> player1Objects = new List<Vector3>();
}
