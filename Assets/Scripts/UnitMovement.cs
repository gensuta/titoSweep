using System.Collections;
using System.Collections.Generic;

public interface UnitMovement 
{
    UnitBehaviour SelectedUnit { get; set; }

    PathFinder pathFinder { get; set; }
    List<TileBehavior> path { get; set; }
    float speed { get; set; }

    void SelectUnit(UnitBehaviour unit);

    void SetPath(TileBehavior end);

    void MoveAlongPath();

    List<TileBehavior> GetTilesWithinRange(); // based on characters move stat and the tiles move cost
}
