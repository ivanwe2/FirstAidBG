
using FirstAidBG_v._2.Business.Services;
using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Data.Seeder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



builder.Services.AddScoped<UserService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddHttpContextAccessor();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Denied";
        options.LogoutPath = "/Logout";

        options.Events = new CookieAuthenticationEvents()
        {

            OnSigningIn = async context =>
            {
                var scheme = context.Properties.Items.Where(k => k.Key == ".AuthScheme").FirstOrDefault();

                var claim = new Claim(scheme.Key, scheme.Value);
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                var userService = context.HttpContext.RequestServices.GetRequiredService(typeof(UserService)) as UserService;
                var nameIdentifier = claimsIdentity.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
                if(userService != null && nameIdentifier != null)
                {
                    var appUser= userService.GetUserByExternalProvider(scheme.Value,nameIdentifier);
                    if(appUser is null)
                    {
                        appUser = userService.AddNewUser(scheme.Value, claimsIdentity.Claims.ToList());

                    }
                    foreach (var item in appUser.RoleList)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item));
                    }
                }
                claimsIdentity.AddClaim(claim);
            }
        };

    }).AddOpenIdConnect("google", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.ClientId = "922404681192-mkng507q35fakq9g40murh9a4j8nj4ui.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.CallbackPath = "/auth";



        options.SignedOutCallbackPath = "/google-signout";
        

        options.SaveTokens = true;
        options.Events = new OpenIdConnectEvents()
        {
            OnTokenValidated = async context =>
            {
                if (context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == "110233425002949748629")
                {
                    var claim = new Claim(ClaimTypes.Role, "Admin");
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity.AddClaim(claim);
                }
                //var claims = context.Principal.Claims;
            }
        };

    })
    .AddOpenIdConnect("okta", options =>
    {
        options.Authority = "https://dev-12128604.okta.com/oauth2/default";
        options.ClientId = "0oa47aaxv5423ojzm5d7";
        options.ClientSecret = "3yieMhdYyNJl_dD5LCG0jbGcBOGLPSjl56PsNFB-";
        options.CallbackPath = "/okta-auth";
        options.SignedOutCallbackPath = "/okta-signout";
        options.ResponseType = "code";


        options.SaveTokens = true;

        options.Scope.Add("openid");
        options.Scope.Add("profile");
    });

//.AddGoogle(options =>
//{
//    options.ClientId = "922404681192-mkng507q35fakq9g40murh9a4j8nj4ui.apps.googleusercontent.com";
//    options.ClientSecret = "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";
//    options.CallbackPath = "/auth";
//    options.AuthorizationEndpoint += "?prompt=consent";
//});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
