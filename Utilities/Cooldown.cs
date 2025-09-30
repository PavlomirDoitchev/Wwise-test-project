
public class Cooldown
{
    private float duration;
    private float timer;

    public bool IsActive => timer > 0f;
    public bool IsFinished => timer <= 0f;

    public Cooldown(float duration)
    {
        this.duration = duration;
        timer = 0f;
    }

    public void Start()
    {
        timer = duration;
    }

    public void Tick(float deltaTime)
    {
        if (timer > 0f)
            timer -= deltaTime;
    }

    public void Reset()
    {
        timer = duration;
    }
}
