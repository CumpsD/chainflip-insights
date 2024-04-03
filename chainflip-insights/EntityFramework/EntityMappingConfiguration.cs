namespace ChainflipInsights.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.Extensions.Logging;
    
    public interface IExcludeEntityTypeConfiguration;

    public abstract class EntityTypeConfiguration<TContext, TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public abstract void Configure(
            EntityTypeBuilder<TEntity> builder);
    }

    public static class ModelBuilderExtensions
    {
        private static IEnumerable<Type> GetMappingTypes(
            this Assembly assembly,
            Type mappingInterface,
            Type excludeInterface)
            => assembly
                .GetTypes()
                .Where(
                    x =>
                        !x.GetTypeInfo().IsAbstract &&
                        x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface) &&
                        x.GetInterfaces().All(y => y != excludeInterface));

        public static void AddEntityConfigurationsFromAssembly<TContext>(
            this ModelBuilder modelBuilder,
            Assembly assembly)
        {
            if (EntityFrameworkLogger.LoggerFactory == null)
                throw new NullReferenceException("EntityFrameworkLogger has not been set");

            var logger = EntityFrameworkLogger.LoggerFactory.CreateLogger<EntityFrameworkLogger>();
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityTypeConfiguration<>), typeof(IExcludeEntityTypeConfiguration));

            foreach (var config in mappingTypes.Select(Activator.CreateInstance))
            {
                // public class EntityConfiguration : IEntityTypeConfiguration<Entity>
                // public class EntityConfiguration : EntityTypeConfiguration<Context, Entity>
                // var entityTypeConfiguration = config.GetType().GetInterface("IEntityTypeConfiguration`1");
                // var entity = entityTypeConfiguration.GetGenericArguments()[0]; // contains entity type

                if (config == null)
                    continue;

                var type = config.GetType().BaseType;
                
                if (type == null)
                    continue;

                var typeName = type.FullName ?? string.Empty;

                if (!typeName.StartsWith("ChainflipInsights.EntityFramework.EntityTypeConfiguration"))
                    continue;

                var context = type.GetGenericArguments()[0];
                var entity = type.GetGenericArguments()[1];

                if (context != typeof(TContext))
                    continue;

                var applyConfigurationMethod = (
                    from method in typeof(ModelBuilder).GetMethods()
                    where method.Name == "ApplyConfiguration"
                    where method.GetParameters().Select(p => p.ParameterType.GetGenericTypeDefinition()).SequenceEqual(new[] { typeof(IEntityTypeConfiguration<>) })
                    select method).Single();

                var applyConfiguration = applyConfigurationMethod.MakeGenericMethod(entity);

                logger.LogInformation(
                    "Applying configuration '{EntityFrameworkConfiguration}' for '{Context}'",
                    config.GetType().FullName,
                    typeof(TContext).Name);

                applyConfiguration.Invoke(modelBuilder, [config]);
            }
        }
    }
}