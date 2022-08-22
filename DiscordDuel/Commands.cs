using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Driver.Builders;

namespace DaVDiscordBot
{
    public class Commands : ModuleBase
    {



        [Command("ping")]
        public async Task Ping()
        {
            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);
            await Context.Message.DeleteAsync();
            var msg = await ReplyAsync("Calculating ping...");

            builder.WithDescription($"Ping from {Context.User} is: {((DateTime.Now.Second * 1000) + DateTime.Now.Millisecond) - ((msg.Timestamp.Second * 1000) + msg.Timestamp.Millisecond)} ms");
            await ReplyAsync("", false, builder.Build());
            await msg.DeleteAsync();
        }
        [Command("bs")]
        public async Task BS()
        {
            var builder = new EmbedBuilder();

            builder.WithColor(Color.Red);
            await Context.Message.DeleteAsync();
            builder.WithDescription("asjfdlkjaskdghaslkdghaskl;dgjwijgiewjgaijdska;gsdkalalgjaiwe;dklsgagjaew;ijasdklgjaiweklsd;agksdlg;askldgjd;asdgl");
            await ReplyAsync("", false, builder.Build());

        }

        [Command("kick")]
        public async Task Kick(SocketGuildUser mention, string reason = null)
        {
            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);
            await Context.Message.DeleteAsync();
            var user = Context.User as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Role");
            if (mention != null && !mention.Roles.Contains(role))
            {
                // Do Stuff
                if (user.GuildPermissions.KickMembers)
                {




                    var channel = await mention.GetOrCreateDMChannelAsync();
                    await channel.SendMessageAsync(reason == null ? $"You've been kicked from {Context.Guild.Name}." : $"You've been kicked from {Context.Guild.Name} for {reason}.");
                    await Task.Delay(2000);
                    await mention.KickAsync();


                    builder.WithDescription(reason == null ? $"User {mention.Username} has been kicked." : $"User {mention.Username} has been kicked for {reason}.");
                    builder.WithColor(Color.Red);
                    await ReplyAsync("", false, builder.Build());
                }
                else
                {
                    builder.WithDescription($"Something went wrong, either you didn't execute the command properly or you don't have permission to kick that player.");
                    builder.WithColor(Color.Red);
                    await ReplyAsync("", false, builder.Build());
                }
            }
            else
            {
                builder.WithDescription($"Something went wrong, either you didn't execute the command properly or you don't have permission to kick that player.");
                builder.WithColor(Color.Red);
                await ReplyAsync("", false, builder.Build());
            }



        }


        [Command("help")]
        public async Task Help()
        {

            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);
            await Context.Message.DeleteAsync();
            builder.WithDescription($"{Context.User} has called the help command!\n\n" +
                $"Here are a list of commands you can call:\n" +
                $"[Command](parameter) : Description\n" +
                $"!ping : returns the users ping\n" +
                $"!info : brings up discord information\n" +
                $"----------------------\n" +
                $"Duel Commands\n" +
                $"----------------------\n" +
                $"!duel @UserMention : Sends a duel request to the user you mention.\n" +
                $"!acceptduel : Accepts a duel request.\n" +
                $"!declineduel : Declines a duel request.\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                 $"Have suggestions? Send the Admin or Moderators a message!");
            await ReplyAsync("", false, builder.Build());
        }

        [Command("info")]
        public async Task Info()
        {
            await Context.Message.DeleteAsync();

            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);

            builder.WithDescription($"You're currently in a Text Channel for StoryTime an Oldschool Runescape Community!\n" +
                $"Type !help if you want some more commands!\n" +

                $"Try Checking out the #duels text channel!\n" +
                $"If the channel isn't open check out Darkness At Valgnar discord for another arena: https://discord.gg/nJgRqj2 \n" +
                $"Email: ThirdPartitionDevelopment@Gmail.com \n");

            await ReplyAsync("", false, builder.Build());


        }




        [Command("duel")]
        public async Task InitiateDuel(SocketGuildUser opponent = null, int stakeAmount = 0)
        {
            
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);



            if (opponent == null)
            {
                builder.WithDescription($"You need to specify a user by @mentioning them after typing !duel [ !duel @<user> ]");
                await ReplyAsync("", false, builder.Build());
                return;
            }

            if (Duel.isDueling)
            {
                builder.WithDescription($"Foolish {Context.User.Username}! There's a duel already under way!");
                await ReplyAsync("", false, builder.Build());
                return;

            }
            if (Duel.isRequesting && (Math.Abs((float)(System.DateTime.Now.Subtract(Duel.requestTime).TotalSeconds)) < 30))
            {
                builder.WithDescription($"Foolish {Context.User.Username}! There's a request already under way! It will be accepted or time out soon! (Try again in 30 seconds)");
                await ReplyAsync("", false, builder.Build());
                return;
            }
            else
            {
                if (Duel.isRequesting)
                {


                    builder1 = ($"The duel request between {Duel.p1Name} and {Duel.p2Name} has been cancelled to make way for another duel!" +
                        $"\n\n\n" +
                        $"");

                }

                Duel.p1Name = Context.User.Username;
                Duel.p1Discriminator = Context.User.Discriminator;
                Console.WriteLine("P1 set");
                if (await CreateAccount("" + Context.User.Username + "#" + Context.User.Discriminator))
                {
                   
                    b1.WithColor(Color.DarkGreen);
                    b1.AddField($"" + Context.User.Username + "#" + Context.User.Discriminator + " has been added to DaVDiscordDuel Database, your wins and losses are now being tracked " +
                        $"while your username remains as it is now. \n\nYou have been given 10m coins to begin staking with. Good Luck.", "-----");
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Duelist");
                await (Context.User as IGuildUser).AddRoleAsync(role);

                //ADD CHECK HERE FOR IF THE PLAYER HAS ENOUGH GOLD
                Duel.stake = stakeAmount;
                if (await FindCoinsInAccount("" + Context.User.Username + "#" + Context.User.Discriminator) < stakeAmount)
                {

                    b1.WithColor(Color.Red);
                    b1.WithDescription($"{Context.User.Mention}");
                    b1.AddField($"You don't have enough coins! \nType !coins to see how much you have.", "-----");
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();

                    return;
                }

                Duel.p2Name = opponent.Username;
                Duel.p2Discriminator = opponent.Discriminator;
                Console.WriteLine("p2 set");
                builder2 = ($"{opponent.Mention}, {Context.User.Username} wishes to duel!\n\n" +
                    $"They are offering to stake {stakeAmount} coins!\n\n" +
                    $"You have 30 seconds to type !acceptduel to begin!\n" +
                    $"You can also type !declineduel to decline.");
                builder.WithDescription(builder1 + builder2);
                Duel.isRequesting = true;
                Duel.isWhip = false;
                Duel.requestTime = System.DateTime.Now;
                await ReplyAsync("", false, builder.Build());




            }
        }
       
        [Command("acceptduel")]
        public async Task AcceptDuel()
        {
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();


            var b2 = new EmbedBuilder();
            var b3 = new EmbedBuilder();
            var b4 = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            string builder3 = "";
            string builder4 = "";
            b1.WithColor(Color.DarkGreen);
            b2.WithColor(Color.DarkGreen);
            b3.WithColor(Color.DarkGreen);
            b4.WithColor(Color.Gold);


            if (Duel.isDueling)
            {
                b1.WithDescription($"{Context.User.Username}, There's a duel already in progress!");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }

            if (!Duel.isRequesting)
            {
                b1.WithDescription($"{Context.User.Username}, There's no duel right now, make one by typing !duel <@user>");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }


            if (!(Context.User.Username == Duel.p2Name))
            {
                b1.WithDescription($"{Context.User.Username}, you aren't the player that has a duel request...");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }


            if (await CreateAccount("" + Context.User.Username + "#" + Context.User.Discriminator))
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.DarkGreen);
                b1.AddField($"" + Context.User.Username + "#" + Context.User.Discriminator + "has been added to DaVDiscordDuel Database, your wins and losses are now being tracked " +
                    $"while your username remains as it is now. \n\nYou have been given 10m coins to begin staking with. Good Luck.", "-----");
                await ReplyAsync("", false, b1.Build());
                b1 = new EmbedBuilder();
            }
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Duelist");
            await (Context.User as IGuildUser).AddRoleAsync(role);
            await Task.Delay(3000);
            if (await FindCoinsInAccount("" + Context.User.Username + "#" + Context.User.Discriminator) < Duel.stake)
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.WithDescription($"{Context.User.Mention}");
                b1.AddField($"You don't have enough coins! \nType !coins to see how much you have.", "-----");
                await ReplyAsync("", false, b1.Build());
                b1 = new EmbedBuilder();

                return;
            }

            builder1 = ($"The duel between {Duel.p1Name} and {Duel.p2Name} has begun!\n\n");
            b1.WithDescription(builder1);
            await ReplyAsync("", false, b1.Build());

            Duel.isRequesting = false;
            Duel.isDueling = true;




            Random random = new Random();
            float p1Rand = random.Next(101);
            float p2Rand = random.Next(101);
            while (p1Rand == p2Rand)
            {
                p1Rand = random.Next(101);
                p2Rand = random.Next(101);
            }

            b3.AddField($"{Duel.p1Name} rolled: {p1Rand}\n" +
                $"{Duel.p2Name} rolled: {p2Rand}", "--------", false);
            if (p1Rand > p2Rand)
            {
                Duel.currentPlayerMove = 1;
                b3.AddField($"{Duel.p1Name} won the roll and gets the first move!\n\n", "--------", false);
            }
            else
            {
                Duel.currentPlayerMove = 2;
                b3.AddField($"{Duel.p2Name} won the roll and gets the first move!\n\n", "--------", false);

            }

            await ReplyAsync("", false, b3.Build());


            

            Duel.p1Health = 99;
            Duel.p1Spec = 100;
            Duel.p1Sharks = 15;
            Duel.p1CombatDoses = 4;
            Duel.p1AntiDoses = 4;
            Duel.p1StatModifier = 1.15f;
            Duel.p1Karams = 4;
            Duel.p1PoisonTurns = 0;

            Duel.p2Health = 99;
            Duel.p2Spec = 100;
            Duel.p2Sharks = 15;
            Duel.p2CombatDoses = 4;
            Duel.p2AntiDoses = 4;
            Duel.p2StatModifier = 1.15f;
            Duel.p2Karams = 4;
            Duel.p2PoisonTurns = 0;

            //THIS SECTION IS WHIPPING
            if (Duel.isWhip)
            {
                b2.AddField($"{Duel.p1Name}", "--------", false);
                b2.AddField($"Health: {Duel.p1Health}        ", "--------", false);

                b2.AddField($"{Duel.p2Name}", "--------", false);
                b2.AddField($"Health: {Duel.p2Health}        ", "--------", false);

               
                await ReplyAsync("", false, b2.Build());
                await Task.Delay(3000);
                while(Duel.p1Health > 0 && Duel.p2Health > 0)
                {
                    if(Duel.currentPlayerMove == 1)
                    {
                        b1 = new EmbedBuilder();
                        int damageDealt;
                        Random rand = new Random();
                        b1.AddField($"{Duel.p1Name} lashes out with their whip!", "-----------", false);


                        if (rand.NextDouble() <= .2d)
                        {
                            damageDealt = 0;
                        }
                        else
                        {
                            damageDealt = (int)(rand.NextDouble() * 25 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        }
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        double splashModifier1 = rand.NextDouble();

                        if (splashModifier1 <= .2d)
                        {
                            damageDealt = 0;
                        }
                        Duel.p2Health -= damageDealt;
                        Duel.currentPlayerMove = 2;
                        b1.AddField($"It deals {damageDealt}! \nLeaving {Duel.p2Name} with {Duel.p2Health} hp!", "-----------", true);
                        b1.WithColor(Color.Gold);
                        await ReplyAsync("", false, b1.Build());

                      
                        await Task.Delay(3000);
                       
                    }
                    else
                    {
                        b1 = new EmbedBuilder();
                        int damageDealt;
                        Random rand = new Random();
                        b1.AddField($"{Duel.p2Name} lashes out with their whip!", "-----------", false);


                        if (rand.NextDouble() <= .2d)
                        {
                            damageDealt = 0;
                        }
                        else
                        {

                            damageDealt = (int)(rand.NextDouble() * 25 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        }
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        double splashModifier1 = rand.NextDouble();

                        if (splashModifier1 <= .2d)
                        {
                            damageDealt = 0;
                        }
                        Duel.p1Health -= damageDealt;

                        b1.AddField($"It deals {damageDealt}! \nLeaving {Duel.p1Name} with {Duel.p1Health} hp!", "-----------", true);
                        b1.WithColor(Color.Blue);
                        await ReplyAsync("", false, b1.Build());

                        Duel.currentPlayerMove = 1;
                        await Task.Delay(3000);
                    }

                }

                if (Duel.p1Health <= 0)
                {
                    Mongo db = new Mongo();
                    db.Init();
                    Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                    Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                    player2Account.Kills += 1;
                    player1Account.Deaths += 1;
                    player2Account.Coins += Duel.stake + 50000;
                    player1Account.Coins -= Duel.stake;
                    if (player1Account.Deaths != 0)
                    {
                        player1Account.KD = player1Account.Kills / player1Account.Deaths;
                    }
                    if (player2Account.Deaths != 0)
                    {
                        player2Account.KD = player2Account.Kills / player2Account.Deaths;
                    }


                    db.UpdateAccount(player1Account, player1Account.Username);
                    db.UpdateAccount(player2Account, player2Account.Username);

                    b1 = new EmbedBuilder();
                    b1.AddField($"Oh Dear, {Duel.p1Name}, you are dead.\n\n" +
                        $"{Duel.p2Name} has won the duel and collected 50k for playing!\n\n" +
                         $"{Duel.p2Name} has collected {Duel.stake} coins from {Duel.p1Name}!\n\n", "----", false);
                    b1.AddField($"{Duel.p1Name} has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                            $"{Duel.p2Name} has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                        $"The arena is free once again!", "--------", false);
                    await ReplyAsync("", false, b1.Build());
                    Duel.isDueling = false;
                    Duel.isRequesting = false;



                    return;
                }
                else if (Duel.p2Health <= 0)
                {
                    Mongo db = new Mongo();
                    db.Init();
                    Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                    Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                    player1Account.Kills += 1;
                    player2Account.Deaths += 1;
                    player1Account.Coins += Duel.stake + 50000;
                    player2Account.Coins -= Duel.stake;

                    if (player1Account.Deaths != 0)
                    {
                        player1Account.KD = player1Account.Kills / player1Account.Deaths;
                    }
                    if (player2Account.Deaths != 0)
                    {
                        player2Account.KD = player2Account.Kills / player2Account.Deaths;
                    }


                    db.UpdateAccount(player1Account, player1Account.Username);
                    db.UpdateAccount(player2Account, player2Account.Username);


                    b1 = new EmbedBuilder();
                    b1.AddField($"Oh Dear, {Duel.p2Name}, you are dead.\n\n" +
                        $"{Duel.p1Name} has won the duel and collected 50k for playing!\n\n" +
                         $"{Duel.p1Name} has collected {Duel.stake} coins from {Duel.p2Name}!\n\n", "----", false);
                    b1.AddField($"{Duel.p1Name} has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                            $"{Duel.p2Name} has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                        $"The arena is free once again!", "--------", false);
                    await ReplyAsync("", false, b1.Build());
                    Duel.isDueling = false;
                    Duel.isRequesting = false;



                    return;

                }


            }


            //BELOW IS STUFF FOR A NORMAL DUEL
            b2.AddField($"{Duel.p1Name}", "--------", false);
            b2.AddField($"Health: {Duel.p1Health}        " +
                $"Spec: {Duel.p1Spec}%\n" +
                $"Sharks: {Duel.p1Sharks}        " +
                $"Spr. Cmb: {Duel.p1CombatDoses}\n" +
                 $"Karams: {Duel.p1Karams}        " +
                $"Poison Turns: {Duel.p1PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p1AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p1StatModifier - 1) * 100)}\n", "--------", false);

            b2.AddField($"{Duel.p2Name}", "--------", false);
            b2.AddField($"Health: {Duel.p2Health}        " +
                $"Spec: {Duel.p2Spec}%\n" +
                $"Sharks: {Duel.p2Sharks}        " +
                $"Spr. Cmb: {Duel.p2CombatDoses}\n" +
                  $"Karams: {Duel.p2Karams}        " +
                $"Poison Turns: {Duel.p2PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p2AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p2StatModifier - 1) * 100)}\n", "--------", false);





            if (Duel.currentPlayerMove == 1)
            {
                b2.AddField($"\n\nIt is {Duel.p1Name}'s move.", "--------", false);
            }
            else
            {
                b2.AddField($"\n\nIt is {Duel.p2Name}'s move.", "--------", false);
            }
            b2.AddField($"If you get stuck type !duelhelp", "--------", false);
            await ReplyAsync("", false, b2.Build());




        }

        [Command("declineduel")]
        public async Task DeclineDuel()
        {
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();


            if (Duel.isDueling)
            {
                b1.WithDescription($"{Context.User.Username}, There's a duel already in progress!");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }
            if (!Duel.isRequesting)
            {
                b1.WithDescription($"{Context.User.Username}, There's no duel right now, make one by typing !duel <@user>");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }

            if (!(Context.User.Username == Duel.p2Name))
            {
                b1.WithDescription($"{Context.User.Username}, you aren't the player that has a duel request...");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }
            var builder = new EmbedBuilder();


            builder.WithColor(Color.Red);
            Duel.isRequesting = false;
            Duel.isDueling = false;


            builder.WithDescription($"{Duel.p2Name} has declined the duel. The arena is free once again!");
            Duel.p1Name = "";
            Duel.p2Name = "";
            await ReplyAsync("", false, builder.Build());





        }


        [Command("forfeit")]
        public async Task Forfeit()
        {
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            if ((!(Context.User.Username == Duel.p2Name)) && (!(Context.User.Username == Duel.p1Name)))
            {
                b1.WithDescription($"{Context.User.Username}, you aren't in a duel currently.");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }

            if (Context.User.Username == Duel.p2Name)
            {
                Mongo db = new Mongo();
                db.Init();
                Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                player1Account.Kills += 1;
                player2Account.Deaths += 1;
                player1Account.Coins += Duel.stake;
                player2Account.Coins -= Duel.stake;
                if (player1Account.Deaths != 0)
                {
                    player1Account.KD = player1Account.Kills / player1Account.Deaths;
                }
                if (player2Account.Deaths != 0)
                {
                    player2Account.KD = player2Account.Kills / player2Account.Deaths;
                }

                db.UpdateAccount(player1Account, player1Account.Username);
                db.UpdateAccount(player2Account, player2Account.Username);


                b1.AddField($"{Duel.p2Name} has forfeited!\n\n" +
                    $"{Duel.p1Name} has won the duel!\n\n" +
                     $"{Duel.p1Name} has collected {Duel.stake} coins from {Duel.p2Name}!\n\n", "----", false);
                b1.AddField($"{Duel.p1Name} now has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                        $"{Duel.p2Name} now has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                    $"The arena is free once again!", "--------", false);
                await ReplyAsync("", false, b1.Build());
                Duel.isDueling = false;
                Duel.isRequesting = false;


                return;


            }
            else
            {
                Mongo db = new Mongo();
                db.Init();
                Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                player2Account.Kills += 1;
                player1Account.Deaths += 1;
                player2Account.Coins += Duel.stake;
                player1Account.Coins -= Duel.stake;
                if (player1Account.Deaths != 0)
                {
                    player1Account.KD = player1Account.Kills / player1Account.Deaths;
                }
                if (player2Account.Deaths != 0)
                {
                    player2Account.KD = player2Account.Kills / player2Account.Deaths;
                }

                db.UpdateAccount(player1Account, player1Account.Username);
                db.UpdateAccount(player2Account, player2Account.Username);


                b1.AddField($"{Duel.p1Name} has forfeited!\n\n" +
                    $"{Duel.p2Name} has won the duel!\n\n" +
                     $"{Duel.p2Name} has collected {Duel.stake} coins from {Duel.p1Name}!\n\n", "----", false);
                b1.AddField($"{Duel.p1Name} now has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                        $"{Duel.p2Name} now has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                    $"The arena is free once again!", "--------", false);
                await ReplyAsync("", false, b1.Build());
                Duel.isDueling = false;
                Duel.isRequesting = false;

            }


        }

        [Command("duelhelp")]
        public async Task DuelInformation()
        {
            var builder = new EmbedBuilder();
            builder.WithColor(Color.DarkGreen);

            builder.WithDescription($"{Context.User} has called the !duelhelp command!\nYou must be in the #duels text channel to execute these commands.\n"
                  + $"Here are a list of commands you can call in a duel:\n"
                  + $"[Command](parameter) : Description\n\n"
                  + $"IF YOU WISH TO STAKE SOMEONE TYPE !duel <@mention> [stake amount]\n\n"
                   + $"IF YOU WISH TO WHIP SOMEONE TYPE !whip <@mention> [stake amount]\n\n"
           + $"!attack dds : uses 25% spec. and applies poison\n"
                  + $"!attack bloodbarrage : heals for 25% of damage dealt.\n"
                  + $"!attack shadowbarrage : drains stats by 3.\n"
                  + $"!attack smokebarrage : applies poison\n"
                  + $"!attack whip : A strong basic attack.\n"
                  + $"!attack ags : Uses 50% spec.\n\n"
                   + $"!attack dh :attacks with Dharok's (does more damage the lower health you are).\n\n"
            + $"!eat shark: Uses one shark to heal for 20.\n"
                  + $"!eat karam : Uses one karam to heal for 16.\n"
                  + $"!eat karam+shark : Heals for 36.\n"
                  + $"!eat combat: Restores 6 combat levels.\n"
                  + $"!eat combat+shark: Restores 6 combat levels and uses one shark to heal for 20.\n"
                  + $"!eat anti: Restores poison.\n"
                  + $"!eat anti+shark: Restores poison and uses one shark to heal for 20.\n\n"
           + $"!forfeit : forfeits the match\n"
                  + $"!hs [Coins/Deaths/KD] : Shows the top 10 players on the server for the parameter\n"
                  + $"!Coins  : The DuelBot will slide into your DMs, telling you how many coins you have.\n"
                  + $"!info : brings up discord information\n");
            await ReplyAsync("", false, builder.Build());


        }
        [Command("attack")]
        public async Task Attack(string attackType = null)
        {
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            b1.WithColor(Color.DarkGreen);
            if ((!(Context.User.Username == Duel.p2Name)) && (!(Context.User.Username == Duel.p1Name)))
            {
                b1.WithDescription($"{Context.User.Username}, you aren't in a duel currently.");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }
            bool appliedPoison = false;
            if ((Duel.currentPlayerMove == 1 && Context.User.Username == Duel.p1Name))
            {
                Random rand = new Random();
                int damageDealt = 0;
                int posionChance = 0;
                float combatReduction = 0f;

                if (attackType.ToLower() == "whip")
                {
                    b1.AddField($"{Duel.p1Name} lashes out with their whip!", "-----------", false);




                    damageDealt = (int)(rand.NextDouble() * 38 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    double splashModifier1 = rand.NextDouble();

                    if (splashModifier1 <= .2d)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"It deals {damageDealt}!", "-----------", true);
                    b1.WithColor(Color.Gold);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "dh")
                {
                    if (rand.NextDouble() <= .2d)
                    {
                        damageDealt = 0;
                    }
                    else{
                        damageDealt = (int)(rand.NextDouble() * (Math.Abs(Duel.p1Health - 94)) * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                         }
                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"{Duel.p1Name} quickly switches into dh and madly cleaves their axe! \nIt deals {damageDealt}!", "-----------", true);
                    b1.WithColor(Color.Gold);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "shadowbarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .2d)
                    {
                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Shadow Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.DarkGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        combatReduction = .06f;
                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Shadow Barrage! It deals {damageDealt} and lowers {Duel.p2Name}'s stats by 6!", "--------", false);
                        b1.WithColor(Color.DarkGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }

                }
                else if (attackType.ToLower() == "dds")
                {
                    if (Duel.p1Spec < 25)
                    {
                        b1.AddField($"{Duel.p1Name}, you don't have enough special energy, use another command!", "-----------", true);
                        b1.WithColor(Color.Red);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }


                    int damageDealt1 = (int)(rand.NextDouble() * 28 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                    if (damageDealt1 < 0) { damageDealt1 = 0; }
                    int damageDealt2 = (int)(rand.NextDouble() * 28 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                    if (damageDealt2 < 0) { damageDealt2 = 0; }
                    double splashModifier1 = rand.NextDouble();

                    if (splashModifier1 <= .35d)
                    {
                        damageDealt1 = 0;
                    }
                    else
                    {
                        appliedPoison = true;
                    }
                    double splashModifier2 = rand.NextDouble();

                    if (splashModifier2 <= .35d)
                    {
                        damageDealt2 = 0;
                    }
                    else
                    {
                        appliedPoison = true;
                    }
                    damageDealt = damageDealt1 + damageDealt2;
                    Duel.p1Spec -= 25.0f;

                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    string poisonString = "";
                    if (appliedPoison)
                    {
                        poisonString = $"{Duel.p2Name} is now poisoned!";
                    }
                    b1.AddField($"{Duel.p1Name} slashes with their dds! \nIt deals {damageDealt1}-{damageDealt2}!\n{poisonString}", "-----------", true);
                    b1.WithColor(Color.DarkMagenta);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "bloodbarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .3d)
                    {
                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Blood Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        Duel.p1Health += damageDealt / 4;
                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Blood Barrage! It deals {damageDealt} to {Duel.p2Name} and heals {Duel.p1Name} for {damageDealt / 4}!", "--------", false);
                        b1.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }


                }
                else if (attackType.ToLower() == "ags")
                {
                    if (Duel.p1Spec < 50)
                    {
                        b1.AddField($"{Duel.p1Name}, you don't have enough special energy, use another command!", "-----------", true);
                        b1.WithColor(Color.Red);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }


                    int damageDealt1 = (int)(rand.NextDouble() * 67 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                    if (damageDealt1 < 0) { damageDealt1 = 0; }

                    double splashModifier1 = rand.NextDouble();

                    if (splashModifier1 <= .3d)
                    {
                        damageDealt1 = 0;
                    }

                    damageDealt = damageDealt1;
                    Duel.p1Spec -= 50.0f;

                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"{Duel.p1Name} wildly twirls in the air with their Armadyl Godsword! \nIt somehow deals {damageDealt1}!", "-----------", true);
                    b1.WithColor(Color.Blue);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "smokebarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .2d)
                    {
                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Smoke Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.LighterGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        appliedPoison = true;

                        b1.AddField($"{Duel.p1Name} summons ancient magic to cast Smoke Barrage! It deals {damageDealt} and poisons {Duel.p2Name}!", "--------", false);
                        b1.WithColor(Color.LighterGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }

                }

                else
                {
                    b1.AddField($"{Duel.p1Name}, that command isn't recognized!\n\n" +
                        $"Type !duelhelp if you need a list of commands.", "--------", false);
                    b1.WithColor(Color.Red);
                    await ReplyAsync("", false, b1.Build());
                    return;

                }
                //END OF ATACKS----------------------------------

                await UpdateP1(damageDealt, combatReduction, appliedPoison);


            }
            //Code for player2
            else if ((Duel.currentPlayerMove == 2 && Context.User.Username == Duel.p2Name))
            {
                Random rand = new Random();
                int damageDealt = 0;

                float combatReduction = 0f;

                if (attackType.ToLower() == "whip")
                {

                    damageDealt = (int)(rand.NextDouble() * 38 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"{Duel.p2Name} lashes out with their whip! \nIt deals {damageDealt}!", "-----------", true);
                    b1.WithColor(Color.Gold);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "dh")
                {
                    if (rand.NextDouble() <= .2d)
                    {
                        damageDealt = 0;
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * (Math.Abs(Duel.p2Health - 94)) * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                    }
                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"{Duel.p2Name} quickly switches into dh and madly cleaves their axe! \nIt deals {damageDealt}!", "-----------", true);
                    b1.WithColor(Color.Gold);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "shadowbarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .2f)
                    {
                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Shadow Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.DarkGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        combatReduction = .06f;
                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Shadow Barrage! It deals {damageDealt} and lowers {Duel.p1Name}'s stats by 6!", "--------", false);
                        b1.WithColor(Color.DarkGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }

                }
                else if (attackType.ToLower() == "dds")
                {
                    if (Duel.p2Spec < 25)
                    {
                        b1.AddField($"{Duel.p2Name}, you don't have enough special energy, use another command!", "-----------", true);
                        b1.WithColor(Color.Red);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }



                    int damageDealt1 = (int)(rand.NextDouble() * 28 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                    if (damageDealt1 < 0) { damageDealt1 = 0; }
                    int damageDealt2 = (int)(rand.NextDouble() * 28 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                    if (damageDealt2 < 0) { damageDealt2 = 0; }

                    double splashModifier1 = rand.NextDouble();
                    if (splashModifier1 <= .35d)
                    {
                        damageDealt1 = 0;
                    }
                    else
                    {
                        appliedPoison = true;
                    }
                    double splashModifier2 = rand.NextDouble();

                    if (splashModifier2 <= .35d)
                    {
                        damageDealt2 = 0;
                    }
                    else
                    {
                        appliedPoison = true;
                    }
                    damageDealt = damageDealt1 + damageDealt2;
                    Duel.p2Spec -= 25.0f;

                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    string poisonString = "";
                    if (appliedPoison)
                    {
                        poisonString = $"{Duel.p1Name} is now poisoned!";
                    }
                    b1.AddField($"{Duel.p2Name} slashes with their dds! \nIt deals {damageDealt1}-{damageDealt2}!\n{poisonString}", "-----------", true);
                    b1.WithColor(Color.DarkMagenta);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();


                }

                else if (attackType.ToLower() == "bloodbarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .3d)
                    {
                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Blood Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        Duel.p2Health += damageDealt / 4;
                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Blood Barrage! It deals {damageDealt} to {Duel.p1Name} and heals {Duel.p2Name} for {damageDealt / 4}!", "--------", false);
                        b1.WithColor(Color.DarkRed);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                }
                else if (attackType.ToLower() == "ags")
                {
                    if (Duel.p2Spec < 50)
                    {
                        b1.AddField($"{Duel.p2Name}, you don't have enough special energy, use another command!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1.WithColor(Color.Red);
                        b1 = new EmbedBuilder();
                        return;
                    }


                    int damageDealt1 = (int)(rand.NextDouble() * 67 * (1 + (Duel.p2StatModifier - Duel.p1StatModifier)));
                    if (damageDealt1 < 0) { damageDealt1 = 0; }

                    double splashModifier1 = rand.NextDouble();

                    if (splashModifier1 <= .3d)
                    {
                        damageDealt1 = 0;
                    }

                    damageDealt = damageDealt1;
                    Duel.p2Spec -= 50.0f;

                    if (damageDealt < 0)
                    {
                        damageDealt = 0;
                    }
                    b1.AddField($"{Duel.p2Name} wildly twirls in the air with their Armadyl Godsword! \nIt somehow deals {damageDealt1}!", "-----------", true);
                    b1.WithColor(Color.Blue);
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                else if (attackType.ToLower() == "smokebarrage")
                {
                    double splashModifier = rand.NextDouble();

                    if (splashModifier <= .2d)
                    {
                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Smoke Barrage! It splashes!", "--------", false);
                        b1.WithColor(Color.LighterGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }
                    else
                    {
                        damageDealt = (int)(rand.NextDouble() * 32 * (1 + (Duel.p1StatModifier - Duel.p2StatModifier)));
                        if (damageDealt < 0)
                        {
                            damageDealt = 0;
                        }
                        appliedPoison = true;

                        b1.AddField($"{Duel.p2Name} summons ancient magic to cast Smoke Barrage! It deals {damageDealt} and poisons {Duel.p1Name}!", "--------", false);
                        b1.WithColor(Color.LighterGrey);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                    }

                }



                else
                {
                    b1.AddField($"{Duel.p2Name}, that command isn't recognized!\n\n" +
                        $"Type !duelhelp if you need a list of commands.", "--------", false);
                    b1.WithColor(Color.Red);
                    await ReplyAsync("", false, b1.Build());
                    return;

                }

                //END OF ATACKS----------------------------------


                await UpdateP2(damageDealt, combatReduction, appliedPoison);



            }

            //
            else
            {
                b1.WithDescription($"{Context.User.Username}, it's not your turn.");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }



        }

        [Command("eat")]
        public async Task Eat(string eatType = null)
        {
            var b1 = new EmbedBuilder();

            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            b1.WithColor(Color.DarkGreen);
            if ((!(Context.User.Username == Duel.p2Name)) && (!(Context.User.Username == Duel.p1Name)))
            {
                b1.WithDescription($"{Context.User.Username}, you aren't in a duel currently.");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }
            if ((Duel.currentPlayerMove == 1 && Context.User.Username == Duel.p1Name))
            {
                if (eatType.ToLower() == "shark")
                {
                    if (Duel.p1Sharks <= 0)
                    {
                        b1.WithDescription("You don't have any sharks left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a shark! \nIt heals for 20!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p1Sharks -= 1;
                        Duel.p1Health += 20;
                        if (Duel.p1Health > 99)
                        {
                            Duel.p1Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "karam+shark")
                {
                    if (Duel.p1Sharks <= 0 || Duel.p1Karams <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and karams left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a shark and karam simultaneously! \nIt heals for 36!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p1Sharks -= 1;
                        Duel.p1Karams -= 1;
                        Duel.p1Health += 36;
                        if (Duel.p1Health > 99)
                        {
                            Duel.p1Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "karam")
                {
                    if (Duel.p1Karams <= 0)
                    {
                        b1.WithDescription("You don't have that many karams left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a karambwan! \nIt heals for 16!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p1Karams -= 1;
                        Duel.p1Health += 16;
                        if (Duel.p1Health > 99)
                        {
                            Duel.p1Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "combat")
                {
                    if (Duel.p1CombatDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many super combat potions to drink. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a super combat! \nIt restores 6 stats!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p1StatModifier += .06f;
                        Duel.p1CombatDoses -= 1;
                        if (Duel.p1StatModifier > 1.15f)
                        {
                            Duel.p1StatModifier = 1.15f;
                        }
                    }

                }
                else if (eatType.ToLower() == "combat+shark")
                {
                    if (Duel.p1Sharks <= 0 || Duel.p1CombatDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and combat potions left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a shark and combat dose simultaneously! \nIt heals for 20 and restores 6 stats!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p1Sharks -= 1;
                        Duel.p1CombatDoses -= 1;
                        Duel.p1Health += 20;
                        Duel.p1StatModifier += .06f;
                        if (Duel.p1Health > 99)
                        {
                            Duel.p1Health = 99;
                        }
                        if (Duel.p1StatModifier > 1.15f)
                        {
                            Duel.p1StatModifier = 1.15f;
                        }
                    }

                }
                else if (eatType.ToLower() == "anti+shark")
                {
                    if (Duel.p1Sharks <= 0 || Duel.p1AntiDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and anti potions left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} eats a shark and anti-poison dose simultaneously! \nIt heals for 20 and removes poison!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p1Sharks -= 1;
                        Duel.p1AntiDoses -= 1;
                        Duel.p1Health += 20;
                        Duel.p1PoisonTurns = 0;

                        if (Duel.p1Health > 99)
                        {
                            Duel.p1Health = 99;
                        }

                    }

                }
                else if (eatType.ToLower() == "anti")
                {
                    if (Duel.p1AntiDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many anti-poison potions to drink. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p1Name} drinks an anti poison dose! \nIt removes poison!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p1PoisonTurns = 0;
                        Duel.p1AntiDoses -= 1;

                    }

                }

                else
                {
                    b1.AddField($"{Duel.p1Name}, that command isn't recognized!\n\n" +
                        $"Type !duelhelp if you need a list of commands.", "--------", false);
                    b1.WithColor(Color.Red);
                    await ReplyAsync("", false, b1.Build());
                    return;

                }


                await UpdateP1(0, 0f, false);


            }
            else if ((Duel.currentPlayerMove == 2 && Context.User.Username == Duel.p2Name))
            {

                if (eatType.ToLower() == "shark")
                {
                    if (Duel.p2Sharks <= 0)
                    {
                        b1.WithDescription("You don't have any sharks left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a shark! \nIt heals for 20!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p2Sharks -= 1;
                        Duel.p2Health += 20;
                        if (Duel.p2Health > 99)
                        {
                            Duel.p2Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "karam+shark")
                {
                    if (Duel.p2Sharks <= 0 || Duel.p2Karams <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and karams left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a shark and karam simultaneously! \nIt heals for 36!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p2Sharks -= 1;
                        Duel.p2Karams -= 1;
                        Duel.p2Health += 36;
                        if (Duel.p2Health > 99)
                        {
                            Duel.p2Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "karam")
                {
                    if (Duel.p2Karams <= 0)
                    {
                        b1.WithDescription("You don't have that many karams left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a karambwan! \nIt heals for 16!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p2Karams -= 1;
                        Duel.p2Health += 16;
                        if (Duel.p2Health > 99)
                        {
                            Duel.p2Health = 99;
                        }
                    }

                }
                else if (eatType.ToLower() == "combat")
                {
                    if (Duel.p2CombatDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many super combat potions to drink. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a super combat! \nIt restores 6 stats!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p2StatModifier += .06f;
                        Duel.p2CombatDoses -= 1;
                        if (Duel.p2StatModifier > 1.15f)
                        {
                            Duel.p2StatModifier = 1.15f;
                        }
                    }

                }
                else if (eatType.ToLower() == "combat+shark")
                {
                    if (Duel.p2Sharks <= 0 || Duel.p2CombatDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and combat potions left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a shark and combat dose simultaneously! \nIt heals for 20 and restores 6 stats!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p2Sharks -= 1;
                        Duel.p2CombatDoses -= 1;
                        Duel.p2Health += 20;
                        Duel.p2StatModifier += .06f;
                        if (Duel.p2Health > 99)
                        {
                            Duel.p2Health = 99;
                        }
                        if (Duel.p2StatModifier > 1.15f)
                        {
                            Duel.p2StatModifier = 1.15f;
                        }
                    }

                }
                else if (eatType.ToLower() == "anti+shark")
                {
                    if (Duel.p2Sharks <= 0 || Duel.p2AntiDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many sharks and anti potions left to eat. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} eats a shark and anti-poison dose simultaneously! \nIt heals for 20 and removes poison!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        Duel.p2Sharks -= 1;
                        Duel.p2AntiDoses -= 1;
                        Duel.p2Health += 20;
                        Duel.p2PoisonTurns = 0;

                        if (Duel.p2Health > 99)
                        {
                            Duel.p2Health = 99;
                        }

                    }

                }
                else if (eatType.ToLower() == "anti")
                {
                    if (Duel.p2AntiDoses <= 0)
                    {
                        b1.WithDescription("You don't have that many anti-poison potions to drink. Use another command.");
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();
                        return;
                    }
                    else
                    {
                        b1.AddField($"{Duel.p2Name} drinks an anti poison dose! \nIt removes poison!", "-----------", true);
                        await ReplyAsync("", false, b1.Build());
                        b1 = new EmbedBuilder();

                        Duel.p2PoisonTurns = 0;
                        Duel.p2AntiDoses -= 1;

                    }

                }

                else
                {
                    b1.AddField($"{Duel.p2Name}, that command isn't recognized!\n\n" +
                        $"Type !duelhelp if you need a list of commands.", "--------", false);
                    b1.WithColor(Color.Red);
                    await ReplyAsync("", false, b1.Build());
                    return;

                }

                await UpdateP2(0, 0f, false);

            }
            else
            {
                b1.WithDescription($"{Context.User.Username}, it's not your turn.");
                b1.WithColor(Color.Red);
                await ReplyAsync("", false, b1.Build());
                return;
            }




        }

        public async Task UpdateP2(int damageDealt, float combatReduction, bool appliedPoison)
        {
            var b1 = new EmbedBuilder();
            b1.WithColor(Color.DarkGreen);
            Duel.p1Health -= damageDealt;
            Duel.p1StatModifier -= combatReduction;
            if (Duel.p1StatModifier < 1)
            {
                Duel.p1StatModifier += .01f;
            }
            else if (Duel.p1StatModifier > 1)
            {
                Duel.p1StatModifier -= .01f;
            }
            if (Duel.p1PoisonTurns > 0)
            {
                int poisonDamage = (int)(Duel.p1PoisonTurns);
                Duel.p1Health -= poisonDamage;
                Duel.p1PoisonTurns--;
                var b2 = new EmbedBuilder();
                b2.WithColor(Color.Green);
                b2.AddField($"{Duel.p1Name} was hit by poison for {poisonDamage}!", "----", false);
                await ReplyAsync("", false, b2.Build());
            }
            if (appliedPoison)
            {
                Duel.p1PoisonTurns = 6;
            }


            if (Duel.p1Health <= 0)
            {
                Mongo db = new Mongo();
                db.Init();
                Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                player2Account.Kills += 1;
                player1Account.Deaths += 1;
                player2Account.Coins += Duel.stake + 50000;
                player1Account.Coins -= Duel.stake;
                if (player1Account.Deaths != 0)
                {
                    player1Account.KD = player1Account.Kills / player1Account.Deaths;
                }
                if (player2Account.Deaths != 0)
                {
                    player2Account.KD = player2Account.Kills / player2Account.Deaths;
                }


                db.UpdateAccount(player1Account, player1Account.Username);
                db.UpdateAccount(player2Account, player2Account.Username);


                b1.AddField($"Oh Dear, {Duel.p1Name}, you are dead.\n\n" +
                    $"{Duel.p2Name} has won the duel and collected 50k for playing!\n\n" +
                     $"{Duel.p2Name} has collected {Duel.stake} coins from {Duel.p1Name}!\n\n", "----", false);
                b1.AddField($"{Duel.p1Name} has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                        $"{Duel.p2Name} has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                    $"The arena is free once again!", "--------", false);
                await ReplyAsync("", false, b1.Build());
                Duel.isDueling = false;
                Duel.isRequesting = false;



                return;
            }

            Duel.currentPlayerMove = 1;
            if (Duel.p1Spec <= 97.5f)
            {
                Duel.p1Spec += 2.5f;
            }
            if (Duel.p2Spec <= 97.5f)
            {
                Duel.p2Spec += 2.5f;
            }


            b1.AddField($"{Duel.p1Name}", "--------", false);
            b1.AddField($"Health: {Duel.p1Health}        " +
                $"Spec: {Duel.p1Spec}%\n" +
                $"Sharks: {Duel.p1Sharks}        " +
                $"Spr. Cmb: {Duel.p1CombatDoses}\n" +
                 $"Karams: {Duel.p1Karams}        " +
                $"Poison Turns: {Duel.p1PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p1AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p1StatModifier - 1) * 100)}\n", "--------", false);

            b1.AddField($"{Duel.p2Name}", "--------", false);
            b1.AddField($"Health: {Duel.p2Health}        " +
                $"Spec: {Duel.p2Spec}%\n" +
                $"Sharks: {Duel.p2Sharks}        " +
                $"Spr. Cmb: {Duel.p2CombatDoses}\n" +
                  $"Karams: {Duel.p2Karams}        " +
                $"Poison Turns: {Duel.p2PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p2AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p2StatModifier - 1) * 100)}\n", "--------", false);


            if (Duel.currentPlayerMove == 1)
            {
                b1.AddField($"\n\nIt is {Duel.p1Name}'s move.", "--------", false);
            }
            else
            {
                b1.AddField($"\n\nIt is {Duel.p2Name}'s move.", "--------", false);
            }
            b1.AddField($"If you get stuck type !duelhelp", "--------", false);
            await ReplyAsync("", false, b1.Build());


        }

        public async Task UpdateP1(int damageDealt, float combatReduction, bool appliedPoison)
        {
            var b1 = new EmbedBuilder();
            b1.WithColor(Color.DarkGreen);
            Duel.p2Health -= damageDealt;
            Duel.p2StatModifier -= combatReduction;
            if (Duel.p2StatModifier < 1)
            {
                Duel.p2StatModifier += .01f;
            }
            else if (Duel.p2StatModifier > 1)
            {
                Duel.p2StatModifier -= .01f;
            }
            if (Duel.p2PoisonTurns > 0)
            {
                int poisonDamage = (int)(Duel.p2PoisonTurns);
                Duel.p2Health -= poisonDamage;
                Duel.p2PoisonTurns--;
                var b2 = new EmbedBuilder();
                b2.WithColor(Color.Green);
                b2.AddField($"{Duel.p2Name} was hit by poison for {poisonDamage}!", "----", false);
                await ReplyAsync("", false, b2.Build());

            }
            if (appliedPoison)
            {
                Duel.p2PoisonTurns = 6;
            }

            if (Duel.p2Health <= 0)
            {
                Mongo db = new Mongo();
                db.Init();
                Model_Account player1Account = db.FindAccountInDatabase(Duel.p1Name + "#" + Duel.p1Discriminator);
                Model_Account player2Account = db.FindAccountInDatabase(Duel.p2Name + "#" + Duel.p2Discriminator);
                player1Account.Kills += 1;
                player2Account.Deaths += 1;
                player1Account.Coins += Duel.stake + 50000;
                player2Account.Coins -= Duel.stake;

                if (player1Account.Deaths != 0)
                {
                    player1Account.KD = player1Account.Kills / player1Account.Deaths;
                }
                if (player2Account.Deaths != 0)
                {
                    player2Account.KD = player2Account.Kills / player2Account.Deaths;
                }


                db.UpdateAccount(player1Account, player1Account.Username);
                db.UpdateAccount(player2Account, player2Account.Username);



                b1.AddField($"Oh Dear, {Duel.p2Name}, you are dead.\n\n" +
                    $"{Duel.p1Name} has won the duel and collected 50k for playing!\n\n" +
                     $"{Duel.p1Name} has collected {Duel.stake} coins from {Duel.p2Name}!\n\n", "----", false);
                b1.AddField($"{Duel.p1Name} has {player1Account.Kills} Kills, {player1Account.Deaths} Deaths, with a K/D Ratio of {player1Account.KD}!\n\n" +
                        $"{Duel.p2Name} has {player2Account.Kills} Kills, {player2Account.Deaths} Deaths, with a K/D Ratio of {player2Account.KD}!\n\n" +
                    $"The arena is free once again!", "--------", false);
                await ReplyAsync("", false, b1.Build());
                Duel.isDueling = false;
                Duel.isRequesting = false;



                return;

            }

            Duel.currentPlayerMove = 2;
            if (Duel.p1Spec <= 97.5f)
            {
                Duel.p1Spec += 2.5f;
            }
            if (Duel.p2Spec <= 97.5f)
            {
                Duel.p2Spec += 2.5f;
            }


            b1.AddField($"{Duel.p1Name}", "--------", false);
            b1.AddField($"Health: {Duel.p1Health}        " +
                $"Spec: {Duel.p1Spec}%\n" +
                $"Sharks: {Duel.p1Sharks}        " +
                $"Spr. Cmb: {Duel.p1CombatDoses}\n" +
                 $"Karams: {Duel.p1Karams}        " +
                $"Poison Turns: {Duel.p1PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p1AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p1StatModifier - 1) * 100)}\n", "--------", false);

            b1.AddField($"{Duel.p2Name}", "--------", false);
            b1.AddField($"Health: {Duel.p2Health}        " +
                $"Spec: {Duel.p2Spec}%\n" +
                $"Sharks: {Duel.p2Sharks}        " +
                $"Spr. Cmb: {Duel.p2CombatDoses}\n" +
                  $"Karams: {Duel.p2Karams}        " +
                $"Poison Turns: {Duel.p2PoisonTurns}\n" +
                $"Anti-Posion: {Duel.p2AntiDoses}        " +
                $"Pot Boost: {(int)((Duel.p2StatModifier - 1) * 100)}\n", "--------", false);

            if (Duel.currentPlayerMove == 1)
            {
                b1.AddField($"\n\nIt is {Duel.p1Name}'s move.", "--------", false);
            }
            else
            {
                b1.AddField($"\n\nIt is {Duel.p2Name}'s move.", "--------", false);
            }
            b1.AddField($"If you get stuck type !duelhelp", "--------", false);
            await ReplyAsync("", false, b1.Build());



        }

        //[Command("createaccountforduel")]
        public async Task<bool> CreateAccount(string username)
        {
            Console.WriteLine("Creating a new account for {username}");


            Mongo db = new Mongo();
            db.Init();

            if (db.FindAccountInDatabase(username) != null)
            {
                return false;
            }

            Model_Account account = new Model_Account();
            account.Username = username;
            account.JoinedOn = System.DateTime.UtcNow;
            account.Coins = 10000000;
            db.InsertNewAccount(account);

            return true;



        }
        public async Task<int> FindCoinsInAccount(string username)
        {
            Mongo db = new Mongo();
            db.Init();

            int coins = 0;
            Model_Account account = db.FindAccountInDatabase(username);
            if (account != null)
            {
                coins = account.Coins;
            }


            return coins;

        }
        [Command("coins")]
        public async Task MessageCoinsToUser()
        {
            if (Context.Message.Channel.Name != "duels")
            {
                EmbedBuilder b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            var channel = await Context.User.GetOrCreateDMChannelAsync();
            int coins = 0;
            coins = await FindCoinsInAccount("" + Context.User.Username + "#" + Context.User.Discriminator);

            await channel.SendMessageAsync($"You have {coins} coins right now!");
        }

        [Command("hs")]
        public async Task DisplayHighscores(string param = null)
        {
            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #Duels text channel to duel and execute duel-related commands! (This is so other channels dont become unusable, sorry!).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            Mongo db = new Mongo();
            db.Init();

            var users = db.FindAllUsers();

            b1.WithColor(Color.Gold);

            if (param == null)
            {
                users.SetSortOrder(SortBy.Descending("Kills"));
                b1.WithDescription($"Ranking by Kills.");
            }
            else
            {
                try
                {
                    users.SetSortOrder(SortBy.Descending(param));
                    b1.WithDescription($"Ranking by {param}.");
                }
                catch
                {
                    b1.WithDescription($"parameter not recognized");
                    await ReplyAsync("", false, b1.Build());
                    return;
                }
            }


            int i = 1;
            foreach (Model_Account user in users)
            {

                if (i < 11)
                {
                    b1.AddField("Rank " + i + ": " + user.Username, $"Kills: {user.Kills},Deaths: {user.Deaths}, Coins: {user.Coins}", false);
                }
                i++;
            }





            await ReplyAsync("", false, b1.Build());

        }

        [Command("whip")]
        public async Task InitiateWhip(SocketGuildUser opponent = null, int stakeAmount = 0)
        {

            var b1 = new EmbedBuilder();
            if (Context.Message.Channel.Name != "duels")
            {
                b1 = new EmbedBuilder();
                b1.WithColor(Color.Red);
                b1.AddField("Head over to #duels text channel to duel! (This is so other channels dont become unusable).", "----");
                await ReplyAsync("", false, b1.Build());
                return;
            }

            await Context.Message.DeleteAsync();

            var builder = new EmbedBuilder();
            string builder1 = "";
            string builder2 = "";
            builder.WithColor(Color.Gold);



            if (opponent == null)
            {
                builder.WithDescription($"You need to specify a user by @mentioning them after typing !duel [ !duel @<user> ]");
                await ReplyAsync("", false, builder.Build());
                return;
            }

            if (Duel.isDueling)
            {
                builder.WithDescription($"Foolish {Context.User.Username}! There's a duel already under way!");
                await ReplyAsync("", false, builder.Build());
                return;

            }
            if (Duel.isRequesting && (Math.Abs((float)(System.DateTime.Now.Subtract(Duel.requestTime).TotalSeconds)) < 30))
            {
                builder.WithDescription($"Foolish {Context.User.Username}! There's a request already under way! It will be accepted or time out soon! (Try again in 30 seconds)");
                await ReplyAsync("", false, builder.Build());
                return;
            }
            else
            {
                if (Duel.isRequesting)
                {


                    builder1 = ($"The duel request between {Duel.p1Name} and {Duel.p2Name} has been cancelled to make way for another duel!" +
                        $"\n\n\n" +
                        $"");

                }

                Duel.p1Name = Context.User.Username;
                Duel.p1Discriminator = Context.User.Discriminator;
                if (await CreateAccount("" + Context.User.Username + "#" + Context.User.Discriminator))
                {

                    b1.WithColor(Color.DarkGreen);
                    b1.AddField($"" + Context.User.Username + "#" + Context.User.Discriminator + " has been added to DaVDiscordDuel Database, your wins and losses are now being tracked " +
                        $"while your username remains as it is now. \n\nYou have been given 10m coins to begin staking with. Good Luck.", "-----");
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();
                }
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Duelist");
                await (Context.User as IGuildUser).AddRoleAsync(role);

                //ADD CHECK HERE FOR IF THE PLAYER HAS ENOUGH GOLD
                Duel.stake = stakeAmount;
                if (await FindCoinsInAccount("" + Context.User.Username + "#" + Context.User.Discriminator) < stakeAmount)
                {

                    b1.WithColor(Color.Red);
                    b1.WithDescription($"{Context.User.Mention}");
                    b1.AddField($"You don't have enough coins! \nType !coins to see how much you have.", "-----");
                    await ReplyAsync("", false, b1.Build());
                    b1 = new EmbedBuilder();

                    return;
                }

                Duel.p2Name = opponent.Username;
                Duel.p2Discriminator = opponent.Discriminator;
                builder2 = ($"{opponent.Mention}, {Context.User.Username} wishes to duel with a whip only!\n\n" +
                    $"They are offering to stake {stakeAmount} coins!\n\n" +
                    $"You have 30 seconds to type !acceptduel to begin!\n" +
                    $"You can also type !declineduel to decline.");
                builder.WithDescription(builder1 + builder2);
                Duel.isRequesting = true;
                Duel.isWhip = true;
                Duel.requestTime = System.DateTime.Now;
                await ReplyAsync("", false, builder.Build());




            }
        }


    }

}