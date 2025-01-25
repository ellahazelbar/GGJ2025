using MoreMountains.Feedbacks;

public class MMFOnBeat : MMF_Player
{
	protected override void Awake()
	{
		base.Awake();
		BaseLineBeat.Beat += OnBeat;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		BaseLineBeat.Beat -= OnBeat;
	}

	private void OnBeat()
	{
		PlayFeedbacks();
	}
}
