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
            if (skipDbConnectIfNoConnectionString && string.IsNullOrWhiteSpace(connectionString)) return false;
            return true;
        }

        /// <summary>
        /// Get connect string.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <returns>String</returns>
        public static string GetConnectString(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings");

            if (configuration.GetValue<bool>("InjectDBCredentialFromEnvironment"))
            {
                connectionString +=
                     $";Username={configuration.GetValue<string>("AspNetCoreDbUserName")};Password='{configuration.GetValue<string>("AspNetCoreDbPassword")}'";
            }
            return connectionString;
        }

        /// <summary>
        /// Load environment variable by json
        /// </summary>
        /// <param name="path">Path of json file.</param>
        /// <param name="isLoadFromRoot">Environment variable load from root of json.</param>
        /// <param name="profileName">json profile name if want to load from launch setting.</param>
        public static void LoadEnvironmentVariableByJson(string path, bool isLoadFromRoot = false, string profileName = "IIS Express")
        {
            if (File.Exists(path))
            {
                var jsonContent = File.ReadAllText(path);
                var jsonReader = new JsonTextReader(new StringReader(jsonContent));
                var jsonObject = JObject.Load(jsonReader);

                JToken environmentVariables;
                if (isLoadFromRoot)
                    environmentVariables = jsonObject.Root;
                else
                    environmentVariables = jsonObject["profiles"]?[profileName]?["environmentVariables"];

                if (environmentVariables != null)
                {
                    foreach (JProperty variable in environmentVariables)
                    {
                        Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                    }
                }
            }
        }
    }
}
