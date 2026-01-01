namespace IgniteShared.Dtos
{
    public class ProductDto : DataBase
    {
        private string _productId;

        public string ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }

        private string _productName;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }
    }
}