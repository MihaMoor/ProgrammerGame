using Server.Shared.Errors;

namespace Server.Shared.Cqrs;

public interface ICommand { }

public interface ICommand<out TResponse> : ICommand { }

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Обрабатывает указанный команду асинхронно.
    /// </summary>
    /// <param name="command">Команда, которую необходимо обработать.</param>
    /// <param name="cancellationToken">Токен для отслеживания запросов отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию, содержащая результат выполнения команды.</returns>
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Обрабатывает команду, ожидающую ответ, выполняя ее асинхронно с поддержкой отмены.
    /// </summary>
    /// <param name="command">Команда для обработки.</param>
    /// <param name="cancellationToken">Токен для отслеживания запросов отмены.</param>
    /// <returns>Задача, представляющая асинхронную операцию, содержащая результат и данные ответа.</returns>
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
