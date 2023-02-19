using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Services;
public class Settings
{
    private static Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>()
    {
        {"AtSomeone", false }

    };

    /// <summary>
    /// Should only be necessary for making new settings in runtime. Otherwise, specify them above.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void AddSetting(string name, bool value)
    {
        keyValuePairs[name] = value;
    }

    /// <summary>
    /// Tells you if the setting exists.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Returns true if it does, false if not.</returns>
    public static bool DoesSettingExist(string name)
    {
        if (keyValuePairs.ContainsKey(name)) return true;
        return false;
    }

    /// <summary>
    /// Returns the setting's value.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool GetSettingValue(string name)
    {
        if (!DoesSettingExist(name)) throw new ArgumentNullException();
        return keyValuePairs[name];
    }

    /// <summary>
    /// Sets a setting in runtime.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SetSettingValue(string name, bool value)
    {
        if (!DoesSettingExist(name)) throw new ArgumentNullException();
        keyValuePairs[name] = value;
    } 
}
