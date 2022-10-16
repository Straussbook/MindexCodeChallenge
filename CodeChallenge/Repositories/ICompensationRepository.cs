using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(String id);
        void Add(Compensation compensation);
        Task SaveAsync();
    }
}