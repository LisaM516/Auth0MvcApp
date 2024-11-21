using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect("Auth0", options =>
{
    options.Authority = "https://dev-ra7h0p3fxfgm02b0.us.auth0.com";
    options.ClientId = "9X3avLk6SS5klGBJ5PDu6johtyOR8Tet";
    options.ClientSecret = "32bI8J1FYzOH2TD83BXy9MEjNwU5tkPq5LrAJLr7oku-Bn6z37Jm99nvQwa7neg6";
    options.ResponseType = "code";

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.CallbackPath = new PathString("/signin-oidc");
    options.SaveTokens = true;
    options.TokenValidationParameters.NameClaimType = "name";

    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = context =>
        {
            context.ProtocolMessage.SetParameter("audience", "");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var idToken = context.SecurityToken as JwtSecurityToken;
            Console.WriteLine($"ID Token: {idToken.RawData}");

            // Optionally decode and log claims
            foreach (var claim in idToken.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }
            return Task.CompletedTask;
        }
        
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


