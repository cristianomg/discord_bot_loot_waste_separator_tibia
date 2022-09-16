using System;
using static System.Collections.Specialized.BitVector32;

namespace Application
{
    public class HuntSessionFactory
    {
        public HuntSessionFactory()
        {

        }

        public HuntSession? Create(string clipboard)
        {
            Console.WriteLine("Criando sessao");

            if (string.IsNullOrEmpty(clipboard?.Trim()))
            {
                Console.WriteLine("Clipboard vazio");
                return default;
            } 

            var parts = clipboard.Split(Environment.NewLine);

            Console.WriteLine("Separado as partes");

            var sessionPart = GetSessionPart(parts);

            if (!Verify(sessionPart)) 
            {
                Console.WriteLine("Sessao invalida.");
                return default;
            }

            var huntSession = new HuntSession(sessionPart);

            var usersPart = GetUsersPart(parts);

            huntSession.AddUsers(usersPart);

            Console.WriteLine("Sessao criada com sucesso.");
            return huntSession;
        }



        public string[] GetSessionPart(string[] parts)
            => parts.Take(6).ToArray();

        public string[] GetUsersPart(string[] parts)
            => parts.Skip(6).ToArray();

        public bool Verify(string[] parts) =>
            parts.Count() == 6
            && parts[0].Contains("Session data:")
            && parts[1].Contains("Session:")
            && parts[2].Contains("Loot Type:")
            && parts[3].Contains("Loot:")
            && parts[4].Contains("Supplies:")
            && parts[5].Contains("Balance:");




    }
}

