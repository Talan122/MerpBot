using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Interactive.Preconditions;

namespace MerpBot.Interactive;
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

    public static string GetShutdownResponse()
    {
        int Rand = new Random().Next(0, ShutdownResponses.Length - 1);
        return ShutdownResponses[Rand];
    }

    /// <summary>
    /// i hate this.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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
            .Replace("ni", "nyi");

        if (Result == input) Result = input + " OwO";

        return Result;
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
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
