namespace Application
{
    public class HuntSessionResult 
    {
        public HuntSessionResult(decimal balance, List<HuntUser> users)
        {
            Profit = balance / users.Count;
        }
        public decimal Profit { get; set; }    
        public List<SessionPayment> Payments { get; set; } = new List<SessionPayment>();


        public override string ToString()
        {
            var result = @$"Profit: {Profit}
                        Payments:
                           ";

            Payments.ForEach(x=> result += $"From : {x.From.Name} - To: {x.To.Name} -- Value: {x.Value} \n");

            return result;
        }

    }
}