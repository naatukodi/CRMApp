using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Repository
{
    public interface IRetailSurveyRepository
    {
        Task CreateRetailSurveyAsync(RetailSurvey survey);
        // Additional methods can be added as needed.
    }
}
    