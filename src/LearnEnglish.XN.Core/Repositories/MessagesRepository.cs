using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnEnglish.XN.Core.Definitions.DalModels;
using LearnEnglish.XN.Core.Repositories.Interfaces;
using SQLite;

namespace LearnEnglish.XN.Core.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private readonly SQLiteAsyncConnection _db;

    private bool _isCreated;

    public MessagesRepository(SQLiteAsyncConnection db) => _db = db;


    public Task InsertAsync(MessageDalModel message) => WrapWithDbCreationCheck(() => _db.InsertAsync(message));

    public async Task<IEnumerable<MessageDalModel>> GetItemsAsync(int fetch, int limit) => await WrapWithDbCreationCheck(() => _db.Table<MessageDalModel>().OrderByDescending(m => m.Id).Skip(fetch).Take(limit).ToArrayAsync());

    private Task<TResult> WrapWithDbCreationCheck<TResult>(Func<Task<TResult>> action)
    {
        return _isCreated ? action() : CreateTablesAndDoAction();

        async Task<TResult> CreateTablesAndDoAction()
        {
            await _db.CreateTableAsync<MessageDalModel>();

            _isCreated = true;
            return await action();
        }
    }
}
