public struct RoutineInfo
{
    public bool DayHasFinished { get; private set; }

    public bool WeekHasFinished { get; private set; }

    public bool RoutineHasFinished { get; private set; }

    public RoutineInfo (bool dayHasFinished)
    {
        DayHasFinished = dayHasFinished;
        WeekHasFinished = false;
        RoutineHasFinished = false;
    }

    public RoutineInfo (bool dayHasFinished, bool weekHasFinished)
    {
        DayHasFinished = dayHasFinished;
        WeekHasFinished = weekHasFinished;
        RoutineHasFinished = false;
    }

    public RoutineInfo (bool dayHasFinished, bool weekHasFinished, bool routineHasFinished)
    {
        DayHasFinished = dayHasFinished;
        WeekHasFinished = weekHasFinished;
        RoutineHasFinished = routineHasFinished;
    }
}
