using Quartz;

namespace RhythmRivals.BLL.Helpers;
public static class TriggerKeyHelper
{
    public static TriggerKey CreateRevealAnswerKey(string gameId)
    {
        return new TriggerKey($"{gameId}-areveal");
    }

    public static TriggerKey CreateDistributeAnswerKey(string gameId)
    {
        return new TriggerKey($"{gameId}-qdist");
    }
}
