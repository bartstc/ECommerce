namespace ProductCatalog.Infrastructure.Documents;

public static class DocumentsConfiguration
{
    public static void ConfigureDocuments(this StoreOptions options)
    {
        options.Schema.For<ProductDocument>().DocumentAlias("productdocuments");
        options.Schema.For<ProductDocument>().Identity(x => x.ProductId);
    }
}