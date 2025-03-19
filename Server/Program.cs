using TodoApi;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;  

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});
builder.Services.AddCors(option => option.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDb"),
    new MySqlServerVersion(new Version(8, 0, 41))));

var app = builder.Build();

app.UseCors("AllowAll");
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.MapGet("",()=>"Hello World!");

app.MapGet("/getall", async (ToDoDbContext conts) => await conts.Items.ToListAsync());

app.MapPost("/add/{name}", async (ToDoDbContext conts ,string name) =>
{
    conts.Items.Add(new Item { Name = name, IsComplete = false });
    await conts.SaveChangesAsync();
    return name;
});

app.MapDelete("/delete/{id}", async (ToDoDbContext conts, int id) =>
{
    var item = await conts.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    conts.Items.Remove(item);
    await conts.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/update/{id}", async (ToDoDbContext conts, int id) =>
{
    // חפש את האובייקט לפי ה-ID
    var item = await conts.Items.FindAsync(id);

    // אם לא מצאנו את האובייקט, החזר NotFound
    if (item == null)
    {
        return Results.NotFound();
    }

    // עדכן את המאפיין (למשל, אם הוא שקר, נהפוך לאמת, ואם אמת, נהפוך לשקר)
    item.IsComplete = !item.IsComplete;

    // עדכון האובייקט בבסיס הנתונים
    conts.Entry(item).State = EntityState.Modified;
    await conts.SaveChangesAsync();

    // החזר תשובה מתאימה
    return Results.NoContent(); // אם הצלחנו לעדכן, החזר NoContent
});
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
// הרצת האפליקציה
app.Run();
