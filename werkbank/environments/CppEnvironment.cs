using werkbank.models;

namespace werkbank.environments
{
    public class CppEnvironment : Environment
    {
        public CppEnvironment(int Index) : base(Index) { }

        public override string Name => "C++";
        public override string Handle => "cpp";
        public override string Directory => "code\\cpp";

        public override EnvironmentPreset Preset => new(
            CompressOnArchive: true
        );
    }
}
