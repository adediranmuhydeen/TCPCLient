using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using TCPApp;

IPAddress ip = IPAddress.Parse("127.0.0.1");
TcpListener server = new TcpListener(ip, 8080);
server.Start();
Console.WriteLine("Server started on " + server.LocalEndpoint);
while (true)
{
    Console.WriteLine("Waiting for a connection...");
    TcpClient client = server.AcceptTcpClient();
    Task.Run(() => HandleClient(client));

    static void HandleClient(TcpClient client)
    {
        Console.WriteLine("Connected to client " + ((IPEndPoint)client.Client.RemoteEndPoint).ToString());
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);

        string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        var discount = JsonConvert.DeserializeObject<Discount>(data);
        List<string> result = new List<string>();
        
        try
        {
            result = CodeService.GenerateDiscountCodesAsync(discount.NumberOfCode, discount.Length);
            foreach( var code in result )
            {
                Console.WriteLine(code);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error generating codes, please try again later");
        }
        var response = JsonConvert.SerializeObject(result);
        //Console.WriteLine("Received message: " + response);
        byte[] responseBuffer = Encoding.ASCII.GetBytes(response);

        stream.Write(responseBuffer, 0, responseBuffer.Length);
        Console.WriteLine("Response sent.");
        stream.Close();
        //client.Close();
    }
}