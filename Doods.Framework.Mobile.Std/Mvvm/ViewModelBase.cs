using System;
using System.Threading;
using System.Threading.Tasks;
using Doods.Framework.Std;

namespace Doods.Framework.Mobile.Std.Mvvm
{
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        private const int _waitingDurationIsSeconds = 200;

        private int _busyCount;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isLoaded;
        private bool _isLoading;
        private bool _isVisible;

        private ViewModelState _viewModelState;

        protected ViewModelBase(ILogger logger)
        {
            Logger = logger;
            NotifyLoading = true;
        }

        /// <summary>
        ///     Permet de définir un temps d'attente avant le lancement du chargement
        /// </summary>
        protected int? WaitingDurationIsSecond { get; set; }

        protected ILogger Logger { get; }

        /// <summary>
        ///     Indique si la progression doit être notifié
        /// </summary>
        protected bool NotifyLoading { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            protected set => SetProperty(ref _isLoading, value);
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }

        public bool IsBusy => BusyCount > 0;

        protected int BusyCount
        {
            get => _busyCount;
            set
            {
                SetProperty(ref _busyCount, value);
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public ViewModelState ViewModelState
        {
            get => _viewModelState;
            set => SetProperty(ref _viewModelState, value);
        }

        protected CancellationToken Token => _cts.Token;

        /// <summary>
        ///     Permet de lancer le chargement du ViewModel
        /// </summary>
        /// <param name="context">Contexte de chargement</param>
        /// <returns></returns>
        public async Task StartLoading(LoadingContext context)
        {
            if (IsLoading) return;

            IsLoading = true;
            ViewModelState = ViewModelState.Loading;

            TimeTracer tracer = null;
            ITimeWatcher timer;

            if (context.IsValid)
            {
                timer = context.Timer;
            }
            else
            {
                tracer = new TimeTracer(Logger);
                timer = tracer.Timer;
            }

            using (var watch = timer.StartWatcher("StartLoading"))
            {
                watch?.Descriptions?.Add("type", GetType());

                // Préparation du chargement
                InitializeLoading(context);

                // Permet d'éviter les freezes de l'UI en navigation
                var duration = WaitingDurationIsSecond ?? _waitingDurationIsSeconds;
                if (duration > 0)
                    await Task.Delay(TimeSpan.FromMilliseconds(duration));

                try
                {
                    // On poursuit uniquement si le token n'est pas déjà annulé 
                    // par un changement de page par exemple
                    if (!Token.IsCancellationRequested)
                        await ExecuteAsync(token =>
                        {
                            var ctx = context.IsValid ? context : LoadingContext.Create(context, token, timer);
                            return Load(ctx);
                        });

                    ViewModelState = ViewModelState.Loaded;
                }
                catch (Exception e)
                {
                    ViewModelState = ViewModelState.Failed;
                    Logger.Error(e);
                }
                finally
                {
                    // Finalisation du chargement
                    FinishLoading(context);
                }
            }

            tracer?.Dispose();

            IsLoading = false;
        }

        private void InitializeLoading(LoadingContext context)
        {
            IsLoaded = false;

            var notify = context.NotifyUser || NotifyLoading;
            if (notify)
                BusyCount++;

            OnInitializeLoading(context);
        }

        private void FinishLoading(LoadingContext context)
        {
            var notify = context.NotifyUser || NotifyLoading;
            if (notify) BusyCount--;

            if (!Token.IsCancellationRequested)
            {
                OnFinishLoading(context);
                IsLoaded = true;
            }
        }

        protected virtual Task Load(LoadingContext context)
        {
            return Task.FromResult(0);
        }

        protected virtual void OnFinishLoading(LoadingContext context)
        {
            //NP
        }

        protected virtual void OnInitializeLoading(LoadingContext context)
        {
            //NP
        }

        public async Task OnAppearing()
        {
            if (_cts.IsCancellationRequested) _cts = new CancellationTokenSource();

            if (!IsLoaded && !IsLoading && ViewModelState != ViewModelState.Loading)
                await StartLoading(LoadingContext.OnAppearing);

            IsVisible = true;
            await OnInternalAppearing();
        }

        protected virtual Task OnInternalAppearing()
        {
            return Task.FromResult(0);
        }

        public Task OnDisappearing()
        {
            IsVisible = false;
            CancelExecutions();
            return OnInternalDisappearing();
        }

        protected virtual Task OnInternalDisappearing()
        {
            return Task.FromResult(0);
        }

        protected async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, bool safeExecution = false)
        {
            try
            {
                Token.ThrowIfCancellationRequested();
                return await action(Token);
            }
            catch (AggregateException e)
            {
                var innerException = e.InnerException;
                while (innerException.InnerException != null)
                    innerException = innerException.InnerException;

                Logger.Error(innerException);
                //TODO : HokeyApp
                if (!safeExecution) throw;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                //TODO : HokeyApp
                if (!safeExecution) throw;
            }

            return default(T);
        }

        protected async Task ExecuteAsync(Func<CancellationToken, Task> action, bool safeExecution = false)
        {
            try
            {
                Token.ThrowIfCancellationRequested();
                await action(Token);
            }
            catch (AggregateException e)
            {
                var innerException = e.InnerException;
                while (innerException.InnerException != null)
                    innerException = innerException.InnerException;

                //TODO : LOG + HokeyApp
                if (!safeExecution) throw;
            }
            catch (Exception e)
            {
                //TODO : LOG + HokeyApp
                var msg = e.Message;
                if (!safeExecution) throw;
            }
        }

        public void CancelExecutions()
        {
            if (Token.CanBeCanceled && !_cts.IsCancellationRequested)
                _cts.Cancel();

            // TODO : Recréer un nouveau _cts ?
        }
    }
}