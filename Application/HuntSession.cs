using System;
namespace Application
{
    public class HuntSession
    {
        public string SessionData { get; private set; }
        public string Time { get; private set; }
        public string LootType { get; private set; }
        public decimal Loot { get; private set; }
        public decimal Supplies { get; private set; }
        public decimal Balance { get; private set; }


        public HuntSession(string[] parts)
        {
            SessionData = parts[0].Trim();
            Time = parts[1].Replace("Session:", "").Trim();
            LootType = parts[2].Replace("Loot Type:", "").Trim();
            Loot = Convert.ToDecimal(parts[3].Replace("Loot:", "").Replace(',', '.').Trim());
            Supplies = Convert.ToDecimal(parts[4].Replace("Supplies:", "").Replace(',', '.').Trim());
            Balance = Convert.ToDecimal(parts[5].Replace("Balance:", "").Replace(',', '.').Trim());
        }

        public List<HuntUser> Users { get; } = new List<HuntUser>();


        public void AddUsers(string[] usersPart) 
        {
            for(var x = 0; x < usersPart.Count(); x += 6)
            {
                var parts = new string[] 
                {
                    usersPart[x], 
                    usersPart[x+1], 
                    usersPart[x+2],
                    usersPart[x+3]
                };

                Users.Add(new HuntUser(parts));
            }
        }


        public HuntSessionResult CalculateSession() 
        {
            var sessionResult = new HuntSessionResult(SessionData, Balance, Users);
        
            return sessionResult.Profit > 0 ? 
                CalculateWithProfit(sessionResult) :
                CalculateWithoutProfit(sessionResult);
        }

        private HuntSessionResult CalculateWithProfit(HuntSessionResult result)
        {
            var positives = Users.Where(x=>x.HasFound());

            Func<IEnumerable<HuntUser>> GetUserWithoutProfit = () =>
                Users.Where(x=>x.IndividualProfit < result.Profit).OrderBy(x=>x.Balance);

            foreach(var pos in positives)
            {
                pos.GetProfit(result.Profit);

                var negatives = GetUserWithoutProfit();

                while(negatives.Any() && pos.HasFound())
                {
                    var neg = negatives.First();

                    var payment = new SessionPayment(pos, neg);

                    payment.Transfer(result.Profit);

                    result.Payments.Add(payment);

                    neg.GetProfit(result.Profit);

                    negatives = GetUserWithoutProfit();
                }
            }
            return result;
        }
        private HuntSessionResult CalculateWithoutProfit(HuntSessionResult result)
        {
            var positives = Users.Where(x=>x.HasFound());

            Func<Queue<HuntUser>> GetUserWithNegativeBalance = () =>
                new Queue<HuntUser>(Users.Where(x=>x.Balance < 0).OrderBy(x=>x.Balance));

            var negatives = GetUserWithNegativeBalance();

            foreach(var pos in positives)
            {
                if (!negatives.Any()) break;
                while(negatives.Any() && pos.HasFound())
                {
                    var neg = negatives.First();
                    var payment = new SessionPayment(pos, neg);
                    payment.Transfer(0);

                    result.Payments.Add(payment);

                    if (!neg.HasWaste())
                        negatives.Dequeue();
                }
            }
            return result;
        }

    }
}

