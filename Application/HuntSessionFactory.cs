using System;
using static System.Collections.Specialized.BitVector32;

namespace Application
{
    public class HuntSessionFactory
    {
        private readonly Dictionary<string, bool> keyValueParts
            = new Dictionary<string, bool>() {
                { "Session:", true },
                { "Loot Type:", true },
                { "Loot:", true },
                { "Supplies:", true },
                { "Balance:", true }
            };
        public HuntSessionFactory()
        {

        }

        public HuntSession? Create(string clipboard)
        {

            var parts = clipboard.Split("\n");

            var sessionPart = GetSessionPart(parts);

            if (!Verify(sessionPart)) return default;

            var huntSession = new HuntSession(sessionPart);

            var usersPart = GetUsersPart(parts);

            huntSession.AddUsers(usersPart);

            return huntSession;
        }



        public string[] GetSessionPart(string[] parts)
            => parts.Skip(1).Take(5).ToArray();

        public string[] GetUsersPart(string[] parts)
            => parts.Skip(6).ToArray();

        public bool Verify(string[] parts) =>
            parts.Count() == 5
            && parts[0].Contains("Session:")
            && parts[1].Contains("Loot Type:")
            && parts[2].Contains("Loot:")
            && parts[3].Contains("Supplies:")
            && parts[4].Contains("Balance:");




    }
}

