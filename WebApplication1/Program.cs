var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add logging middleware before endpoints
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
    logger.LogInformation($"Response: {context.Response.StatusCode}");
});


var bookings = new List<Booking>();

app.MapGet("/bookings", () => bookings);

app.MapGet("/bookings/{id}", (Guid id) =>
    bookings.FirstOrDefault(b => b.Id == id) is Booking booking
        ? Results.Ok(booking)
        : Results.NotFound());

app.MapPost("/bookings", (Booking booking) =>
{
    booking = booking with { Id = Guid.NewGuid() };
    bookings.Add(booking);
    return Results.Created($"/bookings/{booking.Id}", booking);
});

app.MapPut("/bookings/{id}", (Guid id, Booking updatedBooking) =>
{
    var index = bookings.FindIndex(b => b.Id == id);
    if (index == -1) return Results.NotFound();

    bookings[index] = updatedBooking with { Id = id };
    return Results.NoContent();
});

app.MapDelete("/bookings/{id}", (Guid id) =>
{
    var booking = bookings.FirstOrDefault(b => b.Id == id);
    if (booking is null) return Results.NotFound();

    bookings.Remove(booking);
    return Results.NoContent();
});

app.Run();

record Booking(Guid Id, string CustomerName, DateTime BookingDate, string Details)
{
    public bool IsConfirmed { get; set; } = false;
}
