using MerpBot.Interactive;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Services
{
    public class GlobalIDs
    {
        //MC IDs
        public const ulong spiritDenServer = 925491691916525578;
        public const ulong spiritDenModChannel = 926129159007268904;
        public const ulong spiritDenWelcomeMessage = 925973538748387399;
        public const ulong spiritDenMemberRole = 926001213558898729;
        public const ulong spiritDenWhitelistChannel = 925497255329415268;
        public const ulong spiritDenNotesChannel = 954086185804894258;
        public const ulong spiritDenLogChannel = 954086288095596604;
        public const ulong spiritDenAutosChannel = 958888955384856596;

        //katari IDs
        public const ulong katariServer = 889528268263407626;
        public const ulong katariServerGeneralChannel = 889528268699619359;
        public const ulong katariServerRulesChannel = 889528268263407628;
        public const ulong katariServerRolesChannel = 936051305548488764;
        public const ulong katariServerBotCommandsChannel = 941536375210127420;
        public const ulong katariServerNotesChannel = 942967550990487602;
        public const ulong katariServerModChannel = 940390463469404180;
        public const ulong katariServerLogChannel = 948371696308584518;
        public const ulong katariServerArtChannel = 889528268699619361;
        public const ulong katariServerAutosChannel = 958888225265569903;
        public const ulong katariServerKyveraArtChannel = 923238025792749616;
        public const ulong katariServerMemeChannel = 889533702676316210;

        //traso server 
        public const ulong trasoServer = 839238112974602240;
        public const ulong trasoServerSpiritDenChannel = 925993518147711017;
        public const ulong errorChannel = 878660332506202125;
        public const ulong trasoServerGeneralChannel = 921800123107930112;
        public const ulong trasoServerLogChannel = 953440800967127070;
        public const ulong trasoServerNotesChannel = 954472486651703376;
        public const ulong trasoServerAutosChannel = 958889016697167922;

        //talan bot stuff
        public const ulong talansBotId = 938592245890822144;
        public const ulong katariServerCountingChannel = 938591951048040548;

        //people
        public const ulong trasoId = 194108558177075201;
        public const ulong talanId = 267333357874970624;

        //michin emotes
        public const ulong michinEmotes = 878660332506202122;
        public const ulong botTesting = 931653464738660362;

        //ping replies
        public static string[] pingReplies =
        {
            "What did you say?", // 0
            "Water is good for you. Go drink some.",
            "Man when was the last time you touched grass?",
            "Whar.",
            "bro why you gotta do this to me",
            "AAAAAAAA",
            "Sbeve",
            "Coffeve", // 10
            "Papers please.",
            "This dude just tried to sell me 1 bread for 20 emeralds. What a ripoff.",
            "What is a Hollow Knight again?",
            "Talan was listening to Beyond The Heart by Lena Raine while typing this one out.",
            "Fun Fact: It's impossible to be good at For Honor",
            "```java\npublic class program {\n  public static void main(string[] args) {\n    System.out.println(\"This was a pain in the ass to type out\");\n  }\n}```",
            "```js\nconsole.log(\"me when\");```",
            "Hey <@267333357874970624>, can you tell them to stop pinging me?",
            "Please state your name and number and I won't ever get back to you.", // 20
            "The code, Talan! What does it mean?",
            "Ctrl + C and Ctrl + V are the best keybinds ever",
            "What did you expect? A cookie recipe?",
            "https://tenor.com/view/you-dare-ping-me-gif-22265112",
            "Can someone go tell that cheese wedge to stop merping? Please and thanks.",
            "I will send you to Jesus",
            "Emotional damage",
            "R U R' U'",
            "A Rubik's Cube has about 4 quintillion possible combinations",
            "Cubik's Rube", // 30
            "THROW THE CHEEEEEEEEEEEEEEEEEEEESE",
            "You're lookin' mighty sus there bud",
            "Did you know a semicolon is technically completely useless in every programming language? But some require them anyway.",
            "I need a car. Any car.",
            "My name's Crazy Dave, but you can call me Crazy Dave.",
            "Heartbeat sensor deployed.",
            "Beep beep boop bap",
            "Uno reverse card, I ping you instead! <@!<userid>>",
            "Press 'R' to reload.", // 40
            "Glory to Arstotzka!",
            "Hello, hello? Uh, I wanted to record a message for you to help you get settled in on your first night.",
            "I'm just *guitar noises*",
            "Five Nights at Freddy's. Is this really where you want to be? I just don't get it. Why do you want to stay?",
            "WAS THAT THE PING OF 87?!?!",
            "My user ID is 949475675532841001",
            "You have pinged me\nThis represents treason to the highest order.\nYour execution is scheduled for tomorrow.\nThe safety of your family is unknown.\nGlory to Arstotzka.\nhttps://youtu.be/J_3Zad-e9f4",
            "<a:OriCommunist:963251647830716446>\nOur ping!",
            "The pings <user>, what do they mean?",
            "Public static void main string args\nPublic static void main string args\nPublic static void main string args\nPublic static void main string args\nPublic static void main string args",
            "```py\nimport random;input(\"RPS:\")\nprint(\"you\",random.choice([\"won\",\"lost\",\"tied\"]))```",
            "The human eye can only see 2gb of ram",
            "I wish I was a cute anime gorl",
            "Nah cuz now this Bucky dude has to deal with a furry -Talan",
            "Florida Man codes shitty bot with Oregon Man\nThis just in: both are insane, please send help",
            "Some nice resources for learning programming and webdev\n<https://github.com/public-apis/public-apis>\n<https://roadmap.sh>\n<https://www.theodinproject.com/>\n<https://idea-instructions.com/>",
            "devlopers google\nhttps://stackoverflow.com/", // 60
            "```bf\n ++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.\n```"
        };
    }
    public class VolatileData
    {
        public Dictionary<ulong, DateTime> Time;
        public DateTime StartTime;
        public VolatileData()
        {
            Time = new Dictionary<ulong, DateTime>();
            StartTime = DateTime.Now;
        }
    }

    public class SystemInfo
    {
        public string ProcessorName = "";
        public string Usage = "";
        public string Cores = "";
        public string LogicalCores = "";
        public string MaxMemory = "";

        private DateTime _lastCheck;

        public SystemInfo()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            ManagementObjectSearcher mos2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            ManagementObjectSearcher mos3 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");

            foreach (ManagementObject mo in mos.Get())
            {
                ProcessorName = mo["Name"].ToString();
                Usage = mo["LoadPercentage"].ToString();
                Cores = mo["NumberOfCores"].ToString();
                LogicalCores = mo["NumberOfLogicalProcessors"].ToString();
            }

            foreach (ManagementObject mo in mos2.Get())
            {
                MaxMemory = mo["Capacity"].ToString();
            }

            _lastCheck = DateTime.Now;
        }
        ///  <summary>
        ///  Will automatically check if it's been 2 hours since the last update. If it hasn't, it will not update any parameters.
        ///  </summary>
        public void QuickRefresh()
        {
            if (_lastCheck.AddHours(2).Hour >= DateTime.Now.Hour) { Console.WriteLine("It has not been 2 hours, and parameters have not been updated as such."); return; }

            _lastCheck = DateTime.Now;

            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (ManagementObject mo in mos.Get())
            {
                Usage = mo["LoadPercentage"].ToString();
            }
        }
        /// <summary>
        /// Refreshes all statistics. May take a while.
        /// </summary>
        public void Refresh()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            ManagementObjectSearcher mos2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");

            foreach (ManagementObject mo in mos.Get())
            {
                ProcessorName = mo["Name"].ToString();
                Usage = mo["LoadPercentage"].ToString();
                Cores = mo["NumberOfCores"].ToString();
                LogicalCores = mo["NumberOfLogicalProcessors"].ToString();
            }

            foreach (ManagementObject mo in mos2.Get())
            {
                MaxMemory = mo["Capacity"].ToString();
            }
        }
    }

    
}