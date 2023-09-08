namespace Root.Scripts.EventHandling.Base
{
    public enum InputChannels
    {
        Null = 0,
        TouchStartedEc = 10,
        TouchMovedEc = 20,
        TouchStationaryEc = 30,
        TouchEndedEc = 40,
    }
    

    public enum LevelChannels
    {
        Null = 0,
        LevelLoadEc = 10,
        LevelReadyEc = 20,
        LevelStartEc = 30,
        LevelEndEc = 40,
        LevelSuccessfulEc = 50,
        LevelFailedEc = 60,
    }
    
    public enum ScoreChannels
    {
        Null = 0,
        ScoreChangedEc = 10,
        ScoreMultiplierChangedEc = 20
    }

    public enum GameplayChannels
    {
        Null = 0,
        TappedPadEc = 10,
        SwipedPadEc = 20,
        StackSizeChangedEc = 30,
        StackSwapCompletedEc = 35,
        StackMergeCompletedEc = 40,
        LimitReachedEc = 50,
    }
}