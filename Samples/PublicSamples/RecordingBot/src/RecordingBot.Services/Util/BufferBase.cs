// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 09-07-2020
// ***********************************************************************
// <copyright file="BufferBase.cs" company="Microsoft">
//     Copyright ©  2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RecordingBot.Services.Util
{
    /// <summary>
    /// Class BufferBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BufferBase<T>
    {
        protected readonly BufferBlock<T> _buffer;
        private readonly CancellationTokenSource _tokenSource;
        private bool _isRunning;
        private readonly SemaphoreSlim _syncLock = new(1);

        protected BufferBase()
        {
            _buffer = new BufferBlock<T>();
            _tokenSource = new CancellationTokenSource();
        }

        protected BufferBase(CancellationTokenSource token)
        {
            _buffer = new BufferBlock<T>();
            _tokenSource = token;
        }

        public async Task Append(T obj)
        {
            if (!_isRunning)
            {
                await Start();
            }

            try
            {
                await _buffer.SendAsync(obj, _tokenSource.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException e)
            {
                _buffer?.Complete();

                Debug.Write($"Cannot enqueue because queuing operation has been cancelled. Exception: {e}");
            }
        }

        private async Task Start()
        {
            await _syncLock.WaitAsync().ConfigureAwait(false);
            if (!_isRunning)
            {
                await Task.Factory.StartNew(Process).ConfigureAwait(false);
                _isRunning = true;
            }
            _syncLock.Release();
        }

        public virtual async Task End()
        {
            if (_isRunning)
            {
                await _syncLock.WaitAsync().ConfigureAwait(false);
                if (_isRunning)
                {
                    _buffer.Complete();
                    _isRunning = false;
                }
                _syncLock.Release();
            }
        }

        private async Task Process()
        {
            try
            {
                while (await _buffer.OutputAvailableAsync(_tokenSource.Token).ConfigureAwait(false))
                {
                    T data = await _buffer.ReceiveAsync(_tokenSource.Token).ConfigureAwait(false);

                    await Task.Run(() => Process(data)).ConfigureAwait(false);

                    _tokenSource.Token.ThrowIfCancellationRequested();
                }
            }
            catch (TaskCanceledException ex)
            {
                Debug.Write($"The queue processing task has been cancelled. Exception: {ex}");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.Write($"The queue processing task object has been disposed. Exception: {ex}");
            }
            catch (Exception ex)
            {
                Debug.Write($"Caught Exception: {ex}");

                await Process().ConfigureAwait(false);
            }
        }

        protected abstract Task Process(T data);
    }
}