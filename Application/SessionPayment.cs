namespace Application 
{
    public class SessionPayment 
    {
        public HuntUser From {get; set;}
        public HuntUser To {get; set;}
        public decimal Value {get; set;}

        public SessionPayment(HuntUser from, HuntUser to)
        {
            From = from;
            To = to;
        }

        public void Transfer(decimal ammount)
        {
            Value = From.Withdraw((Math.Abs(To.Balance) + ammount) - To.IndividualProfit);

            To.ReceiveFund(Value);
        }

    }
}