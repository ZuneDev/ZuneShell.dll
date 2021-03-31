// Decompiled with JetBrains decompiler
// Type: Microsoft.Iris.Session.ErrorManager
// Assembly: UIX, Version=4.8.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: A56C6C9D-B7F6-46A9-8BDE-B3D9B8D60B11
// Assembly location: C:\Program Files\Zune\UIX.dll

using Microsoft.Iris.Markup;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Iris.Session
{
    internal static class ErrorManager
    {
        private static uint s_totalErrorsReported;
        private static Stack<ErrorManager.Context> s_contextStack = new Stack<ErrorManager.Context>();
        private static uint s_ignoringErrorsDepth;
        private static bool s_errorBatchPending;
        private static IList s_errors;
        private static readonly SimpleCallback s_drainErrorQueueHandler = new SimpleCallback(ErrorManager.OnDrainErrorQueue);

        public static ErrorWatermark Watermark => new ErrorWatermark(ErrorManager.s_totalErrorsReported);

        public static void EnterContext(object contextObject) => ErrorManager.EnterContext(contextObject, false);

        public static void EnterContext(object contextObject, bool ignoreErrors) => ErrorManager.EnterContext(new ErrorManager.Context(contextObject, ignoreErrors));

        public static void EnterContext(IErrorContextSource contextSource) => ErrorManager.EnterContext(new ErrorManager.Context(contextSource));

        private static void EnterContext(ErrorManager.Context context)
        {
            if (context.IgnoreErrors)
                ++ErrorManager.s_ignoringErrorsDepth;
            ErrorManager.s_contextStack.Push(context);
        }

        public static string CurrentContext
        {
            get
            {
                string str = null;
                if (ErrorManager.s_contextStack.Count != 0)
                    str = ErrorManager.s_contextStack.Peek().ToString();
                return str;
            }
        }

        public static bool IgnoringErrors => ErrorManager.s_ignoringErrorsDepth > 0U;

        public static void ExitContext()
        {
            ErrorManager.Context context = ErrorManager.s_contextStack.Peek();
            if (context.IgnoreErrors)
            {
                ErrorManager.s_totalErrorsReported = context.TotalErrorsOnEnter;
                --ErrorManager.s_ignoringErrorsDepth;
            }
            ErrorManager.s_contextStack.Pop();
        }

        public static uint TotalErrorsReported => ErrorManager.s_totalErrorsReported;

        public static event NotifyErrorBatch OnErrors;

        public static IList GetErrors()
        {
            IList errors = ErrorManager.s_errors;
            ErrorManager.s_errors = null;
            return errors;
        }

        public static void ReportError(string message) => ErrorManager.TrackReportWorker(-1, -1, false, message);

        public static void ReportError(string format, object param) => ErrorManager.TrackReport(-1, -1, false, format, param);

        public static void ReportError(string format, object param1, object param2) => ErrorManager.TrackReport(-1, -1, false, format, param1, param2);

        public static void ReportError(string format, object param1, object param2, object param3) => ErrorManager.TrackReport(-1, -1, false, format, param1, param2, param3);

        public static void ReportError(
          string format,
          object param1,
          object param2,
          object param3,
          object param4)
        {
            ErrorManager.TrackReport(-1, -1, false, format, param1, param2, param3, param4);
        }

        public static void ReportError(
          string format,
          object param1,
          object param2,
          object param3,
          object param4,
          object param5)
        {
            ErrorManager.TrackReport(-1, -1, false, format, param1, param2, param3, param4, param5);
        }

        public static void ReportError(int line, int column, string message) => ErrorManager.TrackReportWorker(line, column, false, message);

        public static void ReportError(int line, int column, string format, object param) => ErrorManager.TrackReport(line, column, false, format, param);

        public static void ReportError(
          int line,
          int column,
          string format,
          object param1,
          object param2)
        {
            ErrorManager.TrackReport(line, column, false, format, param1, param2);
        }

        public static void ReportWarning(string message) => ErrorManager.TrackReportWorker(-1, -1, true, message);

        public static void ReportWarning(string format, object param) => ErrorManager.TrackReport(-1, -1, true, format, param);

        public static void ReportWarning(string format, object param1, object param2) => ErrorManager.TrackReport(-1, -1, true, format, param1, param2);

        public static void ReportWarning(int line, int column, string message) => ErrorManager.TrackReportWorker(line, column, true, message);

        public static void ReportWarning(int line, int column, string format, object param) => ErrorManager.TrackReport(line, column, true, format, param);

        private static void TrackReportWorker(int line, int column, bool warning, string message)
        {
            if (!ErrorManager.IgnoringErrors)
            {
                string str = null;
                if (ErrorManager.s_contextStack.Count != 0)
                {
                    ErrorManager.Context context = ErrorManager.s_contextStack.Peek();
                    str = context.Description;
                    if (line == -1 && column == -1)
                        context.GetErrorPosition(ref line, ref column);
                }
                ErrorRecord errorRecord = new ErrorRecord();
                errorRecord.Context = str;
                errorRecord.Line = line;
                errorRecord.Column = column;
                errorRecord.Warning = warning;
                errorRecord.Message = message;
                if (ErrorManager.s_errors == null)
                    ErrorManager.s_errors = new ArrayList();
                ErrorManager.s_errors.Add(errorRecord);
                ErrorManager.QueueNotify();
            }
            if (warning)
                return;
            ++ErrorManager.s_totalErrorsReported;
        }

        public static void TrackReport(
          int line,
          int column,
          bool warning,
          string format,
          object param)
        {
            string message = null;
            if (!ErrorManager.IgnoringErrors)
                message = string.Format(format, param);
            ErrorManager.TrackReportWorker(line, column, warning, message);
        }

        public static void TrackReport(
          int line,
          int column,
          bool warning,
          string format,
          object param1,
          object param2)
        {
            string message = null;
            if (!ErrorManager.IgnoringErrors)
                message = string.Format(format, param1, param2);
            ErrorManager.TrackReportWorker(line, column, warning, message);
        }

        public static void TrackReport(
          int line,
          int column,
          bool warning,
          string format,
          object param1,
          object param2,
          object param3)
        {
            string message = null;
            if (!ErrorManager.IgnoringErrors)
                message = string.Format(format, param1, param2, param3);
            ErrorManager.TrackReportWorker(line, column, warning, message);
        }

        public static void TrackReport(
          int line,
          int column,
          bool warning,
          string format,
          object param1,
          object param2,
          object param3,
          object param4)
        {
            string message = null;
            if (!ErrorManager.IgnoringErrors)
                message = string.Format(format, param1, param2, param3, param4);
            ErrorManager.TrackReportWorker(line, column, warning, message);
        }

        public static void TrackReport(
          int line,
          int column,
          bool warning,
          string format,
          object param1,
          object param2,
          object param3,
          object param4,
          object param5)
        {
            string message = null;
            if (!ErrorManager.IgnoringErrors)
                message = string.Format(format, param1, param2, param3, param4, param5);
            ErrorManager.TrackReportWorker(line, column, warning, message);
        }

        private static void QueueNotify()
        {
            if (ErrorManager.s_errorBatchPending)
                return;
            UIDispatcher currentDispatcher = UIDispatcher.CurrentDispatcher;
            if (currentDispatcher != null && currentDispatcher.UISession != null)
            {
                ErrorManager.s_errorBatchPending = true;
                DeferredCall.Post(DispatchPriority.AppEventHigh, ErrorManager.s_drainErrorQueueHandler);
            }
            else
                ErrorManager.OnDrainErrorQueue();
        }

        private static void OnDrainErrorQueue()
        {
            ErrorManager.s_errorBatchPending = false;
            IList errors = ErrorManager.GetErrors();
            if (ErrorManager.OnErrors == null)
                return;
            ErrorManager.OnErrors(errors);
        }

        internal struct Context
        {
            private object _contextObject;
            private bool _ignoreErrors;
            private uint _errorCountOnEnter;
            private IErrorContextSource _callback;

            public Context(object contextObject, bool ignoreErrors)
            {
                this._contextObject = contextObject;
                this._callback = null;
                this._ignoreErrors = ignoreErrors;
                this._errorCountOnEnter = ErrorManager.s_totalErrorsReported;
            }

            public Context(IErrorContextSource contextSource)
            {
                this._callback = contextSource;
                this._contextObject = null;
                this._ignoreErrors = false;
                this._errorCountOnEnter = ErrorManager.s_totalErrorsReported;
            }

            public string Description
            {
                get
                {
                    string str = null;
                    if (this._callback != null)
                        str = this._callback.GetErrorContextDescription();
                    else if (this._contextObject != null)
                    {
                        if (this._contextObject is string)
                            str = (string)this._contextObject;
                        else if (this._contextObject is TypeSchema)
                            str = ((TypeSchema)this._contextObject).ErrorContextDescription;
                    }
                    return str;
                }
            }

            public void GetErrorPosition(ref int line, ref int column)
            {
                if (this._callback == null)
                    return;
                this._callback.GetErrorPosition(ref line, ref column);
            }

            public bool IgnoreErrors => this._ignoreErrors;

            public uint TotalErrorsOnEnter => this._errorCountOnEnter;

            public override string ToString() => this._callback != null ? this._callback.ToString() : this.Description;
        }
    }
}
