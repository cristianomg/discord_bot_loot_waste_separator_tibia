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

        public decimal TotalProfit => Profit * TotalUsers;

        public int TotalUsers { get; private set; }
        public string SessionData { get; private set; }
        public decimal Profit { get; private set; }    
        public List<SessionPayment> Payments { get; } = new List<SessionPayment>();
    }
}