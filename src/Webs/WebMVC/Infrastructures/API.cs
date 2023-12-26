namespace OrionEShopOnContainer.Webs.WebMVC.Infrastructures;

public static class API
{
    public static class Purchase
    {
        public static string AddItemToBasket(string baseUri) => $"{baseUri}/api/v1/basket/items";
        public static string UpdateBasketItem(string baseUri) => $"{baseUri}/api/v1/basket/items";

        public static string GetOrderDraft(string baseUri, string basketId) => $"{baseUri}/api/v1/order/draft/{basketId}";
    }
}
