using PresentationCodeTower.Models;
using PresentationCodeTower.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var database = new FakeDb();
database.populate();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Map products endpoints
app.MapGet("/products", () => {
    //return a json with all products
    var products = database.GetProducts();
    return products;
});
app.MapGet("/products/{id}", (int id) => {
    var product = database.GetProducts().FirstOrDefault(p => p.Id == id);
    if (product is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});
app.MapPost("/products", (Product product) => {
    if (product.Name is null)
    {
        return Results.BadRequest("Name is required");
    }

    if (product.Price <= 0)
    {
        return Results.BadRequest("Price must be greater than 0");
    }


    var added = database.AddProduct(product);
    return Results.Created($"/products/{added.Id}", added);
});
app.MapPut("/products/{id}", (int id, Product product, HttpContext ctx) =>
{
    if (product.Name is null)
    {
        return Results.BadRequest("Name is required");
    }

    if (product.Price <= 0)
    {
        return Results.BadRequest("Price must be greater than 0");
    }

    var data = database.UpdateProduct(id, product);
    if (data is null)
    {
        ctx.Response.StatusCode = 404;
        return Results.NotFound("Product Not found");
    }

    return Results.Ok(data);
});
app.MapDelete("/products/{id}", (int id, HttpContext ctx) =>
{
    var deletedProduct = database.DeleteProduct(id);
    if (deletedProduct is null)
    {
        ctx.Response.StatusCode = 404;
        return Results.NotFound("Product Not Found");
    }

    return Results.Ok(deletedProduct);
});

// Map orders endpoints
// TODO: Should be tested
app.MapGet("/orders", () =>
{
    //return a json with all orders
    var orders = database.GetOrders();
    return orders;
});
app.MapGet("/orders/{id}", (int id) =>
{
    var order = database.GetOrders().FirstOrDefault(o => o.Id == id);
    if (order is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
});
app.MapPost("/orders", (Order order) =>
{
    if (order.User is null)
    {
        return Results.BadRequest("User is required");
    }

    if (order.Products is null)
    {
        return Results.BadRequest("Products is required");
    }

    if (order.Products.Count == 0)
    {
        return Results.BadRequest("Products is required");
    }


    if (!database.DoesUserExist(order.User))
    {
        return Results.BadRequest("User does not exist");
    }
    if (!database.AllProductsExist(order.Products))
    {
        return Results.BadRequest("One or more products does not exist");
    }


    var added = database.AddOrder(order);
    return Results.Created($"/orders/{added.Id}", added);
});
app.MapPut("/orders/{id}", (int id, Order order, HttpContext ctx) =>
{
    if (order.User is null)
    {
        return Results.BadRequest("User is required");
    }

    if (order.Products is null)
    {
        return Results.BadRequest("Products is required");
    }

    if (order.Products.Count == 0)
    {
        return Results.BadRequest("Products is required");
    }

    
    if (!database.DoesUserExist(order.User))
    {
        return Results.BadRequest("User does not exist");
    }

    if (!database.AllProductsExist(order.Products))
    {
        return Results.BadRequest("One or more products does not exist");
    }


    if (!database.GetOrders().Any(o => o.Id == id))
    {
        ctx.Response.StatusCode = 404;
        return Results.NotFound("Order Not found");
    }


    var data = database.UpdateOrder(id, order);
    if (data is null)
    {
        ctx.Response.StatusCode = 404;
        return Results.NotFound("Order Not found");
    }

    return Results.Ok(data);
});
app.MapDelete("/orders/{id}", (int id) =>
{
    if (!database.GetOrders().Any(o => o.Id == id))
    { 
        return Results.NotFound("Order Not Found");
    }

    database.DeleteOrder(id);

    return Results.Ok("Order succesfully deleted");
});


// Map users endpoints
app.MapGet("/users", () =>
{
    //return a json with all users
    var users = database.GetUsers();
    return users;
});

app.Run();