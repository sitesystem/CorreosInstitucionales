namespace CorreosInstitucionales.Client
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("[Rol] Administrador", rol => rol.RequireClaim("Rol", "1"));
                options.AddPolicy("[Rol] Usuario Solicitante", rol => rol.RequireClaim("Rol", "2"));
            });

            return services;
        }
    }
}
