using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveGame() {
        ShipPersistenceController.BackupAllShips();
        DockingField.SaveLocation();
        QuestManager.SaveQuests();
        JobManager.SaveJobs();
    }
}
