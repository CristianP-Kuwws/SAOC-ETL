using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using SAOC.Application.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOC.Persistence.Helpers
{
    public class StoredProcedureExecutor : IStoredProcedureExecutor
    {
        private readonly string _connectionString;
        public StoredProcedureExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<string> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                using var command = new MySqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Parameters

                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }

                var resultParam = new MySqlParameter("p_result", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Output,
                };
                command.Parameters.Add(resultParam);

                // Open Connection

                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                // Return Result

                var result = resultParam?.Value?.ToString() ?? "No message returned by stored procedure.";

                if (!string.IsNullOrEmpty(result) && result.StartsWith("Success", StringComparison.OrdinalIgnoreCase))
                {
                    return result; // Success
                }

                if (affectedRows > 0)
                {
                    return $"Success (rows affected): {affectedRows}";
                }

                return $"Error: {result}";

            }
            catch (Exception ex)
            {
                return $"Exception while executing {procedureName}: {ex.Message}";
            }
        }
    }
}
