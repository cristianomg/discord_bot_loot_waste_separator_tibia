using Application;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace bot_discord_loot.Application
{
    public static class Program
    {
        private static DiscordClient? _client;

        static void Main(string[] args)
        {
            DotEnv.Load();
            var token = Environment.GetEnvironmentVariable("private_key");
            if(!string.IsNullOrEmpty(token)){
                Console.WriteLine("Iniciando bot...");
                RunBotAsync(token!).GetAwaiter().GetResult();
            }
            else 
            Console.WriteLine("Token não encontrado.");
        }

        public static async Task RunBotAsync(string token)
        {

            var config = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };

            _client = new DiscordClient(config);
            _client.MessageCreated += Cliente_Mensagem;

            await _client.ConnectAsync();

            await Task.Delay(-1);
        }

        private static async Task Cliente_Mensagem(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
                await Task.CompletedTask;
            else
            {
                var message = e.Message.Content;
                var huntSession = new HuntSessionFactory().Create(message);
                
                if (huntSession == null){
                    await Task.CompletedTask;
                    return;
                } 

                Console.WriteLine("Enviando calculo.");

                var sessionResult = huntSession?.CalculateSession();

                await e.Channel.SendMessageAsync(sessionResult?.ToString());

                Console.WriteLine("Calculo enviado.");


            }
        }
    }


   

}