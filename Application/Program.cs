using System.Globalization;
using Application;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;

namespace bot_discord_loot.Application
{
    public static class Program
    {
        private static DiscordClient? _client;

        static void Main(string[] args)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "pt-BR")
                Thread.CurrentThread.CurrentCulture = new  CultureInfo("en-US");
            DotEnv.Load();
            var token = Environment.GetEnvironmentVariable("private_key");
            if (!string.IsNullOrEmpty(token)){

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
                MinimumLogLevel = LogLevel.Error,
                Intents = DiscordIntents.All
            };

            _client = new DiscordClient(config);
            _client.MessageCreated += Cliente_Mensagem;

            _client.SocketErrored += (sender, args) =>
            {
                Console.Error.WriteLine($"Erro de soquete: {args.Exception}");
                return Task.CompletedTask;
            };

            _client.ClientErrored += (sender, args) =>
            {
                Console.Error.WriteLine($"Erro do cliente: {args.Exception}");
                return Task.CompletedTask;
            };

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
                Console.WriteLine("Mensagem recebida: ");
                Console.WriteLine(message);

                var huntSession = new HuntSessionFactory().Create(message);
                
                if (huntSession == null)
                {
                    return;
                } 

                Console.WriteLine("Enviando calculo.");

                var sessionResult = huntSession?.CalculateSession();

                if (sessionResult == null)
                {
                    return;
                }

                var embed = new DiscordEmbedBuilder()
                {
                    Title = sessionResult.SessionData,
                    Description = @$"Profit total: {sessionResult.TotalProfit}
                                     Profit individual: {sessionResult.Profit}",
                    Color = DiscordColor.Blue
                };

                sessionResult.Payments.ForEach(x =>
                {
                    embed.AddField($"From: {x.From.Name} To: {x.To.Name}", x.BankMessage, inline:false);
                });

                await e.Message.RespondAsync(embed: embed);
            }
        }
    }


   

}