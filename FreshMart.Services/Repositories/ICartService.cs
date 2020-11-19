namespace FreshMart.Services
{
    public interface ICartService
    {
        int GetCartCount();
        float GetCartTotalPrice();
    }

}
