using System;

namespace GameContracts
{
public interface IPauseStateSource
{
    event Action<bool> GamePaused;
}

public interface IGameUIVisibilitySource
{
    event Action<bool> ShowingGameUI;
}

public interface IPauseControl
{
    void SetPauseEnabled(bool value);
}

public interface IGameUIVisibilityControl
{
    void SetGameUIVisibility(bool isVisible);
}

public interface IDeadAnimationSource
{
    event Action<bool> DeadFinished;
}
}