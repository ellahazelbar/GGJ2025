using MoreMountains.Feedbacks;

namespace Extensions
{
	static class MMF
	{
		public static void StopAndPlayFeedbacks(this MMF_Player player)
		{
			player.StopFeedbacks();
			player.PlayFeedbacks();
		}
	}
}