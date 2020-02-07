public class SpawnPlayerMessageHandler : BaseHandler<SpawnPlayerMessage>
{
    private PlayerSpawner _spawner;

    public SpawnPlayerMessageHandler(
        MessageProcessor messageProcessor,
        PlayerSpawner spawner
    ) : base(messageProcessor)
    {
        _spawner = spawner;
    }

    public override void Handle(IUdpMessage message)
    {
        _spawner.SpawnPlayer();
    }
}
