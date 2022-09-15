using System;
namespace Application
{
    public class HuntUser
    {
        public string Name { get; set; }
        public decimal Loot { get; private set; }
        public decimal Supplies { get; private set; }
        public decimal Balance { get; private set; }
        public decimal IndividualProfit {get; private set;}

        public HuntUser(string[] userParts)
        {
            Name = userParts[0];
            Loot = Convert.ToDecimal(userParts[1].Replace("Loot:", "").Replace(',', '.').Trim());
            Supplies = Convert.ToDecimal(userParts[2].Replace("Supplies:", "").Replace(',', '.').Trim());
            Balance = Convert.ToDecimal(userParts[3].Replace("Balance:", "").Replace(',', '.').Trim());
        }

        public void ReceiveFund(decimal fund) 
            => Balance += fund;

        public decimal Withdraw(decimal ammount){
            if (Balance <= 0) return 0.0M;

            else if (Balance >= ammount){
                Balance -= ammount;
                return ammount;
            }
            else {
                var auxBalance = Balance;
                Balance -= auxBalance;
                return auxBalance;
            }
        }

        public bool HasFound() 
            => Balance > 0;
        public bool HasWaste()
            => Balance < 0;
        public void GetProfit(decimal profit) 
        {
            var valueToComplete = profit - IndividualProfit;

            if (Balance >= valueToComplete)
            {
                IndividualProfit += valueToComplete;
                Balance -= valueToComplete;
            }
            else if (Balance > 0)
            {
                IndividualProfit += Balance;
                Balance = 0;
            }
        }
    }
}

