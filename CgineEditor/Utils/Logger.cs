using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace CgineEditor.Utils
{

    enum MessageType
    {
        Info = 0x01,
        Warning = 0x02,
        Error = 0x03,
    }

    class LogMessage
    {
        public DateTime Time { get; }
        public MessageType MessageType { get; }
        public String Message { get; }
        public string File { get; }
        public string Caller { get; }
        public int Line { get; }
        public string MetaData => $"{File}: {Caller} ({Line})";

        public LogMessage(MessageType type, string msg, string file, string caller, int line)
        {
            Time = DateTime.Now;
            MessageType = type;
            Message = msg;
            File = Path.GetFileName(file);
            Caller = caller;
            Line = line;
        }

    }

    class Logger
    {
        private static readonly ObservableCollection<LogMessage> _messages = new ObservableCollection<LogMessage>();

        public static ReadOnlyObservableCollection<LogMessage> Messages { get; }

    }
}
