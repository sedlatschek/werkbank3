using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using werkbank.exceptions;

namespace werkbank.operations
{
    public enum OperationType
    {
        Copy,
        MoveFile,
        Delete,
        Hide,
        Unhide,
        Write,
        Zip,
        Unzip,
    }

    public class OperationOptions { }

    public class OperationCopyOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "source")]
        public readonly string SourcePath;

        [JsonProperty(PropertyName = "dest")]
        public readonly string DestinationPath;

        public OperationCopyOptions(string SourcePath, string DestinationPath)
        {
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }
    }

    public class OperationMoveFileOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "source")]
        public readonly string SourcePath;

        [JsonProperty(PropertyName = "dest")]
        public readonly string DestinationPath;

        public OperationMoveFileOptions(string SourcePath, string DestinationPath)
        {
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }
    }

    public class OperationDeleteOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "target")]
        public readonly string TargetPath;

        public OperationDeleteOptions(string TargetPath)
        {
            this.TargetPath = TargetPath;
        }
    }

    public class OperationHideOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "target")]
        public readonly string TargetPath;

        public OperationHideOptions(string TargetPath)
        {
            this.TargetPath = TargetPath;
        }
    }

    public class OperationUnhideOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "target")]
        public readonly string TargetPath;

        public OperationUnhideOptions(string TargetPath)
        {
            this.TargetPath = TargetPath;
        }
    }

    public class OperationWriteOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "dest")]
        public readonly string DestinationPath;

        [JsonProperty(PropertyName = "content")]
        public readonly string Content;

        public OperationWriteOptions(string DestinationPath, string Content)
        {
            this.DestinationPath = DestinationPath;
            this.Content = Content;
        }
    }

    public class OperationZipOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "source")]
        public readonly string SourcePath;

        [JsonProperty(PropertyName = "dest")]
        public readonly string DestinationPath;

        public OperationZipOptions(string SourcePath, string DestinationPath)
        {
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }
    }

    public class OperationUnzipOptions : OperationOptions
    {
        [JsonProperty(PropertyName = "source")]
        public readonly string SourceZip;

        [JsonProperty(PropertyName = "dest")]
        public readonly string DestinationPath;

        public OperationUnzipOptions(string SourceZip, string DestinationPath)
        {
            this.SourceZip = SourceZip;
            this.DestinationPath = DestinationPath;
        }
    }

    public class Operation
    {
        [JsonProperty(PropertyName = "type")]
        protected OperationType _type;
        public OperationType Type => _type;

        [JsonProperty(PropertyName = "data")]
        protected OperationOptions _options;
        public OperationOptions Options => _options;

        public Operation(OperationType Type, OperationOptions Options)
        {
            _type = Type;
            _options = Options;
        }

        public bool Run()
        {
            return _type switch
            {
                OperationType.Copy => Copy.Perform((OperationCopyOptions)Options),
                OperationType.MoveFile => MoveFile.Perform((OperationMoveFileOptions)Options),
                OperationType.Delete => Delete.Perform((OperationDeleteOptions)Options),
                OperationType.Hide => Hide.Perform((OperationHideOptions)Options),
                OperationType.Unhide => Unhide.Perform((OperationUnhideOptions)Options),
                OperationType.Write => Write.Perform((OperationWriteOptions)Options),
                OperationType.Zip => Zip.Perform((OperationZipOptions)Options),
                OperationType.Unzip => Unzip.Perform((OperationUnzipOptions)Options),
                _ => throw new UnhandledOperationTypeException(_type),
            };
        }

        public bool Verify()
        {
            return _type switch
            {
                OperationType.Copy => Copy.Verify((OperationCopyOptions)Options),
                OperationType.MoveFile => MoveFile.Verify((OperationMoveFileOptions)Options),
                OperationType.Delete => Delete.Verify((OperationDeleteOptions)Options),
                OperationType.Hide => Hide.Verify((OperationHideOptions)Options),
                OperationType.Unhide => Unhide.Verify((OperationUnhideOptions)Options),
                OperationType.Write => Write.Verify((OperationWriteOptions)Options),
                OperationType.Zip => Zip.Verify((OperationZipOptions)Options),
                OperationType.Unzip => Unzip.Verify((OperationUnzipOptions)Options),
                _ => throw new UnhandledOperationTypeException(_type),
            };
        }
    }
}
