using System;
using System.Collections.Generic;

public static class ExceptionManager
{
    /// <summary>
    /// Permet de lever une exception si un argument est null
    /// </summary>
    /// <param name="args">Un dictionnaire contenant les arguments et leur nom</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void CheckArgumentNull(Dictionary<object, string> args)
    {
        // Pour chaque argument
        foreach (KeyValuePair<object, string> arg in args)
        {
            // Si l'argument est null, lever une exception
            if (arg.Key == null) throw new ArgumentNullException($"{arg.Value} a été null");
        }
    }
}