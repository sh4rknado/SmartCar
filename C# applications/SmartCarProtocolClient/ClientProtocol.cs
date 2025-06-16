using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using ResultPattern;
using SmartCarProtocol;

namespace SmartCarProtocolClient
{
    public class ClientProtocol
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private readonly string _serverIp;
        private readonly int _serverPort;
        
        public ClientProtocol(string serverIp, int serverPort)
        {
            ArgumentNullException.ThrowIfNull(serverIp);
            ArgumentNullException.ThrowIfNull(serverPort);
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public bool? IsConnected => _client?.Connected;

        public async Task<Result> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_serverIp, _serverPort);
                _stream = _client.GetStream();
                return Result.Success();
            }
            catch (SocketException ex)
            {
                return Result.Failure(new(nameof(ClientProtocol), $"Connection error: {ex.Message}"));
            }
            catch (Exception ex) 
            { 
                return Result.Failure(new(nameof(ClientProtocol), $"Unexpected error during the connection: {ex.Message}"));
            }
        }

        public void Disconnect()
        {
            try
            {
                _stream?.Close();
                _client?.Close();
            }
            catch (ObjectDisposedException) { }
        }

        public async Task<Result> SendMessageAsync(ProtocolMessage message)
        {
            if (_client == null || !_client.Connected || _stream == null)
                return Result.Failure(new(nameof(ClientProtocol), "Non connecté au serveur."));
            
            try
            {
                string jsonString = JsonSerializer.Serialize(message);
                byte[] messageBytes = Encoding.UTF8.GetBytes(jsonString);
                byte[] lengthPrefix = BitConverter.GetBytes(messageBytes.Length); // Préfixe de 4 octets

                await _stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await _stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new(nameof(ClientProtocol), $"Error occured during Send to server with the reason:{ex.Message}"));
            }
        }

        // Méthode simplifiée pour lire une réponse (pour la requête d'état)
        private async Task<string> ReadResponseAsync()
        {
            if (_stream == null) return null;

            try
            {
                byte[] lengthBuffer = new byte[4];
                int bytesRead = await _stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                if (bytesRead == 0) return null; // Serveur déconnecté

                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                if (messageLength <= 0 || messageLength > 4096) return null; // Taille invalide

                byte[] messageBuffer = new byte[messageLength];
                int totalBytesRead = 0;
                while (totalBytesRead < messageLength)
                {
                    bytesRead = await _stream.ReadAsync(messageBuffer, totalBytesRead, messageLength - totalBytesRead);
                    if (bytesRead == 0) return null; // Serveur déconnecté
                    totalBytesRead += bytesRead;
                }

                string response = Encoding.UTF8.GetString(messageBuffer, 0, messageLength);
                Console.WriteLine($"Réponse reçue: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture de la réponse: {ex.Message}");
                return null;
            }
        }
    }
}
