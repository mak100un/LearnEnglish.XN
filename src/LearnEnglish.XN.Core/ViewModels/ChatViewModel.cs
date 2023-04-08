using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using LearnEnglish.XN.Core.Definitions.DalModels;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Definitions.Exceptions;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.Repositories.Interfaces;
using LearnEnglish.XN.Core.Services.Interfaces;
using LearnEnglish.XN.Core.ViewModels.Items;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using Swordfish.NET.Collections;
using Swordfish.NET.Collections.Auxiliary;

namespace LearnEnglish.XN.Core.ViewModels;

public class ChatViewModel : BaseViewModel
{
    private const int PAGE_SIZE = 10;

    private readonly IMessagesRepository _messagesRepository;
    private readonly ICommand _selectVariantCommand;
    private readonly IMapper _mapper;

    public ICommand LoadMoreCommand { get; }

    public ChatViewModel(ITranslateService translateService, IRandomizerService randomizerService, IMessagesRepository messagesRepository, IMapper mapper, ILogger<ChatViewModel> logger)
        : base(logger)
    {
        _messagesRepository = messagesRepository;
        _mapper = mapper;

        _selectVariantCommand = new MvxAsyncCommand<VariantViewModel>(variant => CreateMvxNotifyTask(async () =>
            {
                await AddMessageAsync(_mapper.Map<MessageViewModel>(variant));

                switch (variant.VariantType)
                {
                    case VariantTypes.Answer:
                        if (variant.IsCorrect)
                        {
                            await AddMessageAsync(new MessageViewModel
                            {
                                Text = "Поздравляем! Вы правильно ответили на вопрос!",
                                MessageType = MessageTypes.Operator,
                            });
                            await DialogService.DisplaySuccessMessageAsync();
                        }
                        else
                        {
                            await AddMessageAsync(new MessageViewModel
                            {
                                Text = $"Увы! Но правильный ответ: \"{variant.CorrectText}\"",
                                MessageType = MessageTypes.Operator,
                            });
                        }

                        break;
                    case VariantTypes.Continue:
                        break;
                }

                await AddMessageWithVariantsAsync();

                async Task AddMessageWithVariantsAsync()
                {
                    IsLoading = true;

                    var newVariants = await randomizerService.GetRandomWordsAsync();
                    var translations = new ConcurrentObservableDictionary<string, string>();

                    newVariants?.ForEach(newVariant =>
                        translations.Add(new KeyValuePair<string, string>(newVariant, newVariant)));

                    // Code under commented cause key became disabled so show variants without translation.
                    /*await Task.WhenAll(newVariants.Select(v => Task.Run(async () =>
                    {
                        var translation = await translateService.TranslateAsync(v);

                        // https://yandex.ru/dev/translate/doc/dg/reference/translate.html.
                        switch (translation.Code)
                        {
                            case 401:
                                throw new LogicException("Неправильный API-ключ");

                            case 402:
                                throw new LogicException("API-ключ заблокирован");

                            case 404:
                                throw new LogicException("Превышено суточное ограничение на объем переведенного текста");

                            case 413:
                                throw new LogicException("Превышен максимально допустимый размер текста");

                            case 422:
                                throw new LogicException("Текст не может быть переведен");

                            case 501:
                                throw new LogicException("Заданное направление перевода не поддерживается");
                        }

                        if (translation.Text?.FirstOrDefault() is not { } text
                            || string.IsNullOrWhiteSpace(text))
                        {
                            return;
                        }

                        translations.Add(v, text);
                    })).ToArray());*/

                    if (translations.Count <= 1)
                    {
                        throw new LogicException("Список переводов пустой");
                    }

                    var correctVariant = translations.FirstOrDefault();

                    await AddMessageAsync(new MessageViewModel
                    {
                        Text = $"Выберите правильный перевод слова \"{correctVariant.Key}\"",
                        SelectVariantCommand = _selectVariantCommand,
                        MessageType = MessageTypes.OperatorWithVariants,
                        Variants = translations?.OrderBy(t => t.Key)
                            .Select(t => new VariantViewModel
                            {
                                IsCorrect = correctVariant.Key == t.Key,
                                VariantType = VariantTypes.Answer,
                                Text = t.Value,
                                CorrectText = correctVariant.Key,
                            })
                            .ToArray()
                    });

                    IsLoading = false;
                }
            },
            async ex =>
            {
                DialogService.ShowToast(ex is LogicException lEx ? lEx.Message : "Произошла неизвестная ошибка. Возможно отсутствует интернет соединение");
                await AddContinueMessageAsync();
                IsLoading = false;
            }).TaskCompleted, variant => Messages?.LastOrDefault()?.Variants?.Contains(variant) == true);

        LoadMoreCommand = new MvxAsyncCommand(() =>
        {
            var loading = new MessageViewModel { MessageType = MessageTypes.Loading, };
            return CreateMvxNotifyTask(async () =>
                {
                    IsLoadingMore = true;
                    Messages.Insert(0, loading);
                    LoadingOffset = 2;

                    // Imitate some work
                    await Task.Delay(5000);
                    var newItems = (await _messagesRepository.GetItemsAsync(Messages.Count, PAGE_SIZE))?.ToArray();
                    IsLoadMoreEnabled = newItems?.Length >= PAGE_SIZE;

                    if (newItems?.Length > 0)
                    {
                        Messages.InsertRange(0, _mapper.Map<IList<MessageViewModel>>(newItems.OrderBy(m => m.Id)));
                    }

                    Messages.TryRemove(loading);
                    LoadingOffset = 1;
                    IsLoadingMore = false;
                },
                ex =>
                {
                    Messages.TryRemove(loading);
                    LoadingOffset = 1;
                    IsLoadMoreEnabled = false;
                    IsLoadingMore = false;
                }).TaskCompleted;
        },() => IsLoadMoreEnabled && !IsLoadingMore);
    }

    public bool IsLoadMoreEnabled { get; set; }

    public bool IsLoadingMore { get; set; }

    public int LoadingOffset { get; private set; } = 1;

    public ConcurrentObservableCollection<MessageViewModel> Messages { get; } = new ();

    public override async Task Initialize()
    {
        await base.Initialize();

        await CreateMvxNotifyTask(async () =>
        {
            var newItems = (await _messagesRepository.GetItemsAsync(Messages.Count, PAGE_SIZE))?.ToArray();
            IsLoadMoreEnabled = newItems?.Length >= PAGE_SIZE;

            if (newItems?.Any() != true)
            {
                await AddMessageAsync(new MessageViewModel
                {
                    Text = "Добро пожаловать в приложение \"LearnEnglish.XN\"",
                    MessageType = MessageTypes.Operator,
                });
                await AddMessageAsync(new MessageViewModel
                {
                    Text = "Планируешь начать изучение English?",
                    SelectVariantCommand = _selectVariantCommand,
                    MessageType = MessageTypes.OperatorWithVariants,
                    Variants = new []
                    {
                        new VariantViewModel
                        {
                            VariantType = VariantTypes.Continue,
                            Text = "Of course",
                        }
                    }
                });
                return;
            }

            var messages = _mapper.Map<IList<MessageViewModel>>(newItems.OrderBy(m => m.Id));
            var lastMessage = messages.Last();
            Messages.InsertRange(0, messages);

            if (!lastMessage.IsMine && lastMessage.MessageType == MessageTypes.OperatorWithVariants)
            {
                lastMessage.SelectVariantCommand = _selectVariantCommand;
                return;
            }

            await AddContinueMessageAsync();
        }).TaskCompleted;
    }

    private Task AddContinueMessageAsync() => AddMessageAsync(new MessageViewModel
    {
        Text = "Продолжить изучение English?",
        SelectVariantCommand = _selectVariantCommand,
        MessageType = MessageTypes.OperatorWithVariants,
        Variants = new []
        {
            new VariantViewModel
            {
                VariantType = VariantTypes.Continue,
                Text = "Of course",
            }
        }
    });

    private async Task AddMessageAsync(MessageViewModel message)
    {
        await _messagesRepository.InsertAsync(_mapper.Map<MessageDalModel>(message));
        Messages.Add(message);
    }
}
