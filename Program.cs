using StyleAssistantCA1_SOA_MeghanKeightley.Components;

namespace StyleAssistantCA1_SOA_MeghanKeightley
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IWeather, Weather>();
            // Configure the ASOS API HTTP client
            builder.Services.AddHttpClient<Asos>(client =>
            {
                client.BaseAddress = new Uri("https://asos2.p.rapidapi.com/");
                // Corrected header key without colon
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "asos2.p.rapidapi.com");
                // Use configuration to set the API key for security
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", builder.Configuration["RapidAPIAsos:asosKey"]);
            });

            builder.Services.AddScoped<Asos>();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapRazorPages();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
