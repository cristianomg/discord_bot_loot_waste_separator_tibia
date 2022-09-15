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
            RunBotAsync().GetAwaiter().GetResult();
        }

        public static async Task RunBotAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("private_key"),
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
                
                if (huntSession == null) 
                    await e.Channel.SendMessageAsync("Clipboard invalido...");

                Console.WriteLine("Enviando calculo.");

                var sessionResult = huntSession?.CalculateSession();

                await e.Channel.SendMessageAsync(sessionResult?.ToString());

                Console.WriteLine("Calculo enviado.");


            }
        }
    }


   

}