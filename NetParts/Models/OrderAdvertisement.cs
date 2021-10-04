namespace NetParts.Models
{
    public class OrderAdvertisement
    {
        public int IdAdvert { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        public int IdOrder { get; set; }
        public virtual Order Order { get; set; }
    }
}
