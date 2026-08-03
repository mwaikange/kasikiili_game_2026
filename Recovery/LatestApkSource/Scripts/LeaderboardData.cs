using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardData
{
	public string active_referrals;

	public string player_position;

	public List<LeaderboardPerson> candidates;

	public LeaderboardPerson player;
}
