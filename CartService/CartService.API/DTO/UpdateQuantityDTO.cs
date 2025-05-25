namespace CartService.API.DTO
{
    public class UpdateQuantityDTO
    {
        public long CartItemId { get; set; }
        public int NewQuantity { get; set; }
    }
}
