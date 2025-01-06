namespace CorreosInstitucionales.Client
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("[Rol] Administrador", rol =>
                {
                    rol.RequireClaim("Rol", "1");
                    rol.RequireClaim("RecuperarContrasenia", "false");
                });
                options.AddPolicy("[Rol] Usuario Solicitante", rol =>
                {
                    rol.RequireClaim("Rol", "2");
                    rol.RequireClaim("RecuperarContrasenia", "false");
                });
                options.AddPolicy("[Rol] Ambos", rol =>
                {
                    rol.RequireAssertion(context =>
                    (
                        context.User.HasClaim(c => (c.Type == "Rol" && c.Value == "1")) && context.User.HasClaim(c => (c.Type == "RecuperarContrasenia" && c.Value == "false")) ||
                        context.User.HasClaim(c => (c.Type == "Rol" && c.Value == "2")) && context.User.HasClaim(c => (c.Type == "RecuperarContrasenia" && c.Value == "false"))
                    ));
                });
                options.AddPolicy("Anónimo", rol =>
                {
                    rol.RequireAssertion(context =>
                    (
                        true
                    ));
                });
            });

            return services;
        }
    }
}
