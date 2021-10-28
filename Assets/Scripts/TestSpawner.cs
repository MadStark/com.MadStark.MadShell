using MadStark.MadShell;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [Command("spawn")]
    private static void SpawnFromCommand(string[] args)
    {
        if (args.Length < 1)
        {
            Debug.Log("Usage: spawn <name> [count]");
            return;
        }

        string name = args[0];
        GameObject go = Resources.Load<GameObject>(name);

        if (go == null)
        {
            Debug.LogError($"Object '{name}' not found in Resources folder.");
            return;
        }

        int count = 1;

        if (args.Length >= 2 && int.TryParse(args[1], out int c))
            count = c;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 10f;
            Instantiate(go, pos, Quaternion.identity);
        }
    }
}
