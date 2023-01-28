using ACPROpenAI.DAC.OpenAIConnectionPreferences;
using ACPROpenAI.Descriptor;
using ACPROpenAI.Helpers;
using ACPROpenAI.Helpers.Exceptions;
using ACPROpenAI.Models;
using ACPROpenAI.Services.Abstractions;
using PX.Data;
using RestSharp;

namespace ACPROpenAI.Services
{
    public class ACPROpenAIRestService : IACPROpenAIRestService
    {
        private const string Application = "application/json";
        private const string Authorization = "Authorization";
        private const string Bearer = "Bearer";

        private readonly PXGraph _graph;

        public ACPROpenAIRestService(PXGraph graph)
        {
            _graph = graph;
        }

        public ResponseModel RequestToAI(string prompt)
        {
            var setup = GetACPRSetup();

            if (setup != null)
            {
                var requestModel = CreateRequestModel(prompt, setup);
                string content = GetPostContent(setup.EndPoint, setup.APIKey, requestModel);

                var responseModel = ACPRJsonConverter<ResponseModel>.FromJson(content);
                return responseModel;
            }
            
            return null;
        }
        
        protected virtual string GetPostContent(string endPoint, string apiKey, RequestModel requestModel)
        {
            var requestJson = ACPRJsonConverter<RequestModel>.ToJson(requestModel);
            var client = new RestClient(endPoint);
            var request = new RestRequest();
            request.AddHeader(Authorization, Bearer + $" {apiKey}");
            request.AddJsonBody(requestJson, Application);

            try
            {
                var responce = client.Execute(request, Method.POST);
                if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ACPRStatusCodeException(responce.StatusCode, responce.Content);
                }
                return responce.Content;
            }
            catch (ACPRStatusCodeException ex)
            {
                PXTrace.WriteError($"Error: {ex.Message}");
                PXTrace.WriteError($"Status Code: {ex.StatusCode}");
                PXTrace.WriteError($"Content: {ex.Content}");
                throw new PXException(ACPRMessages.RemoteServerError);
            }
        }

        private RequestModel CreateRequestModel(string prompt, ACPRSetup setup) =>
            new RequestModel
            {
                Model = setup.Model,
                Prompt = prompt,
                MaxTokens = setup.MaxTokens.Value,
                Temperature = setup.Temperature.Value,
                TopP = setup.TopP.Value,
                PresencePenalty = setup.PresencePenalty.Value,
                FrequencyPenalty = setup.PresencePenalty.Value,
            };

        #region BaseSelect
        /// <summary>
        /// Retrieving a configuration setup object of type ACPRSetup from a data source.
        /// </summary>
        /// <returns></returns>
        public ACPRSetup GetACPRSetup() =>
              PXSelectBase<ACPRSetup, PXSelect<ACPRSetup>.Config>.Select(_graph);
        #endregion
    }
}
