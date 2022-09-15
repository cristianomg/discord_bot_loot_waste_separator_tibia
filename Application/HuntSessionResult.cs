using System.Text;

namespace Application
{
    public class HuntSessionResult 
    {
        public HuntSessionResult(string sessionData, decimal balance, List<HuntUser> users)
        {
            SessionData = sessionData;
            Profit = balance / users.Count;
            TotalUsers = users.Count;
        }
        public int TotalUsers { get; private set; }
        public string SessionData { get; private set; }
        public decimal Profit { get; private set; }    
        public List<SessionPayment> Payments { get; } = new List<SessionPayment>();


        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(SessionData + "\n");
            result.Append("Total Profit: " + Profit * TotalUsers + "\n");
            result.Append("Individual Profit: " + Profit + "\n");
            result.Append("Payments:  \n" );

            Payments.ForEach(x=> result.Append($"From : {x.From.Name} - To: {x.To.Name} -- Value: {Math.Floor(x.Value)} \n"));

            return result.ToString();
        }

    }
}