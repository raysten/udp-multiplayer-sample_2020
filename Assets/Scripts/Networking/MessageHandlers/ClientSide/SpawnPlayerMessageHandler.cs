public class SpawnPlayerMessageHandler : BaseHandler<SpawnPlayerMessage>
{
    private PlayerSpawner _spawner;
	private GameLoop _loop;

    public SpawnPlayerMessageHandler(
        MessageProcessor messageProcessor,
        PlayerSpawner spawner,
		GameLoop loop
    ) : base(messageProcessor)
    {
        _spawner = spawner;
		_loop = loop;
	}

    public override void Handle(SpawnPlayerMessage message)
    {
		_loop.SetTickIndex(message.TickIndex + 10); // TODO:
        _spawner.SpawnPlayer(message.Sender.ToString());
    }
}
