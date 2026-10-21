namespace EshtryWepApi.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
