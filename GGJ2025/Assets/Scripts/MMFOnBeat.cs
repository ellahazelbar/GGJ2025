using MoreMountains.Feedbacks;

public class MMFOnBeat : MMF_Player
{
	private bool shouldBeat = true;
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
		if (shouldBeat)
		{
			PlayFeedbacks();
			shouldBeat = false;
		}
		else
		{
			shouldBeat = true;
		}
	}
}
