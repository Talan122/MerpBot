using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Interactive.Preconditions;

namespace MerpBot.Interactive
{
    public class OtherStuff
    {
        // These are just Portal 2 turret deaths.
        private static readonly string[] ShutdownResponses =
        {
            "Shutting down.",
            "Critical Error.",
            "I don't blame you.",
            "I don't hate you.",
            "Why...",
            "No hard feelings."
        };

        private static readonly string[] ChallengeResponses =
        {
            "Here's your challenge:",
            "Here's the next challenge:",
            "Here's what you asked for:",
            "I'm sorry, but:",
            "Here's yer next challenge matey!",
            "Wait... you wanted a challenge right?",
            "Oi! I got ya challenge here!"
        };

        public static string GetShutdownResponse()
        {
            int Rand = new Random().Next(0, ShutdownResponses.Length - 1);
            return ShutdownResponses[Rand];
        }

        public static string GetChallengeResponse()
        {
            int Rand = new Random().Next(0, ChallengeResponses.Length - 1);
            return ChallengeResponses[Rand] + "\n";
        }

        public static string OwOfy(string input)
        {
            string Result = input.Replace("R", "W")
                .Replace("r", "w")
                .Replace("No", "Nyo")
                .Replace("no", "nyo")
                .Replace("Na", "Nya")
                .Replace("na", "nya")
                .Replace("Ne", "Nye")
                .Replace("ne", "nye")
                .Replace("Nu", "Nyu")
                .Replace("nu", "nyu")
                .Replace("Ni", "Nyi")
                .Replace("ni", "nyi")
                .Replace("L", "W")
                .Replace("l", "w")
                .Replace("A", "Ya")
                .Replace("a", "ya");

            if (Result == input) Result = input + " OwO";

            return Result;
        }

        public static bool IsHighUser(SocketGuildUser user)
        {
            if (user.Roles.Any(r => r.Id == 937175477657944074)) return true;
            else if (SuperUser.SuperUsers.Contains(user.Id)) return true;
            else return false;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Checks for a blacklisted word.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns true if it DOES contain a blacklisted word.</returns>
        public static bool CheckForBlacklistedWords(string input)
        {
            bool allowed = false;

            foreach (var word in BlacklistedWords)
            {
                if (input.ToLower().Contains(word)) 
                { 
                    allowed = true;
                    Console.WriteLine($"{word} is blacklisted");
                    break;
                }
            }

            return allowed;
        }
        /// <summary>
        /// A list of blacklisted words.
        /// I have this blocked out in my IDE lol.
        /// </summary>
        private static string[] BlacklistedWords =
        {
            //"test",
            "nigga",
            "nigger",
            "whore",
            "faggot"
        };

        
    }

    public static class TemperatureConverter
    {
        public static double FahrenheitToCelcius(double fahrenheit)
        {
            double result = (fahrenheit - 32) * 5 / 9;
            return result;
        }

        public static double CelciusToFahrenheit(double celcius)
        {
            double result = (celcius * (9 / 5)) + 32;
            return result;
        }

        public static double CelciusToKelvin(double celcius)
        {
            double result = celcius + 273.15;
            return result;
        }

        public static double KelvinToCelcius(double kelvin)
        {
            double result = kelvin - 273.15;
            return result;
        }

        public static double FahrenheitToKelvin(double fahrenheit)
        {
            double result = FahrenheitToCelcius(fahrenheit) + 273.15;
            return result;
        }

        public static double KelvinToFahrenheit(double kelvin)
        {
            double result = CelciusToFahrenheit(kelvin - 273.15);
            return result;
        }
    }
}
