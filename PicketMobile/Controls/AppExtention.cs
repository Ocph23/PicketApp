using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PicketMobile.Controls
{
    public static class AppExtention
    {
        public static async Task<string> Error(this HttpClient httpClient, HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, Helper.JsonOption);
                return errorResponse?.Message ?? "An error occurred.";
            }
            catch (JsonException)
            {
                return "An error occurred.";
            }


        }
    }
}
