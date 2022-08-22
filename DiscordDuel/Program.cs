using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DaVDiscordBot
{
    class Program
    {

        public DiscordSocketClient Client;
        public CommandHandler Handler;

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {

            Client = new DiscordSocketClient();
            Handler = new CommandHandler();


            await Client.LoginAsync(Discord.TokenType.Bot, "NjAxMTU3OTQwMTU3NjEyMDY0.XS-OYA.cyB7m_p__ujMz5lIllgSpBiO9TA", true);
            await Client.StartAsync();

            await Handler.Install(Client);


            Client.Ready += Client_Ready;
            await Task.Delay(-1);
        }

        private async Task Client_Ready()
        {
            Console.WriteLine("The bot is online! (Storytime)");

            return;
        }
    }
}
