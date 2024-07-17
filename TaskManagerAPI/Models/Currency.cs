using System;

namespace TaskManagerAPI.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Coin { get; set; }
        public string Currency_type { get; set; }
        public decimal Price { get; set; }
        public DateTime Checked { get; set; }
    }
}
