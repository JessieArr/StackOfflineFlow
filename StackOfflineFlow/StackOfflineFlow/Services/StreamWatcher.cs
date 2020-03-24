using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StackOfflineFlow.Services
{
    public class StreamWatcher : Stream
    {
        private Stream _StreamToWatch;
        public StreamWatcher(Stream streamToWatch)
        {
            _StreamToWatch = streamToWatch;
        }

        public override bool CanRead
        {
            get
            {
                return _StreamToWatch.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return _StreamToWatch.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return _StreamToWatch.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return _StreamToWatch.Length;
            }
        }

        public override long Position { 
            get 
            {
                return _StreamToWatch.Position;
            } 
            set {
                _StreamToWatch.Position = value;
            }

        }

        public override void Flush()
        {
            _StreamToWatch.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _StreamToWatch.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _StreamToWatch.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _StreamToWatch.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _StreamToWatch.Write(buffer, offset, count);
        }
    }
}
