using System.Collections.Generic;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.DalModels;

namespace LearnEnglish.XN.Core.Repositories.Interfaces;

public interface IMessagesRepository
{
    Task InsertAsync(MessageDalModel message);

    Task<IEnumerable<MessageDalModel>> GetItemsAsync(int fetch, int limit);
}
