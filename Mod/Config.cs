namespace EfficientQuesting;

public class EfficientQuestingConfig
{
    public bool IsEnabled = true;

#if DEBUG
    public bool IsDebug = true;
#else
    public bool IsDebug;
#endif
}