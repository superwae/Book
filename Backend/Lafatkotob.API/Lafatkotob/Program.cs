using Lafatkotob;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lafatkotob.Services.EventService;
using Lafatkotob.Services.BookService;
using Lafatkotob.Services.BadgeService;
using Lafatkotob.Services.HistoryService;
using Lafatkotob.Services.BookGenreService;
using Lafatkotob.Services.BookPostCommentService;
using Lafatkotob.Services.BookPostLikeServices;
using Lafatkotob.Services.BooksInWishlistsService;
using Lafatkotob.Services.GenreService;
using Lafatkotob.Services.ConversationService;
using Lafatkotob.Services.ConversationsUserService;
using Lafatkotob.Services.TokenService;
using Lafatkotob.Services.UserLikeService;
using Lafatkotob.Services.UserEventService;
using Lafatkotob.Services.WishedBookService;
using Lafatkotob.Services.WishListService;
using Lafatkotob.Services.AppUserService;
using Lafatkotob.Services.MessageService;
using Lafatkotob.Services.NotificationService;
using Lafatkotob.Services.EmailService;
using Lafatkotob.Services.UserPreferenceService;
using Lafatkotob.Services.NotificationUserService;
using Lafatkotob.Services.UserBadgeService;
using Lafatkotob.Services.UserReviewService;
using Lafatkotob.Entities;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IBookGenreServices,BookGenreServices>();
builder.Services.AddScoped<IBookPostCommentServices,BookPostCommentServices>();
builder.Services.AddScoped<IBookPostLikeService,BookPostLikeService>();
builder.Services.AddScoped<IBooksInWishlistsService,BooksInWishlistsService>();
builder.Services.AddScoped<IGenreService,GenreService>();
builder.Services.AddScoped<IConversationService,ConversationService>();
builder.Services.AddScoped<IConversationsUserService,ConversationsUserService>();
builder.Services.AddScoped<ITokenSerive,TokenService>();
builder.Services.AddScoped<IUserLikeService,UserLikeService>();
builder.Services.AddScoped<IUserEventService,UserEventService>();
builder.Services.AddScoped<IWishedBookService,WishedBookService>();
builder.Services.AddScoped<IWishListService,WishListService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMessageService,MessageService>();
builder.Services.AddScoped<INotificationService,NotificationService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IUserPreferenceService,UserPreferenceService>();
builder.Services.AddScoped<INotificationUserService,NotificationUserService>();
builder.Services.AddScoped<IUserBadgeService,UserBadgeService>();
builder.Services.AddScoped<IUserReviewService,UserReviewService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null);
        })
    );

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//jwt auth
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
