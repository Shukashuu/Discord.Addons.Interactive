using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SampleApp
{
    public class Program
    {
        private static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;
        private static IConfigurationRoot _configuration;
        
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
            
            _services = ConfigureServices();
            
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _commands = _services.GetRequiredService<CommandService>();

            _client.Log += log =>
            {
                Console.WriteLine(log.ToString());
                return Task.CompletedTask;
            };

            var token = _configuration["Token"];
            
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            
            await _commands.AddModulesAsync(typeof(Program).Assembly, _services);
            
            _client.MessageReceived += HandleCommandAsync;

            await Task.Delay(-1);
        }

        private static IServiceProvider ConfigureServices()
        {
            var client = new DiscordSocketClient();
            var commands = new CommandService();

            return new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .AddSingleton(_configuration)
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();
        }

        private static async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)
                || !(message.Author is IGuildUser guildUser)
                || guildUser.IsBot)
            {
                return;
            }

            var argPos = 0;
            if (message.HasStringPrefix("!!", ref argPos))
            {
                var context = new SocketCommandContext(_client, message);
                await _commands.ExecuteAsync(context, argPos, _services);
            }
        }
    }
}