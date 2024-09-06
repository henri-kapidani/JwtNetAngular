# Implematazione dell'autenticazione con jwt in un progetto .NET Core con frontend in Angular

1. Il tipo di progetto migliore da cui partire per fare un backend di tipo API (senza viste Razon) è `ASP.NET Core Web API` con il check su:

-   `Configure for HTTPS`
-   `Enable OpenAPI support`
-   `Use controllers`

## Gestione del Jwt

1. Installare i NuGet packages necessari (latest stable per la versione 8 di .NET):

    - `Microsoft.AspNetCore.Authentication.JwtBearer`
    - `Microsoft.IdentityModel.JsonWebTokens`

1. In `appsettings.json` aggiungere:

    ```json
        "Jwt": {
            "Issuer": "ilnostrosito.com",
            "Audience": "ilnostrosito.com",
            "Key": "chiave segreta sufficientemente lunga"
        }
    ```

1. In `Program.cs` configurare la verifica del jwt:

    ```csharp
    // Configurazione JWT
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

    builder.Services.AddAuthorization();
    ```

1. Creare un controller `AuthController.cs` che deve contenere i metodi che gestiscono il `Login` (dove viene creato il jwt token se le credenziali di login sono corrette) e il `Register`. Il logout con la versione base del jwt si gestisce interamente da frontend e non necessita di una rotta nel backend.

## Gestire il CORS

1. In `appsettings.json` aggiungere:

    ```json
    "FrontendUrl": "http://localhost:4200",
    ```

1. In `Program.cs` configurare la gestione del CORS:

    ```csharp
    // Configurazione CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins(builder.Configuration["FrontendUrl"]!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.AddControllers();

    // ..... altro codice
    // dopo var app = builder.Build();

    app.UseCors(); // registriamo il middlaware che gestisce il CORS
    ```

## Frontend

Nel frontend basta fare le richieste all'indirizzo della nostra API (qualcosa del tipo https://localhost:PORTA) tenendo in esecuzione il backend .NET durante i test.
