public class SpawnPlayerMessageHandler : BaseHandler<SpawnPlayerMessage>
{
    private PlayerSpawner _spawner;
	private GameLoop _loop;
	private RemoteClient _client;

    public SpawnPlayerMessageHandler(
        MessageProcessor messageProcessor,
        PlayerSpawner spawner,
		GameLoop loop,
		RemoteClient client
    ) : base(messageProcessor)
    {
        _spawner = spawner;
		_loop = loop;
		_client = client;
	}

    public override void Handle(SpawnPlayerMessage message)
    {
		_loop.SetTickIndex(message.TickIndex + 10); // TODO:
        _spawner.SpawnPlayer(message.Sender.ToString());
		_client.IsConnected = true;
    }
}
