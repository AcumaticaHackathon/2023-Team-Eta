using ACPROpenAI.Models;

namespace ACPROpenAI.Services.Abstractions
{
    public interface IACPROpenAIRestService
    {
        /// <summary>
        /// Method for sending a prompt to an AI and receiving a response.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns>Response model. If the setup information is null, the function returns null</returns>
        ResponseModel RequestToAI(string prompt);
    }
}
