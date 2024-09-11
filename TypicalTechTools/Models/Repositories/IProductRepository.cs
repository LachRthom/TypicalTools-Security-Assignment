namespace TypicalTechTools.Models.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void UpdateProductPrice(Product product); // We only need to update price
        void DeleteProduct(int id);
    }
}
