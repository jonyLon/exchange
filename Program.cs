using System.Xml.Linq;

namespace exchange
{

    class Trader
    {
        public int Amount { get; private set; }
        public Trader(int amount)
        {
            Amount = amount;
        }
        public void GetNotification(object sender, MyArgs a)
        {
            Console.WriteLine($"Trader notified about {a.Description} --> Date {a.Date.ToShortTimeString()}");
        }

    }

    class MyArgs : EventArgs
    {
        public string? Description { get; set; }
        public DateTime Date => DateTime.Now;
    }


    class Exchange
    {
        public int Rate { get; private set; }
        public Exchange(int rate = 100)
        {
            Rate = rate;
        }
        public event EventHandler<MyArgs>? RateDecrease;
        public event EventHandler<MyArgs>? RateIncrease;
        public void Increase(int i) { 
            Rate += i;
            MyArgs args = new MyArgs()
            {
                Description = $"Rate increased by {i}",
            };
            RateIncrease?.Invoke(this, args);

        }
        public void Decrease(int i)
        {
            Rate -= i;
            MyArgs args = new MyArgs()
            {
                Description = $"Rate decreased by {i}",
            };
            RateDecrease?.Invoke(this, args);
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Exchange exchange = new Exchange();
            Trader trader = new Trader(1000);

            exchange.RateIncrease += trader.GetNotification;
            exchange.RateDecrease += trader.GetNotification;


            exchange.Increase(1000);
            exchange.Decrease(800);
        }
    }
}