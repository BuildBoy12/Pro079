namespace Pro079.Logic
{
    using API.Interfaces;
    using Exiled.API.Features;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Extensions
    {
        /// <summary>
        /// Simple bool to check if the players role is a <see cref="RoleType.Scp079"/>.
        /// </summary>
        /// <param name="ply">Player to check.</param>
        /// <returns></returns>
        public static bool IsScp079(this Player ply) => ply.Role == RoleType.Scp079;

        /// <summary>
        /// Checks if the player has an active CASSIE command cooldown. Returns false if the player is not a Scp079.
        /// </summary>
        /// <param name="ply">Player object of the Scp079.</param>
        /// <returns></returns>
        public static bool OnCassieCooldown(this Player ply)
        {
            if (!Manager.UltimateCooldowns.ContainsKey(ply))
                return false;

            return Manager.CassieCooldowns[ply] > DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ply">The player object of the Scp079.</param>
        /// <param name="command">The command to check the cooldown of.</param>
        /// <returns></returns>
        public static bool OnCommandCooldown(this Player ply, ICommand079 command)
        {
            if (!Manager.CommandCooldowns.ContainsKey(ply))
                return false;

            return Manager.CommandCooldowns[ply].Contains(command);
        }

        /// <summary>
        /// Checks if the player has an active Ultimate cooldown. Returns false if the player is not a Scp079.
        /// </summary>
        /// <param name="ply">Player object of the Scp079.</param>
        /// <returns></returns>
        public static bool OnUltimateCooldown(this Player ply)
        {
            if (!Manager.UltimateCooldowns.ContainsKey(ply))
                return false;

            return Manager.UltimateCooldowns[ply] > DateTime.Now;
        }

        /// <summary>
        /// Optimized method that replaces a <see cref="string"/> based on an <see cref="Tuple[]"/>
        /// </summary>
        /// <param name="source">The string to use as source</param>
        /// <param name="token">The starting token</param>
        /// <param name="valuePairs">The value pairs (tuples) to use as "key -> value"</param>
        /// <returns>The string after replacement</returns>
        public static string ReplaceAfterToken(this string source, char token, Tuple<string, object>[] valuePairs)
        {
            if (valuePairs == null)
            {
                throw new ArgumentNullException("valuePairs");
            }

            StringBuilder builder = new StringBuilder(Convert.ToInt32(Math.Ceiling(source.Length * 1.5f)));

            int i = 0;
            int sourceLength = source.Length;

            do
            {
                // Append characters until you find the token
                char auxChar = token == char.MaxValue ? (char) (char.MaxValue - 1) : char.MaxValue;
                for (; i < sourceLength && (auxChar = source[i]) != token; i++) builder.Append(auxChar);

                // Ensures no weird stuff regarding token being null
                if (auxChar == token)
                {
                    int movePos = 0;

                    // Try to find a tuple
                    foreach (Tuple<string, object> kvp in valuePairs)
                    {
                        int j, k;
                        for (j = 0, k = i + 1;
                            j < kvp.Item1.Length && k < source.Length && source[k] == kvp.Item1[j];
                            j++, k++) ;
                        // General condition for "key found"
                        if (j == kvp.Item1.Length)
                        {
                            movePos = j;
                            builder.Append(kvp.Item2); // append what we're replacing the key with
                            break;
                        }
                    }

                    // Don't skip the token if you didn't find the key, append it
                    if (movePos == 0) builder.Append(token);
                    else i += movePos;
                }

                i++;
            } while (i < sourceLength);

            return builder.ToString();
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> source)
            => source.OrderBy(x => Guid.NewGuid()).ToList();

        public static List<T> PickRandom<T>(this IEnumerable<T> source, int count)
            => source.Shuffle().Take(count).ToList();
    }
}