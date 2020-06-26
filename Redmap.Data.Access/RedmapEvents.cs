using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Redmap.Data.Access
{
    /// <summary>
    ///Library to save events message
    /// </summary>
    public static class RedmapEvents
    {
        /// <summary>
        /// Method for save event.
        /// </summary>
        /// <param name="webapiurl"></param>
        /// <param name="Category"></param>
        /// <param name="ErrorCode"></param>
        /// <param name="Message"></param>
        /// <param name="ServerDetail"></param>
        /// <param name="Source"></param>
        /// <param name="Summary"></param>
        /// <param name="Tag1"></param>
        /// <param name="Tag2"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Response saveEvents(string webapiurl,string Category, string ErrorCode,string Message,string ServerDetail,string Source,string Summary,string Tag1,string Tag2, IFormFile file)
        {
            Response response = new Response();
            try
            {
                var multipartContent = new MultipartFormDataContent();

                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        byte[] bytearray = ms.ToArray();
                        var fileContent = new ByteArrayContent(bytearray);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                        //File
                        multipartContent.Add(fileContent, "AttachedFile", Path.GetFileName(file.FileName));
                    }
                }

                // EventModel other fields
                multipartContent.Add(new StringContent(Category), "Category");
                multipartContent.Add(new StringContent(ErrorCode), "ErrorCode");
                multipartContent.Add(new StringContent(Message), "Message");
                multipartContent.Add(new StringContent(ServerDetail), "ServerDatail");
                multipartContent.Add(new StringContent(Source), "Source");
                multipartContent.Add(new StringContent(Summary), "Summary");
                multipartContent.Add(new StringContent(Tag1), "Tag1");
                multipartContent.Add(new StringContent(Tag2), "Tag2");

                var httpClient = new HttpClient();
                var result = httpClient.PostAsync(webapiurl, multipartContent).Result;

                if(result.IsSuccessStatusCode)
                {
                    response.StatusCode = result.StatusCode.ToString();
                    response.Message = "Success";
                }
                else
                {
                    response.StatusCode = result.StatusCode.ToString();
                    response.Message = "Failed";
                }               

            }
            catch (Exception ex)
            {                
                response.Message = ex.ToString();
            }

            return response;
        }

    }
}
