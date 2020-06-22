using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine.Networking;

namespace SampleMapBuilder.Infra
{
    public static class ObservableRequestExtension
    {
        public static IObservable<UnityWebRequest> ToRequestObservable(
            this UnityWebRequest request,
            IProgress<float> progress = null)
        {
            return Observable.FromCoroutine<UnityWebRequest>((observer, cancellation) =>
                Fetch(request, null, observer, progress, cancellation));
        }

        public static IObservable<string> ToRequestTextObservable(
            this UnityWebRequest request,
            IProgress<float> progress = null)
        {
            return Observable.FromCoroutine<string>((observer, cancellation) =>
                FetchText(request, null, observer, progress, cancellation));
        }

        public static IObservable<byte[]> ToRequestBinaryObservable(
            this UnityWebRequest request,
            IProgress<float> progress = null)
        {
            return Observable.FromCoroutine<byte[]>((observer, cancellation) =>
                FetchBinary(request, null, observer, progress, cancellation));
        }

        private static IEnumerator Fetch<T>(
            UnityWebRequest request,
            IDictionary<string, string> headers,
            IObserver<T> observer,
            IProgress<float> reportProgress,
            CancellationToken cancel)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            if (reportProgress != null)
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone && !cancel.IsCancellationRequested)
                {
                    try
                    {
                        reportProgress.Report(operation.progress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }

                    yield return null;
                }
            }
            else
            {
                yield return request.SendWebRequest();
            }


            if (cancel.IsCancellationRequested)
            {
                yield break;
            }

            if (reportProgress != null)
            {
                try
                {
                    reportProgress.Report(request.downloadProgress);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    yield break;
                }
            }
        }

        private static IEnumerator FetchText(
            UnityWebRequest request,
            IDictionary<string, string> headers,
            IObserver<string> observer,
            IProgress<float> reportProgress,
            CancellationToken cancel)
        {
            using (request)
            {
                yield return Fetch(request, headers, observer, reportProgress, cancel);

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (!string.IsNullOrEmpty(request.error))
                {
                    observer.OnError(new Exception($"{request.url} -> {request.error}"));
                }
                else
                {
                    var text = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                    observer.OnNext(text);
                    observer.OnCompleted();
                }
            }
        }

        private static IEnumerator FetchBinary(
            UnityWebRequest request,
            IDictionary<string, string> headers,
            IObserver<byte[]> observer,
            IProgress<float> reportProgress,
            CancellationToken cancel)
        {
            using (request)
            {
                yield return Fetch(request, headers, observer, reportProgress, cancel);

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (!string.IsNullOrEmpty(request.error))
                {
                    observer.OnError(new Exception($"{request.url} -> {request.error}"));
                }
                else
                {
                    observer.OnNext(request.downloadHandler.data);
                    observer.OnCompleted();
                }
            }
        }
    }
}