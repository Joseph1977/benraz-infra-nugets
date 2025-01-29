using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Benraz.Infrastructure.Common.CommonUtilities
{
    /// <summary>
    /// Common utilities.
    /// </summary>
    public static class CommonUtilities
    {
        /// <summary>
        /// Is need to connect to db.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="skipDbConnectIfNoConnectionString">Skip db connect if no connection string.</param>
        /// <returns>True/False</returns>
        public static bool IsNeedToConnectToDB(string connectionString, bool skipDbConnectIfNoConnectionString)
        {
            return !(skipDbConnectIfNoConnectionString && string.IsNullOrWhiteSpace(connectionString));
        }

        /// <summary>
        /// Get connect string.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="isForSqlServer">Is for sql server.</param>
        /// <returns>String</returns>
        public static string GetConnectString(IConfiguration configuration, bool isForSqlServer = false)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings");

            if (configuration.GetValue<bool>("InjectDBCredentialFromEnvironment"))
            {
                if (isForSqlServer)
                {
                    connectionString +=
                        $";User Id={configuration.GetValue<string>("AspNetCoreDbUserName")};Password='{configuration.GetValue<string>("AspNetCoreDbPassword")}'";
                }
                else
                {
                    connectionString +=
                        $";Username={configuration.GetValue<string>("AspNetCoreDbUserName")};Password='{configuration.GetValue<string>("AspNetCoreDbPassword")}'";
                }
            }
            return connectionString;
        }

        /// <summary>
        /// Load environment variable from json (Use only for local development and test project)
        /// </summary>
        /// <param name="path">Path of json file.</param>
        /// <param name="environmentVariablesJsonPath">Environment variables json path like profiles['IIS Express'].environmentVariables.</param>
        public static void LoadEnvironmentVariablesFromJson(string path, string environmentVariablesJsonPath = "")
        {
            if (!File.Exists(path)) return;

            string jsonContent;
            try
            {
                jsonContent = File.ReadAllText(path);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return;
            }

            JObject jsonObject;
            try
            {
                jsonObject = JObject.Parse(jsonContent);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return;
            }

            // Use a dynamic path to extract environment variables from the JSON object
            var environmentVariables = string.IsNullOrWhiteSpace(environmentVariablesJsonPath) ? jsonObject.Root : jsonObject.SelectToken(environmentVariablesJsonPath);

            if (environmentVariables == null)
            {
                Console.WriteLine("No environment variables found at the specified path.");
                return;
            }

            foreach (JProperty variable in environmentVariables)
            {
                try
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting environment variable {variable.Name}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Datetime milli seconds value rounding. (Use in test project with postgres)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Entity</returns>
        public static TEntity RoundToMilliseconds<TEntity>(TEntity entity)
        {
            // Get the type of the entity
            var entityType = typeof(TEntity);

            // Check if the 'CreateTimeUtc' property exists and is of type DateTime
            var createTimeUtcProperty = entityType.GetProperty("CreateTimeUtc");
            if (createTimeUtcProperty != null && createTimeUtcProperty.PropertyType == typeof(DateTime))
            {
                var createTimeUtcValue = (DateTime)createTimeUtcProperty.GetValue(entity);
                // Round to milliseconds
                createTimeUtcProperty.SetValue(entity, new DateTime((createTimeUtcValue.Ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond));
            }

            // Check if the 'UpdateTimeUtc' property exists and is of type DateTime
            var updateTimeUtcProperty = entityType.GetProperty("UpdateTimeUtc");
            if (updateTimeUtcProperty != null && updateTimeUtcProperty.PropertyType == typeof(DateTime))
            {
                var updateTimeUtcValue = (DateTime)updateTimeUtcProperty.GetValue(entity);
                // Round to milliseconds
                updateTimeUtcProperty.SetValue(entity, new DateTime((updateTimeUtcValue.Ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond));
            }

            return entity;
        }
    }
}
