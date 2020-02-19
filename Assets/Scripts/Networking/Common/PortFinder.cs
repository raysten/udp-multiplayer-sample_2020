using System.Linq;
using System.Net.NetworkInformation;

public class PortFinder
{
	public int GetAvailablePort(int startingPort)
	{
		var properties = IPGlobalProperties.GetIPGlobalProperties();

		var tcpConnectionPorts = properties.GetActiveTcpConnections()
			.Where(n => n.LocalEndPoint.Port >= startingPort)
			.Select(n => n.LocalEndPoint.Port);

		var tcpListenerPorts = properties.GetActiveTcpListeners()
			.Where(n => n.Port >= startingPort)
			.Select(n => n.Port);

		var udpListenerPorts = properties.GetActiveUdpListeners()
			.Where(n => n.Port >= startingPort)
			.Select(n => n.Port);

		var port = Enumerable.Range(startingPort, ushort.MaxValue)
			.Where(i => !tcpConnectionPorts.Contains(i))
			.Where(i => !tcpListenerPorts.Contains(i))
			.Where(i => !udpListenerPorts.Contains(i))
			.FirstOrDefault();

		return port;
	}
}
